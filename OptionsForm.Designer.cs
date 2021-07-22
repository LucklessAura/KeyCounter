
namespace KeyCounter
{
    partial class OptionsForm
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
            System.Windows.Forms.Label profilesFolder_label;
            System.Windows.Forms.Label startProfile_label;
            this.autoStart_checkBox = new System.Windows.Forms.CheckBox();
            this.profilesLocation_textBox = new System.Windows.Forms.TextBox();
            this.useLastProfile_checkBox = new System.Windows.Forms.CheckBox();
            this.startMinimised_checkBox = new System.Windows.Forms.CheckBox();
            this.onStartProfile_comboBox = new System.Windows.Forms.ComboBox();
            this.changeProfilesLocation_button = new System.Windows.Forms.Button();
            this.profilesLocation_folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.fileMover_backgroundWorker = new System.ComponentModel.BackgroundWorker();
            profilesFolder_label = new System.Windows.Forms.Label();
            startProfile_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // profilesFolder_label
            // 
            profilesFolder_label.AutoSize = true;
            profilesFolder_label.Location = new System.Drawing.Point(12, 65);
            profilesFolder_label.Name = "profilesFolder_label";
            profilesFolder_label.Size = new System.Drawing.Size(82, 15);
            profilesFolder_label.TabIndex = 7;
            profilesFolder_label.Text = "Profiles Folder";
            // 
            // startProfile_label
            // 
            startProfile_label.AutoSize = true;
            startProfile_label.Location = new System.Drawing.Point(206, 188);
            startProfile_label.Name = "startProfile_label";
            startProfile_label.Size = new System.Drawing.Size(68, 15);
            startProfile_label.TabIndex = 8;
            startProfile_label.Text = "Start Profile";
            // 
            // autoStart_checkBox
            // 
            this.autoStart_checkBox.AutoSize = true;
            this.autoStart_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.autoStart_checkBox.Location = new System.Drawing.Point(12, 12);
            this.autoStart_checkBox.Name = "autoStart_checkBox";
            this.autoStart_checkBox.Size = new System.Drawing.Size(127, 19);
            this.autoStart_checkBox.TabIndex = 0;
            this.autoStart_checkBox.Text = "Start With Windows";
            this.autoStart_checkBox.UseVisualStyleBackColor = true;
            this.autoStart_checkBox.CheckedChanged += new System.EventHandler(this.autoStart_checkBox_CheckedChanged);
            // 
            // profilesLocation_textBox
            // 
            this.profilesLocation_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.profilesLocation_textBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.profilesLocation_textBox.Location = new System.Drawing.Point(11, 83);
            this.profilesLocation_textBox.Multiline = true;
            this.profilesLocation_textBox.Name = "profilesLocation_textBox";
            this.profilesLocation_textBox.ReadOnly = true;
            this.profilesLocation_textBox.Size = new System.Drawing.Size(264, 41);
            this.profilesLocation_textBox.TabIndex = 1;
            this.profilesLocation_textBox.Text = "Profiles location";
            this.profilesLocation_textBox.DoubleClick += new System.EventHandler(this.profilesLocation_textBox_DoubleClick);
            // 
            // useLastProfile_checkBox
            // 
            this.useLastProfile_checkBox.AutoSize = true;
            this.useLastProfile_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.useLastProfile_checkBox.Location = new System.Drawing.Point(11, 157);
            this.useLastProfile_checkBox.Name = "useLastProfile_checkBox";
            this.useLastProfile_checkBox.Size = new System.Drawing.Size(103, 19);
            this.useLastProfile_checkBox.TabIndex = 2;
            this.useLastProfile_checkBox.Text = "Use Last Profile";
            this.useLastProfile_checkBox.UseVisualStyleBackColor = true;
            this.useLastProfile_checkBox.CheckedChanged += new System.EventHandler(this.useLastProfile_checkBox_CheckedChanged);
            // 
            // startMinimised_checkBox
            // 
            this.startMinimised_checkBox.AutoSize = true;
            this.startMinimised_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startMinimised_checkBox.Location = new System.Drawing.Point(369, 12);
            this.startMinimised_checkBox.Name = "startMinimised_checkBox";
            this.startMinimised_checkBox.Size = new System.Drawing.Size(106, 19);
            this.startMinimised_checkBox.TabIndex = 3;
            this.startMinimised_checkBox.Text = "Start Minimised";
            this.startMinimised_checkBox.UseVisualStyleBackColor = true;
            // 
            // onStartProfile_comboBox
            // 
            this.onStartProfile_comboBox.FormattingEnabled = true;
            this.onStartProfile_comboBox.ItemHeight = 15;
            this.onStartProfile_comboBox.Location = new System.Drawing.Point(120, 206);
            this.onStartProfile_comboBox.Name = "onStartProfile_comboBox";
            this.onStartProfile_comboBox.Size = new System.Drawing.Size(253, 23);
            this.onStartProfile_comboBox.TabIndex = 5;
            // 
            // changeProfilesLocation_button
            // 
            this.changeProfilesLocation_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changeProfilesLocation_button.Location = new System.Drawing.Point(305, 83);
            this.changeProfilesLocation_button.Name = "changeProfilesLocation_button";
            this.changeProfilesLocation_button.Size = new System.Drawing.Size(170, 41);
            this.changeProfilesLocation_button.TabIndex = 6;
            this.changeProfilesLocation_button.Text = "Change Profiles Location";
            this.changeProfilesLocation_button.UseVisualStyleBackColor = false;
            this.changeProfilesLocation_button.Click += new System.EventHandler(this.changeProfilesLocation_button_Click);
            // 
            // fileMover_backgroundWorker
            // 
            this.fileMover_backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.fileMover_backgroundWorker_DoWork);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(488, 247);
            this.Controls.Add(startProfile_label);
            this.Controls.Add(profilesFolder_label);
            this.Controls.Add(this.changeProfilesLocation_button);
            this.Controls.Add(this.onStartProfile_comboBox);
            this.Controls.Add(this.startMinimised_checkBox);
            this.Controls.Add(this.useLastProfile_checkBox);
            this.Controls.Add(this.profilesLocation_textBox);
            this.Controls.Add(this.autoStart_checkBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox autoStart_checkBox;
        private System.Windows.Forms.TextBox profilesLocation_textBox;
        private System.Windows.Forms.CheckBox useLastProfile_checkBox;
        private System.Windows.Forms.CheckBox startMinimised_checkBox;
        private System.Windows.Forms.ComboBox onStartProfile_comboBox;
        private System.Windows.Forms.Button changeProfilesLocation_button;
        private System.Windows.Forms.FolderBrowserDialog profilesLocation_folderBrowserDialog;
        internal System.ComponentModel.BackgroundWorker fileMover_backgroundWorker;
    }
}