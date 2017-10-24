﻿namespace FiveDFileNumberSearch
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
            this.inputFileNameText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chooseInputFile = new System.Windows.Forms.Button();
            this.DoUpdate = new System.Windows.Forms.Button();
            this.showAllBtn = new System.Windows.Forms.Button();
            this.showFileNumbersBtn = new System.Windows.Forms.Button();
            this.searchFNBtn = new System.Windows.Forms.Button();
            this.fileNumberTB = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 99);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(623, 334);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // inputFileNameText
            // 
            this.inputFileNameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputFileNameText.Location = new System.Drawing.Point(80, 15);
            this.inputFileNameText.Name = "inputFileNameText";
            this.inputFileNameText.Size = new System.Drawing.Size(518, 20);
            this.inputFileNameText.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input File:";
            // 
            // chooseInputFile
            // 
            this.chooseInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chooseInputFile.Location = new System.Drawing.Point(604, 14);
            this.chooseInputFile.Name = "chooseInputFile";
            this.chooseInputFile.Size = new System.Drawing.Size(31, 23);
            this.chooseInputFile.TabIndex = 3;
            this.chooseInputFile.Text = "...";
            this.chooseInputFile.UseVisualStyleBackColor = true;
            this.chooseInputFile.Click += new System.EventHandler(this.chooseInputFile_Click);
            // 
            // DoUpdate
            // 
            this.DoUpdate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DoUpdate.Location = new System.Drawing.Point(270, 41);
            this.DoUpdate.Name = "DoUpdate";
            this.DoUpdate.Size = new System.Drawing.Size(100, 23);
            this.DoUpdate.TabIndex = 7;
            this.DoUpdate.Text = "Load Model";
            this.DoUpdate.UseVisualStyleBackColor = true;
            this.DoUpdate.Click += new System.EventHandler(this.DoUpdate_Click);
            // 
            // showAllBtn
            // 
            this.showAllBtn.Location = new System.Drawing.Point(16, 70);
            this.showAllBtn.Name = "showAllBtn";
            this.showAllBtn.Size = new System.Drawing.Size(75, 23);
            this.showAllBtn.TabIndex = 8;
            this.showAllBtn.Text = "Show All";
            this.showAllBtn.UseVisualStyleBackColor = true;
            this.showAllBtn.Click += new System.EventHandler(this.showAllBtn_Click);
            // 
            // showFileNumbersBtn
            // 
            this.showFileNumbersBtn.Location = new System.Drawing.Point(118, 70);
            this.showFileNumbersBtn.Name = "showFileNumbersBtn";
            this.showFileNumbersBtn.Size = new System.Drawing.Size(116, 23);
            this.showFileNumbersBtn.TabIndex = 9;
            this.showFileNumbersBtn.Text = "All File Numbers";
            this.showFileNumbersBtn.UseVisualStyleBackColor = true;
            this.showFileNumbersBtn.Click += new System.EventHandler(this.showFileNumbersBtn_Click);
            // 
            // searchFNBtn
            // 
            this.searchFNBtn.Location = new System.Drawing.Point(250, 70);
            this.searchFNBtn.Name = "searchFNBtn";
            this.searchFNBtn.Size = new System.Drawing.Size(120, 23);
            this.searchFNBtn.TabIndex = 10;
            this.searchFNBtn.Text = "Search File Number:";
            this.searchFNBtn.UseVisualStyleBackColor = true;
            this.searchFNBtn.Click += new System.EventHandler(this.searchFNBtn_Click);
            // 
            // fileNumberTB
            // 
            this.fileNumberTB.Location = new System.Drawing.Point(381, 72);
            this.fileNumberTB.Name = "fileNumberTB";
            this.fileNumberTB.Size = new System.Drawing.Size(217, 20);
            this.fileNumberTB.TabIndex = 11;
            this.fileNumberTB.TextChanged += new System.EventHandler(this.fileNumberTB_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(448, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 445);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fileNumberTB);
            this.Controls.Add(this.searchFNBtn);
            this.Controls.Add(this.showFileNumbersBtn);
            this.Controls.Add(this.showAllBtn);
            this.Controls.Add(this.DoUpdate);
            this.Controls.Add(this.chooseInputFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputFileNameText);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Form1";
            this.Text = "Clean 5D Object Names";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox inputFileNameText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button chooseInputFile;
        private System.Windows.Forms.Button DoUpdate;
        private System.Windows.Forms.Button showAllBtn;
        private System.Windows.Forms.Button showFileNumbersBtn;
        private System.Windows.Forms.Button searchFNBtn;
        private System.Windows.Forms.TextBox fileNumberTB;
        private System.Windows.Forms.Button button1;
    }
}

