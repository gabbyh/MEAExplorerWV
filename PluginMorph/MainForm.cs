using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PluginSystem;
using Be.Windows.Forms;

namespace PluginMorph
{
    public partial class MainForm : Form
    {
        // morph res type
        public List<uint> morphAssetTypes = new List<uint>(new uint[] { 0xEB228507 });

        // data access
        public MainClass main;
        public List<string> tocFiles;
        public Dictionary<string, List<string>> bundlePaths = new Dictionary<string,List<string>>();
        public Dictionary<string, List<ChunkInfo>> tocChunks;
        public string currBundle;
        public List<DataInfo> res = new List<DataInfo>();
        public List<ChunkInfo> chunks = new List<ChunkInfo>();
        public List<ChunkInfo> globalChunks = new List<ChunkInfo>();
        
        private Dictionary<string, List<DataInfo>> resByBundles = new Dictionary<string,List<DataInfo>>();

        public string currPath = "";
        public string currToc = "";
        public byte[] rawEbxBuffer;
        public byte[] rawResBuffer;

        public MainForm()
        {
            InitializeComponent();
        }

        // toggles Enabled property on menus and controls of the form.
        private void ActivateControls(bool active)
        {
            menuStrip1.Enabled = active;
            fileToolStripMenuItem.Enabled = active;
            toolStrip1.Enabled = active;
            toolStrip2.Enabled = active;
            tabControl1.Enabled = active;
        }

        private void ActivateExportControls(bool active)
        {
            exportRawBufferToolStripMenuItem.Enabled = active;
            exportResRawBufferToolStripMenuItem.Enabled = active;
            exportMorphToolStripFileMenuItem.Enabled = active;
            exportMorphToolStripMenuItem.Enabled = active;
        }

        //
        // form loading functions
        //
        public void MainForm_Load()
        {
            // set autopatching to false : for some reason, the morph res is fucked up when patched.
            main.Host.setAutoPatching(false);

            // load ebx guids (needed to get base mesh)
            EbxUtils.ebxGuid = main.Host.getEBXGuids();

            tocChunks = new Dictionary<string, List<ChunkInfo>>();
            globalChunks = new List<ChunkInfo>();

            // check if cache was already generated and load from there if so.
            if (PluginMorphCache.CheckForCache())
            {
                bundlePaths = PluginMorphCache.LoadFromCache();
                tocFiles = new List<string>();
                tocFiles.AddRange(bundlePaths.Keys);
                LoadChunks();
                RefreshTocTree();
                ActivateControls(true);
            }
            else
            {
                // if no cache, scan for content (ie scan for toc and bundles containing morph static assets)
                ScanForContent();
            }            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ActivateControls(false);
            TreeNode tempNode = new TreeNode("Loading, please wait...");
            tv1.Nodes.Add(tempNode);
            MainForm_Load();
        }

        // scan for content when no cache data is found (or when scan button is hit)
        private void ScanForContent()
        {
            tocFiles = main.Host.getTOCFiles();
            StatusBarProgressBar.Maximum = tocFiles.Count;
            StatusBarProgressBar.Value = 0;
            LoadBackgroundWorker.RunWorkerAsync();
        }

        // Background worker functions (used for scanning content)
        //

        // display only toc which have bundles containing morph res
        // ignore patch toc
        private void LoadBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int progress = 0;
            var tocToRemove = new List<string>();
            foreach (string toc in tocFiles)
            {
                LoadBackgroundWorker.ReportProgress(progress);
                try
                {
                    if (toc.Contains("Patch"))
                    {
                        // ignore Patch data
                        tocToRemove.Add(toc);
                    }
                    else
                    {
                        var TocValidBundles = FilterBundles(toc);
                        if (TocValidBundles.Count > 0)
                        {
                            bundlePaths.Add(toc, TocValidBundles);
                        }
                        else
                        {
                            tocToRemove.Add(toc);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                progress++;
            }
            tocToRemove.ForEach(t => tocFiles.Remove(t));
        }

        private void LoadBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            StatusBarProgressBar.Value = e.ProgressPercentage;
            string currentTocFile = Path.GetFileName(tocFiles[e.ProgressPercentage]);
            StatusBarLabel.Text = "Searching file " + currentTocFile + "...";
        }

        private void LoadBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusBarLabel.Text = "Writing cache...";
            PluginMorphCache.WriteCache(bundlePaths);
            StatusBarLabel.Text = "Loading chunks...";
            LoadChunks();
            StatusBarLabel.Text = "";
            StatusBarProgressBar.Value = 0;
            RefreshTocTree();
            ActivateControls(true);
        }
        // end background worker.

