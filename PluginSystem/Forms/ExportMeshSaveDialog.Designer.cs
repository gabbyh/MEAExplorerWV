namespace PluginSystem
{
    partial class ExportMeshSaveDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxSkeleton = new System.Windows.Forms.GroupBox();
            this.skeletonComboBox = new System.Windows.Forms.ComboBox();
            this.groupBoxLod = new System.Windows.Forms.GroupBox();
            this.allLodCheckBox = new System.Windows.Forms.CheckBox();
            this.LodSelectorNumeric = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ExportButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxScale = new System.Windows.Forms.GroupBox();
            this.ScaleNumeric = new System.Windows.Forms.NumericUpDown();
            this.groupBoxBlendShape = new System.Windows.Forms.GroupBox();
            this.BakeMorphCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBoxFormat = new System.Windows.Forms.GroupBox();
            this.formatComboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxSkeleton.SuspendLayout();
            this.groupBoxLod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LodSelectorNumeric)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBoxScale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleNumeric)).BeginInit();
            this.groupBoxBlendShape.SuspendLayout();
            this.groupBoxFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBoxSkeleton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxLod, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxFormat, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(10, 15, 10, 15);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(298, 243);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBoxSkeleton
            // 
            this.groupBoxSkeleton.Controls.Add(this.skeletonComboBox);
            this.groupBoxSkeleton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSkeleton.Location = new System.Drawing.Point(2, 50);
            this.groupBoxSkeleton.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxSkeleton.Name = "groupBoxSkeleton";
            this.groupBoxSkeleton.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxSkeleton.Size = new System.Drawing.Size(294, 44);
            this.groupBoxSkeleton.TabIndex = 5;
            this.groupBoxSkeleton.TabStop = false;
            this.groupBoxSkeleton.Text = "Skeleton";
            // 
            // skeletonComboBox
            // 
            this.skeletonComboBox.FormattingEnabled = true;
            this.skeletonComboBox.Location = new System.Drawing.Point(50, 17);
            this.skeletonComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.skeletonComboBox.Name = "skeletonComboBox";
            this.skeletonComboBox.Size = new System.Drawing.Size(168, 21);
            this.skeletonComboBox.TabIndex = 0;
            // 
            // groupBoxLod
            // 
            this.groupBoxLod.Controls.Add(this.allLodCheckBox);
            this.groupBoxLod.Controls.Add(this.LodSelectorNumeric);
            this.groupBoxLod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxLod.Location = new System.Drawing.Point(2, 98);
            this.groupBoxLod.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxLod.Name = "groupBoxLod";
            this.groupBoxLod.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxLod.Size = new System.Drawing.Size(294, 44);
            this.groupBoxLod.TabIndex = 6;
            this.groupBoxLod.TabStop = false;
            this.groupBoxLod.Text = "Lod";
            // 
            // allLodCheckBox
            // 
            this.allLodCheckBox.AutoSize = true;
            this.allLodCheckBox.Location = new System.Drawing.Point(188, 11);
            this.allLodCheckBox.Name = "allLodCheckBox";
            this.allLodCheckBox.Size = new System.Drawing.Size(37, 17);
            this.allLodCheckBox.TabIndex = 1;
            this.allLodCheckBox.Text = "All";
            this.allLodCheckBox.UseVisualStyleBackColor = true;
            this.allLodCheckBox.CheckedChanged += new System.EventHandler(this.allLodCheckBox_CheckedChanged);
            // 
            // LodSelectorNumeric
            // 
            this.LodSelectorNumeric.Location = new System.Drawing.Point(50, 10);
            this.LodSelectorNumeric.Margin = new System.Windows.Forms.Padding(2, 10, 2, 2);
            this.LodSelectorNumeric.Name = "LodSelectorNumeric";
            this.LodSelectorNumeric.Size = new System.Drawing.Size(80, 20);
            this.LodSelectorNumeric.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.ExportButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.CancelButton, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 199);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(294, 42);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // ExportButton
            // 
            this.ExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportButton.Location = new System.Drawing.Point(50, 9);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(50, 3, 10, 3);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(87, 23);
            this.ExportButton.TabIndex = 0;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(157, 9);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(10, 3, 50, 3);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(87, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel3.Controls.Add(this.groupBoxScale, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBoxBlendShape, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 147);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(292, 47);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // groupBoxScale
            // 
            this.groupBoxScale.Controls.Add(this.ScaleNumeric);
            this.groupBoxScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxScale.Location = new System.Drawing.Point(3, 3);
            this.groupBoxScale.Name = "groupBoxScale";
            this.groupBoxScale.Size = new System.Drawing.Size(125, 41);
            this.groupBoxScale.TabIndex = 0;
            this.groupBoxScale.TabStop = false;
            this.groupBoxScale.Text = "Scale";
            // 
            // ScaleNumeric
            // 
            this.ScaleNumeric.DecimalPlaces = 1;
            this.ScaleNumeric.Location = new System.Drawing.Point(46, 14);
            this.ScaleNumeric.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ScaleNumeric.Name = "ScaleNumeric";
            this.ScaleNumeric.Size = new System.Drawing.Size(59, 20);
            this.ScaleNumeric.TabIndex = 0;
            this.ScaleNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // groupBoxBlendShape
            // 
            this.groupBoxBlendShape.Controls.Add(this.BakeMorphCheckBox);
            this.groupBoxBlendShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxBlendShape.Location = new System.Drawing.Point(134, 3);
            this.groupBoxBlendShape.Name = "groupBoxBlendShape";
            this.groupBoxBlendShape.Size = new System.Drawing.Size(155, 41);
            this.groupBoxBlendShape.TabIndex = 1;
            this.groupBoxBlendShape.TabStop = false;
            this.groupBoxBlendShape.Text = "Bake Blend Shape (fbx only)";
            // 
            // BakeMorphCheckBox
            // 
            this.BakeMorphCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.BakeMorphCheckBox.AutoSize = true;
            this.BakeMorphCheckBox.Location = new System.Drawing.Point(53, 21);
            this.BakeMorphCheckBox.Name = "BakeMorphCheckBox";
            this.BakeMorphCheckBox.Size = new System.Drawing.Size(15, 14);
            this.BakeMorphCheckBox.TabIndex = 0;
            this.BakeMorphCheckBox.UseVisualStyleBackColor = true;
            this.BakeMorphCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBoxFormat
            // 
            this.groupBoxFormat.Controls.Add(this.formatComboBox);
            this.groupBoxFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFormat.Location = new System.Drawing.Point(3, 3);
            this.groupBoxFormat.Name = "groupBoxFormat";
            this.groupBoxFormat.Size = new System.Drawing.Size(292, 42);
            this.groupBoxFormat.TabIndex = 9;
            this.groupBoxFormat.TabStop = false;
            this.groupBoxFormat.Text = "Export Format";
            // 
            // formatComboBox
            // 
            this.formatComboBox.FormattingEnabled = true;
            this.formatComboBox.Location = new System.Drawing.Point(49, 17);
            this.formatComboBox.Name = "formatComboBox";
            this.formatComboBox.Size = new System.Drawing.Size(168, 21);
            this.formatComboBox.TabIndex = 0;
            // 
            // ExportMeshSaveDialog
            // 
            this.AcceptButton = this.ExportButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 243);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ExportMeshSaveDialog";
            this.Text = "Export Options";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBoxSkeleton.ResumeLayout(false);
            this.groupBoxLod.ResumeLayout(false);
            this.groupBoxLod.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LodSelectorNumeric)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBoxScale.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScaleNumeric)).EndInit();
            this.groupBoxBlendShape.ResumeLayout(false);
            this.groupBoxBlendShape.PerformLayout();
            this.groupBoxFormat.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.GroupBox groupBoxSkeleton;
        private System.Windows.Forms.ComboBox skeletonComboBox;
        private System.Windows.Forms.GroupBox groupBoxLod;
        private System.Windows.Forms.NumericUpDown LodSelectorNumeric;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBoxScale;
        private System.Windows.Forms.NumericUpDown ScaleNumeric;
        private System.Windows.Forms.GroupBox groupBoxBlendShape;
        private System.Windows.Forms.CheckBox BakeMorphCheckBox;
        private System.Windows.Forms.CheckBox allLodCheckBox;
        private System.Windows.Forms.GroupBox groupBoxFormat;
        private System.Windows.Forms.ComboBox formatComboBox;
    }
}