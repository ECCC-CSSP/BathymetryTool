namespace BathymetryToolApp;

public partial class BathymetryToolAppForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        splitContainer1 = new SplitContainer();
        butOnlyUseXYZWithinPolygon = new Button();
        lblStatus = new Label();
        lblStatusText = new Label();
        textBoxStartDir = new TextBox();
        lblStartDirText = new Label();
        butSubdivide = new Button();
        richTextBoxStatus = new RichTextBox();
        lblGooglePolygonText = new Label();
        richTextBox1 = new RichTextBox();
        lblCopyInDirectoryText = new Label();
        textBoxCopyInDirectory = new TextBox();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        SuspendLayout();
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        splitContainer1.Orientation = Orientation.Horizontal;
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(textBoxCopyInDirectory);
        splitContainer1.Panel1.Controls.Add(lblCopyInDirectoryText);
        splitContainer1.Panel1.Controls.Add(richTextBox1);
        splitContainer1.Panel1.Controls.Add(lblGooglePolygonText);
        splitContainer1.Panel1.Controls.Add(butOnlyUseXYZWithinPolygon);
        splitContainer1.Panel1.Controls.Add(lblStatus);
        splitContainer1.Panel1.Controls.Add(lblStatusText);
        splitContainer1.Panel1.Controls.Add(textBoxStartDir);
        splitContainer1.Panel1.Controls.Add(lblStartDirText);
        splitContainer1.Panel1.Controls.Add(butSubdivide);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(richTextBoxStatus);
        splitContainer1.Size = new Size(1182, 671);
        splitContainer1.SplitterDistance = 208;
        splitContainer1.TabIndex = 0;
        // 
        // butOnlyUseXYZWithinPolygon
        // 
        butOnlyUseXYZWithinPolygon.Location = new Point(12, 72);
        butOnlyUseXYZWithinPolygon.Name = "butOnlyUseXYZWithinPolygon";
        butOnlyUseXYZWithinPolygon.Size = new Size(100, 47);
        butOnlyUseXYZWithinPolygon.TabIndex = 5;
        butOnlyUseXYZWithinPolygon.Text = "Only Use xyz within polygon";
        butOnlyUseXYZWithinPolygon.UseVisualStyleBackColor = true;
        butOnlyUseXYZWithinPolygon.Click += butOnlyUseXYZWithinPolygon_Click;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(73, 183);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(37, 15);
        lblStatus.TabIndex = 4;
        lblStatus.Text = "------";
        // 
        // lblStatusText
        // 
        lblStatusText.AutoSize = true;
        lblStatusText.Location = new Point(12, 183);
        lblStatusText.Name = "lblStatusText";
        lblStatusText.Size = new Size(42, 15);
        lblStatusText.TabIndex = 3;
        lblStatusText.Text = "Status:";
        // 
        // textBoxStartDir
        // 
        textBoxStartDir.Location = new Point(185, 20);
        textBoxStartDir.Name = "textBoxStartDir";
        textBoxStartDir.Size = new Size(375, 23);
        textBoxStartDir.TabIndex = 2;
        textBoxStartDir.Text = "C:\\CSSP Latest Code Old\\BathymetryTool\\Data\\";
        // 
        // lblStartDirText
        // 
        lblStartDirText.AutoSize = true;
        lblStartDirText.Location = new Point(118, 24);
        lblStartDirText.Name = "lblStartDirText";
        lblStartDirText.Size = new Size(52, 15);
        lblStartDirText.TabIndex = 1;
        lblStartDirText.Text = "Start Dir:";
        // 
        // butSubdivide
        // 
        butSubdivide.Enabled = false;
        butSubdivide.Location = new Point(12, 20);
        butSubdivide.Name = "butSubdivide";
        butSubdivide.Size = new Size(75, 23);
        butSubdivide.TabIndex = 0;
        butSubdivide.Text = "Subdivide";
        butSubdivide.UseVisualStyleBackColor = true;
        butSubdivide.Click += butSubdivide_Click;
        // 
        // richTextBoxStatus
        // 
        richTextBoxStatus.Dock = DockStyle.Fill;
        richTextBoxStatus.Location = new Point(0, 0);
        richTextBoxStatus.Name = "richTextBoxStatus";
        richTextBoxStatus.Size = new Size(1182, 459);
        richTextBoxStatus.TabIndex = 0;
        richTextBoxStatus.Text = "";
        // 
        // lblGooglePolygonText
        // 
        lblGooglePolygonText.AutoSize = true;
        lblGooglePolygonText.Location = new Point(118, 76);
        lblGooglePolygonText.Name = "lblGooglePolygonText";
        lblGooglePolygonText.Size = new Size(95, 15);
        lblGooglePolygonText.TabIndex = 6;
        lblGooglePolygonText.Text = "Google Polygon:";
        // 
        // richTextBox1
        // 
        richTextBox1.Location = new Point(213, 72);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(316, 58);
        richTextBox1.TabIndex = 7;
        richTextBox1.Text = "";
        // 
        // lblCopyInDirectoryText
        // 
        lblCopyInDirectoryText.AutoSize = true;
        lblCopyInDirectoryText.Location = new Point(118, 148);
        lblCopyInDirectoryText.Name = "lblCopyInDirectoryText";
        lblCopyInDirectoryText.Size = new Size(102, 15);
        lblCopyInDirectoryText.TabIndex = 8;
        lblCopyInDirectoryText.Text = "Copy in Directory:";
        // 
        // textBoxCopyInDirectory
        // 
        textBoxCopyInDirectory.Location = new Point(226, 145);
        textBoxCopyInDirectory.Name = "textBoxCopyInDirectory";
        textBoxCopyInDirectory.Size = new Size(375, 23);
        textBoxCopyInDirectory.TabIndex = 9;
        textBoxCopyInDirectory.Text = "C:\\CSSP Latest Code Old\\BathymetryTool\\Data\\Bouctouche\\";
        // 
        // BathymetryToolAppForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1182, 671);
        Controls.Add(splitContainer1);
        Name = "BathymetryToolAppForm";
        Text = "Bathymetry Tool App";
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel1.PerformLayout();
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    private Button butSubdivide;
    private TextBox textBoxStartDir;
    private Label lblStartDirText;
    private RichTextBox richTextBoxStatus;
    private Label lblStatusText;
    private Label lblStatus;
    private Button butOnlyUseXYZWithinPolygon;
    private RichTextBox richTextBox1;
    private Label lblGooglePolygonText;
    private Label lblCopyInDirectoryText;
    private TextBox textBoxCopyInDirectory;
}