namespace SoCute
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
            this.btnSupFolder = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboLanguage = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.progressProcess = new System.Windows.Forms.ProgressBar();
            this.listSup = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSupFolder
            // 
            this.btnSupFolder.Location = new System.Drawing.Point(6, 198);
            this.btnSupFolder.Name = "btnSupFolder";
            this.btnSupFolder.Size = new System.Drawing.Size(801, 23);
            this.btnSupFolder.TabIndex = 1;
            this.btnSupFolder.Text = "Search...";
            this.btnSupFolder.UseVisualStyleBackColor = true;
            this.btnSupFolder.Click += new System.EventHandler(this.btnSupFolder_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listSup);
            this.groupBox1.Controls.Add(this.btnSupFolder);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(813, 230);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1 - Your SUP file folder (from VobEdit)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboLanguage);
            this.groupBox2.Location = new System.Drawing.Point(12, 248);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(813, 49);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2 - Your favorite language for ASS output (must be the SUP language)";
            // 
            // comboLanguage
            // 
            this.comboLanguage.FormattingEnabled = true;
            this.comboLanguage.ItemHeight = 13;
            this.comboLanguage.Location = new System.Drawing.Point(6, 19);
            this.comboLanguage.Name = "comboLanguage";
            this.comboLanguage.Size = new System.Drawing.Size(801, 21);
            this.comboLanguage.TabIndex = 0;
            this.comboLanguage.SelectedIndexChanged += new System.EventHandler(this.comboLanguage_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.progressProcess);
            this.groupBox3.Controls.Add(this.btnProcess);
            this.groupBox3.Location = new System.Drawing.Point(12, 303);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(813, 81);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "3 - Process";
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(6, 19);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(801, 23);
            this.btnProcess.TabIndex = 0;
            this.btnProcess.Text = "Do and wait";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // progressProcess
            // 
            this.progressProcess.Location = new System.Drawing.Point(6, 48);
            this.progressProcess.Name = "progressProcess";
            this.progressProcess.Size = new System.Drawing.Size(801, 23);
            this.progressProcess.TabIndex = 1;
            // 
            // listSup
            // 
            this.listSup.FormattingEnabled = true;
            this.listSup.Location = new System.Drawing.Point(12, 19);
            this.listSup.Name = "listSup";
            this.listSup.Size = new System.Drawing.Size(795, 173);
            this.listSup.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 397);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSupFolder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboLanguage;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ProgressBar progressProcess;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.ListBox listSup;
    }
}

