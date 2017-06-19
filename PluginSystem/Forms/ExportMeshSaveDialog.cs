using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace PluginSystem
{
    public partial class ExportMeshSaveDialog : Form
    {
        public int Lod;
        public bool AllLod;
        public string Skeleton;
        public float ExportScale;
        public bool BakeMorph;
        public string Format;

        Dictionary<string, string> skeletons;

        public ExportMeshSaveDialog(int lodCount, float defaultScale, bool morphMode = false, bool skinnedMesh = true, string selectedskeleton=null, int selectedLod=0)
        {
            InitializeComponent();
            groupBoxBlendShape.Visible = morphMode;
            if (!groupBoxBlendShape.Visible)
            {
                tableLayoutPanel3.ColumnStyles.Clear();
                tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            }
            
            LodSelectorNumeric.Maximum = lodCount - 1;
            LodSelectorNumeric.Value = selectedLod;
            ScaleNumeric.Value = (decimal) defaultScale;
            LoadSkeletonList(selectedskeleton, skinnedMesh);
            LoadFormatList(morphMode);
        }

        private void LoadSkeletonList(string selected, bool isMeshSkinned)
        {
            if (isMeshSkinned)
            {
                skeletonComboBox.Items.Add("None");
                skeletonComboBox.SelectedIndex = 0;
                StringReader sr = new StringReader(Properties.Res.skeletonSHA1s);
                string line;
                skeletons = new Dictionary<string, string>();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(':');
                    skeletons.Add(parts[0].Trim(), parts[1].Trim());
                }
                skeletonComboBox.Items.AddRange(skeletons.Keys.ToArray());

                if (selected != null)
                {
                    skeletonComboBox.SelectedItem = selected;
                }
            }
            else
            {
                skeletonComboBox.Items.Add("None");
                skeletonComboBox.SelectedIndex = 0;
                skeletonComboBox.Enabled = false;
            }           
        }

        private void LoadFormatList(bool morphMode)
        {
            formatComboBox.Items.Clear();
            formatComboBox.Items.Add(".fbx");
            formatComboBox.Items.Add(".obj");
            if (!morphMode)
                formatComboBox.Items.Add(".psk");

            formatComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Lod = (int)LodSelectorNumeric.Value;
            AllLod = allLodCheckBox.Checked;
            ExportScale = (float)ScaleNumeric.Value;
            BakeMorph = BakeMorphCheckBox.Checked;
            Format = (string) formatComboBox.SelectedItem;
            string selectedSkeletonValue = (string)skeletonComboBox.SelectedItem;
            Skeleton = selectedSkeletonValue == "None" ? null : skeletons[selectedSkeletonValue];
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void allLodCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LodSelectorNumeric.Enabled = !allLodCheckBox.Checked;
        }
    }
}