        //
        // Load/filter data functions
        //

        // returns the list of bundles of this toc which contain morph res data
        private List<string> FilterBundles(string toc)
        {
            List<string> bundlesToc = main.Host.getBundleNames(toc);
            List<string> bundlesToKeep = new List<string>();
            foreach (string bundle in bundlesToc)
            {
                List<DataInfo> currRes = main.Host.getAllRES(toc, bundle);
                List<DataInfo> morphRes = currRes.FindAll(di => morphAssetTypes.Contains((uint)di.type));

                if (morphRes.Count > 0)
                {
                    // check if this bundles was already added : we keep the original data 
                    if (!resByBundles.ContainsKey(bundle))
                    {
                        resByBundles.Add(bundle, morphRes);
                    }                  
                    bundlesToKeep.Add(bundle);
                }
            }
            return bundlesToKeep;
        }

        // load all morph res of given bundle/toc and add it to global dictionnary resByBundles if this bundle contains any res morph data.
        private void LoadBundle(string toc, string bundle)
        {
            List<DataInfo> currRes = main.Host.getAllRES(toc, bundle);
            List<DataInfo> morphRes = currRes.FindAll(di => morphAssetTypes.Contains((uint)di.type));

            if (morphRes.Count > 0)
            {
                resByBundles.Add(bundle, morphRes);
            }           
        }

        private void LoadChunks()
        {
            foreach (string toc in tocFiles)
            {
                tocChunks.Add(toc, main.Host.getAllTocCHUNKs(toc));
                if (toc.Contains("chunks"))
                    globalChunks.AddRange(main.Host.getAllTocCHUNKs(toc));
            }
        }

        // load ebx corresponding to given res path (ie ebx by the same path since morph static consists of one ebx+one res with the same path
        private DataInfo LoadMorphEbx(string resPath)
        {
            var allEbx = main.Host.getAllEBX(currToc, currBundle);
            return allEbx.Find(aebx => aebx.path == resPath);
        }

        private bool IsDataInfoValid(DataInfo di)
        {
            if (morphAssetTypes.Contains((uint)di.type) || di.path.ToLower().Contains("morph"))
            {
                return true;
            }
            else return false;
        }

        private DataInfo SearchItemRES(string s)
        {
            foreach (KeyValuePair<string, List<string>> pair in bundlePaths)
            {
                foreach (string bundle in pair.Value)
                {
                    foreach (DataInfo res in main.Host.getAllRES(pair.Key, bundle))
                        if (res.path.ToLower().EndsWith(s))
                        {
                            return res;
                        }
                }
            }
            return null;
        }

        private ChunkInfo SearchChunk(string chunkId)
        {
            var allChunks = new List<ChunkInfo>();
            var allTocs = main.Host.getTOCFiles();
            allTocs.ForEach(toc => allChunks.AddRange(main.Host.getAllTocCHUNKs(toc)));
            return allChunks.Find(c => c.id == chunkId);
        }

        //
        // refresh and display trees functions
        //

        // refresh toc tree
        public void RefreshTocTree()
        {
            tv1.Nodes.Clear();
            TreeNode t = new TreeNode("ROOT");
            foreach (string toc in tocFiles)
                foreach (string bundle in bundlePaths[toc])
                    Helpers.AddPath(t, bundle);
            t.Expand();
            if (t.Nodes.Count != 0)
                t.Nodes[0].Expand();
            tv1.Nodes.Add(t);
        }

        // refresh bundle tree
        public void RefreshNodes()
        {
            tv2.Nodes.Clear();
            TreeNode t = new TreeNode("ROOT");
            foreach (DataInfo info in res)
                if (IsDataInfoValid(info))
                    Helpers.AddPath(t, info.path);
            t.ExpandAll();
            tv2.Nodes.Add(t);
        }

        //
        // on click in trees events
        //

