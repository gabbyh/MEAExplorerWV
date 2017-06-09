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

namespace PluginMorph
{
    public partial class ExportMorphSaveDialog : Form
    {
        public int Lod;
        public string Skeleton;
        public float ExportScale;
        public bool BakeMorph;

        Dictionary<string, string> skeletons;

        public ExportMorphSaveDialog(int lodCount, float defaultScale)
        {
            InitializeComponent();
            LodSelectorNumeric.Maximum = lodCount - 1;
            ScaleNumeric.Value = (decimal) defaultScale;
            LoadSkeletonList();
        }

        private void LoadSkeletonList()
        {
            skeletonComboBox.Items.Add("None");
            skeletonComboBox.SelectedIndex = 0;
            StringReader sr = new StringReader(Properties.Resources.skeletonSHA1s);
            string line;
            skeletons = new Dictionary<string, string>();
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(':');
                skeletons.Add(parts[0].Trim(), parts[1].Trim());
            }
            skeletonComboBox.Items.AddRange(skeletons.Keys.ToArray());
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Lod = (int)LodSelectorNumeric.Value;
            ExportScale = (float)ScaleNumeric.Value;
            BakeMorph = BakeMorphCheckBox.Checked;
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
    }
}
