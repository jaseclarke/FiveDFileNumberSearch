namespace FiveDFileNumberSearch
{
    partial class Form1
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.rootFolderText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chooseInputFile = new System.Windows.Forms.Button();
            this.showFileNumbersBtn = new System.Windows.Forms.Button();
            this.searchFNBtn = new System.Windows.Forms.Button();
            this.fileNumberTB = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DatabaseStatusText = new System.Windows.Forms.Label();
            this.ProcessChangesBtn = new System.Windows.Forms.Button();
            this.setRootFolderBtn = new System.Windows.Forms.Button();
            this.uploadToGoogleDriveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 138);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(767, 400);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // rootFolderText
            // 
            this.rootFolderText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rootFolderText.Location = new System.Drawing.Point(94, 61);
            this.rootFolderText.Name = "rootFolderText";
            this.rootFolderText.Size = new System.Drawing.Size(515, 20);
            this.rootFolderText.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Search Folder:";
            // 
            // chooseInputFile
            // 
            this.chooseInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chooseInputFile.Location = new System.Drawing.Point(615, 59);
            this.chooseInputFile.Name = "chooseInputFile";
            this.chooseInputFile.Size = new System.Drawing.Size(31, 23);
            this.chooseInputFile.TabIndex = 3;
            this.chooseInputFile.Text = "...";
            this.chooseInputFile.UseVisualStyleBackColor = true;
            this.chooseInputFile.Click += new System.EventHandler(this.chooseInputFile_Click);
            // 
            // showFileNumbersBtn
            // 
            this.showFileNumbersBtn.Location = new System.Drawing.Point(190, 109);
            this.showFileNumbersBtn.Name = "showFileNumbersBtn";
            this.showFileNumbersBtn.Size = new System.Drawing.Size(116, 23);
            this.showFileNumbersBtn.TabIndex = 9;
            this.showFileNumbersBtn.Text = "All File Numbers";
            this.showFileNumbersBtn.UseVisualStyleBackColor = true;
            this.showFileNumbersBtn.Click += new System.EventHandler(this.showFileNumbersBtn_Click);
            // 
            // searchFNBtn
            // 
            this.searchFNBtn.Location = new System.Drawing.Point(312, 109);
            this.searchFNBtn.Name = "searchFNBtn";
            this.searchFNBtn.Size = new System.Drawing.Size(120, 23);
            this.searchFNBtn.TabIndex = 10;
            this.searchFNBtn.Text = "Search File Number:";
            this.searchFNBtn.UseVisualStyleBackColor = true;
            this.searchFNBtn.Click += new System.EventHandler(this.searchFNBtn_Click);
            // 
            // fileNumberTB
            // 
            this.fileNumberTB.Location = new System.Drawing.Point(447, 109);
            this.fileNumberTB.Name = "fileNumberTB";
            this.fileNumberTB.Size = new System.Drawing.Size(217, 20);
            this.fileNumberTB.TabIndex = 11;
            this.fileNumberTB.TextChanged += new System.EventHandler(this.fileNumberTB_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(791, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDatabaseToolStripMenuItem,
            this.openDatabaseToolStripMenuItem,
            this.toolStripSeparator2,
            this.exportDatabaseToolStripMenuItem,
            this.toolStripMenuItem1,
            this.uploadToGoogleDriveToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newDatabaseToolStripMenuItem
            // 
            this.newDatabaseToolStripMenuItem.Name = "newDatabaseToolStripMenuItem";
            this.newDatabaseToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.newDatabaseToolStripMenuItem.Text = "New Database...";
            this.newDatabaseToolStripMenuItem.Click += new System.EventHandler(this.newDatabaseToolStripMenuItem_Click);
            // 
            // openDatabaseToolStripMenuItem
            // 
            this.openDatabaseToolStripMenuItem.Name = "openDatabaseToolStripMenuItem";
            this.openDatabaseToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.openDatabaseToolStripMenuItem.Text = "Open Database...";
            this.openDatabaseToolStripMenuItem.Click += new System.EventHandler(this.openDatabaseToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(203, 6);
            // 
            // exportDatabaseToolStripMenuItem
            // 
            this.exportDatabaseToolStripMenuItem.Name = "exportDatabaseToolStripMenuItem";
            this.exportDatabaseToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.exportDatabaseToolStripMenuItem.Text = "Export Database...";
            this.exportDatabaseToolStripMenuItem.Click += new System.EventHandler(this.exportDatabaseToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(203, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // DatabaseStatusText
            // 
            this.DatabaseStatusText.AutoSize = true;
            this.DatabaseStatusText.Location = new System.Drawing.Point(12, 34);
            this.DatabaseStatusText.Name = "DatabaseStatusText";
            this.DatabaseStatusText.Size = new System.Drawing.Size(127, 13);
            this.DatabaseStatusText.TabIndex = 13;
            this.DatabaseStatusText.Text = "Database Loaded: None ";
            // 
            // ProcessChangesBtn
            // 
            this.ProcessChangesBtn.Location = new System.Drawing.Point(12, 109);
            this.ProcessChangesBtn.Name = "ProcessChangesBtn";
            this.ProcessChangesBtn.Size = new System.Drawing.Size(174, 23);
            this.ProcessChangesBtn.TabIndex = 14;
            this.ProcessChangesBtn.Text = "Process Changed Files";
            this.ProcessChangesBtn.UseVisualStyleBackColor = true;
            this.ProcessChangesBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // setRootFolderBtn
            // 
            this.setRootFolderBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setRootFolderBtn.Location = new System.Drawing.Point(652, 59);
            this.setRootFolderBtn.Name = "setRootFolderBtn";
            this.setRootFolderBtn.Size = new System.Drawing.Size(105, 23);
            this.setRootFolderBtn.TabIndex = 15;
            this.setRootFolderBtn.Text = "Set Search Folder";
            this.setRootFolderBtn.UseVisualStyleBackColor = true;
            this.setRootFolderBtn.Click += new System.EventHandler(this.SetRootFolderBtnClick);
            // 
            // uploadToGoogleDriveToolStripMenuItem
            // 
            this.uploadToGoogleDriveToolStripMenuItem.Name = "uploadToGoogleDriveToolStripMenuItem";
            this.uploadToGoogleDriveToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.uploadToGoogleDriveToolStripMenuItem.Text = "Upload to Google Drive...";
            this.uploadToGoogleDriveToolStripMenuItem.Click += new System.EventHandler(this.uploadToGoogleDriveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(203, 6);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 550);
            this.Controls.Add(this.setRootFolderBtn);
            this.Controls.Add(this.ProcessChangesBtn);
            this.Controls.Add(this.DatabaseStatusText);
            this.Controls.Add(this.fileNumberTB);
            this.Controls.Add(this.searchFNBtn);
            this.Controls.Add(this.showFileNumbersBtn);
            this.Controls.Add(this.chooseInputFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rootFolderText);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "5D File Number Search";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox rootFolderText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button chooseInputFile;
        private System.Windows.Forms.Button showFileNumbersBtn;
        private System.Windows.Forms.Button searchFNBtn;
        private System.Windows.Forms.TextBox fileNumberTB;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDatabaseToolStripMenuItem;
        private System.Windows.Forms.Label DatabaseStatusText;
        private System.Windows.Forms.Button ProcessChangesBtn;
        private System.Windows.Forms.Button setRootFolderBtn;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exportDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem uploadToGoogleDriveToolStripMenuItem;
    }
}