        // handles click in toc tree
        private void tv1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ActivateExportControls(false);
            TreeNode sel = tv1.SelectedNode;
            if (sel == null)
                return;
            currBundle = Helpers.GetPathFromNode(sel);
            if (currBundle.Length > 5)
                currBundle = currBundle.Substring(5);
            chunks = new List<ChunkInfo>();
            chunks.AddRange(globalChunks);
            foreach (KeyValuePair<string, List<string>> pair in bundlePaths)
                if (pair.Value.Contains(currBundle))
                {                 
                    if (!resByBundles.ContainsKey(currBundle))
                    {
                        LoadBundle(pair.Key, currBundle);
                    }
                    currToc = pair.Key;
                    res = resByBundles[currBundle];
                    chunks.AddRange(main.Host.getAllBundleCHUNKs(pair.Key, currBundle));
                    RefreshNodes();
                    return;
                }
        }

        // handles click in bundle tree
        private void tv2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ActivateExportControls(false);
            TreeNode sel = tv2.SelectedNode;
            if (sel == null)
                return;
            string path = Helpers.GetPathFromNode(sel);
            if(path.Length > 5)
                path = path.Substring(5);

            // display temporary node
            tv3.Nodes.Clear();
            TreeNode tempNode = new TreeNode();
            tempNode.Text = "Loading...";
            tv3.Nodes.Add(tempNode);
            Application.DoEvents();

            // find ebx and res with this path
            var selectedRes = res.Find(resInfo => resInfo.path == path);
            var selEbx = LoadMorphEbx(path);
            if (selectedRes != null && selEbx != null)
            {
                ActivateExportControls(true);
                DisplayMorphDetails(selectedRes, selEbx);
            }
            else
            {
                tv3.Nodes.Clear();
                Application.DoEvents();
            }
        }

        private void DisplayMorphDetails(DataInfo res, DataInfo ebx)
        {
            currPath = res.path;
            rawEbxBuffer = main.Host.getDataBySha1(ebx.sha1);
            rawResBuffer = main.Host.getDataBySha1(res.sha1);
            if (rawResBuffer == null || rawEbxBuffer == null)
            {
                TreeNode noDataNode = new TreeNode();
                noDataNode.Text = "Unable to find raw data buffers for this morph";
                tv3.Nodes.Clear();
                tv3.Nodes.Add(noDataNode);
                return;
            }
            try
            {
                hb1.ByteProvider = new DynamicByteProvider(rawEbxBuffer);
                hb2.ByteProvider = new DynamicByteProvider(rawResBuffer);
                var morph = new MorphStaticExtended(new MemoryStream(rawResBuffer), new MemoryStream(rawEbxBuffer));
                MorphDetailsTB.Text = morph.ToJson();
                tv3.Nodes.Clear();
                tv3.Nodes.Add(JsonToTree.ToNode(morph.ToJson(), "MorphStatic"));
            }
            catch (Exception ex)
            {
                TreeNode ErrorNode = new TreeNode();
                ErrorNode.Text = "ERROR!!!:\n" + ex.Message + "\n\n";
                tv3.Nodes.Clear();
                tv3.Nodes.Add(ErrorNode);
            }
        }

        //
        // buttons click events
        //
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Helpers.SelectNext(toolStripTextBox1.Text, tv1);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Helpers.SelectNext(toolStripTextBox2.Text, tv2);
        }

        // export ebx raw buffer of selected morph in tree
        private void exportRawBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rawEbxBuffer != null && rawEbxBuffer.Length != 0)
            {
                SaveFileDialog d = new SaveFileDialog();
                d.Filter = "*.bin|*.bin";
                d.FileName = Path.GetFileName(currPath) + ".ebx.raw.bin";
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    File.WriteAllBytes(d.FileName, rawEbxBuffer);
                    MessageBox.Show("Done.");
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (tv1.SelectedNode == null)
                tv1.Nodes[0].ExpandAll();
            else
                tv1.SelectedNode.ExpandAll();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (tv2.SelectedNode == null)
                tv2.Nodes[0].ExpandAll();
            else
                tv2.SelectedNode.ExpandAll();
        }

        private void CollapseAll(TreeNode t)
        {
            foreach (TreeNode t2 in t.Nodes)
                CollapseAll(t2);
            t.Collapse();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (tv1.SelectedNode == null)
                CollapseAll(tv1.Nodes[0]);
            else
                CollapseAll(tv1.SelectedNode);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (tv2.SelectedNode == null)
                CollapseAll(tv2.Nodes[0]);
            else
                CollapseAll(tv2.SelectedNode);
        }

        private void copyNodePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv1.SelectedNode != null)
                Clipboard.SetText(Helpers.GetPathFromNode(tv1.SelectedNode));
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (tv2.SelectedNode != null)
                Clipboard.SetText(Helpers.GetPathFromNode(tv2.SelectedNode));
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            // empty all fields and disables all controls
            ActivateControls(false);
            tv1.Nodes.Clear();
            tv2.Nodes.Clear();
            tv3.Nodes.Clear();
            MorphDetailsTB.Text = "";
            hb1.ByteProvider = new DynamicByteProvider(new byte[0]);
            hb2.ByteProvider = new DynamicByteProvider(new byte[0]);
            TreeNode tempNode = new TreeNode("Loading, please wait...");
            tv1.Nodes.Add(tempNode);

            // delete cache
            PluginMorphCache.CleanCache();

            // clean objects holding data
            tocFiles.Clear();
            bundlePaths.Clear();
            tocChunks.Clear();
            currBundle = "";
            res.Clear();
            chunks.Clear();
            globalChunks.Clear();
            resByBundles.Clear();

            // scan for content again
            ScanForContent();
        }

        // export selected morph :
        // search for base mesh and applies selected morph deformation to it and to selected skeleton and exports the result.
        private void exportMorphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rawEbxBuffer != null && rawResBuffer != null)
            {
                var morph = new MorphStaticExtended(new MemoryStream(rawResBuffer), new MemoryStream(rawEbxBuffer));

                // look for preset Mesh
                DataInfo presetMeshRes = SearchItemRES(morph.PresetMesh);
                if (presetMeshRes != null)
                {
                    byte[] meshData = main.Host.getDataBySha1(presetMeshRes.sha1);
                    MeshAsset presetMeshAsset = new MeshAsset(new MemoryStream(meshData));

                    ExportMorphSaveDialog emd = new ExportMorphSaveDialog(morph.LodCount, 100f);
                    if (emd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SkeletonAsset skeleton = null;
                        int lod = emd.Lod;
                        string sha1 = emd.Skeleton;
                        if (sha1 != null)
                        {
                            var sebx = new EBX(new MemoryStream(main.Host.getDataBySha1(Helpers.HexStringToByteArray(sha1))));
                            skeleton = new SkeletonAsset(sebx);
                        }

                        float oScale = emd.ExportScale;
                        bool bakeMorphToMesh = emd.BakeMorph;

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Title = "Save Morph as...";
                        if (skeleton == null) 
                        {
                            sfd.Filter = "*.obj|*.obj";
                            sfd.FileName = Path.GetFileName(currPath) + ".obj";
                        }
                        else
                        {
                            sfd.Filter = "*.fbx|*.fbx"; /*|*.psk|*.psk";*/ // morph export not working in psk
                            sfd.FileName = Path.GetFileName(currPath) + ".fbx";
                        }

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            string ext = Path.GetExtension(sfd.FileName);
                            ChunkInfo lodChunk = SearchChunk(Helpers.ByteArrayToHexString(presetMeshAsset.lods[lod].chunkID));
                            if (lodChunk != null)
                            {
                                byte[] rawChunkBuffer = main.Host.getDataBySha1(lodChunk.sha1);
                                presetMeshAsset.lods[lod].LoadVertexData(new MemoryStream(rawChunkBuffer));

                                IMeshExporter exporter = MeshExporter.GetExporterByExtension(ext, skeleton);
                                //exporter.ExportLodWithMorph(presetMeshAsset, lod, morph, sfd.FileName, oScale, bakeMorphToMesh);
                                exporter.ExportLod(presetMeshAsset, lod, sfd.FileName);
                                MessageBox.Show("Done.");
                            }
                            else MessageBox.Show("Error : chunk for this lod was not found");
                        }                   
                    }
                }
                else
                {
                    MessageBox.Show("Error : Res data corresponding to preset mesh " + morph.PresetMesh + " not found.");
                }
            }
        }

        // exports res raw buffer of selected morph
        private void exportResRawBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rawResBuffer != null && rawResBuffer.Length != 0)
            {
                SaveFileDialog d = new SaveFileDialog();
                d.Filter = "*.bin|*.bin";
                d.FileName = Path.GetFileName(currPath) + ".res.raw.bin";
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    File.WriteAllBytes(d.FileName, rawResBuffer);
                    MessageBox.Show("Done.");
                }
            }
        }

        private void copyNodeContentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv3.SelectedNode != null)
                Clipboard.SetText(tv3.SelectedNode.Text);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // end button click functions
    }
}
