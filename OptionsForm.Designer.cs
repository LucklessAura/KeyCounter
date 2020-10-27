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
            this.startWithWindowsCheckBox = new System.Windows.Forms.CheckBox();
            this.profilesLocationFileBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.profilesLocationTextBox = new System.Windows.Forms.TextBox();
            this.profilesLocationLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.onStartProfileCheckBox = new System.Windows.Forms.CheckBox();
            this.changePathButton = new System.Windows.Forms.Button();
            this.profilesComboBox = new System.Windows.Forms.ComboBox();
            this.profilesLabel = new System.Windows.Forms.Label();
            this.resetToDefaultButton = new System.Windows.Forms.Button();
            this.startMinimisedCheckBox = new System.Windows.Forms.CheckBox();
            this.unloadImagesCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // startWithWindowsCheckBox
            // 
            this.startWithWindowsCheckBox.AutoSize = true;
            this.startWithWindowsCheckBox.Location = new System.Drawing.Point(12, 28);
            this.startWithWindowsCheckBox.Name = "startWithWindowsCheckBox";
            this.startWithWindowsCheckBox.Size = new System.Drawing.Size(130, 19);
            this.startWithWindowsCheckBox.TabIndex = 0;
            this.startWithWindowsCheckBox.Text = "Start With Windows";
            this.startWithWindowsCheckBox.UseVisualStyleBackColor = true;
            // 
            // profilesLocationTextBox
            // 
            this.profilesLocationTextBox.Location = new System.Drawing.Point(12, 122);
            this.profilesLocationTextBox.Name = "profilesLocationTextBox";
            this.profilesLocationTextBox.ReadOnly = true;
            this.profilesLocationTextBox.Size = new System.Drawing.Size(215, 23);
            this.profilesLocationTextBox.TabIndex = 1;
            this.profilesLocationTextBox.DoubleClick += new System.EventHandler(this.ProfilesLocationTextBox_DoubleClick);
            // 
            // profilesLocationLabel
            // 
            this.profilesLocationLabel.AutoSize = true;
            this.profilesLocationLabel.Location = new System.Drawing.Point(12, 104);
            this.profilesLocationLabel.Name = "profilesLocationLabel";
            this.profilesLocationLabel.Size = new System.Drawing.Size(131, 15);
            this.profilesLocationLabel.TabIndex = 2;
            this.profilesLocationLabel.Text = "Profiles Folder Location";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(43, 305);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(114, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // onStartProfileCheckBox
            // 
            this.onStartProfileCheckBox.AutoSize = true;
            this.onStartProfileCheckBox.Location = new System.Drawing.Point(12, 174);
            this.onStartProfileCheckBox.Name = "onStartProfileCheckBox";
            this.onStartProfileCheckBox.Size = new System.Drawing.Size(215, 19);
            this.onStartProfileCheckBox.TabIndex = 4;
            this.onStartProfileCheckBox.Text = "On Start Select The Last Used Profile";
            this.onStartProfileCheckBox.UseVisualStyleBackColor = true;
            this.onStartProfileCheckBox.CheckedChanged += new System.EventHandler(this.OnStartProfileCheckBox_CheckedChanged);
            // 
            // changePathButton
            // 
            this.changePathButton.Location = new System.Drawing.Point(254, 121);
            this.changePathButton.Name = "changePathButton";
            this.changePathButton.Size = new System.Drawing.Size(114, 23);
            this.changePathButton.TabIndex = 5;
            this.changePathButton.Text = "Change Path";
            this.changePathButton.UseVisualStyleBackColor = true;
            this.changePathButton.Click += new System.EventHandler(this.ChangePathButton_Click);
            // 
            // profilesComboBox
            // 
            this.profilesComboBox.FormattingEnabled = true;
            this.profilesComboBox.Location = new System.Drawing.Point(12, 235);
            this.profilesComboBox.Name = "profilesComboBox";
            this.profilesComboBox.Size = new System.Drawing.Size(215, 23);
            this.profilesComboBox.TabIndex = 6;
            // 
            // profilesLabel
            // 
            this.profilesLabel.AutoSize = true;
            this.profilesLabel.Location = new System.Drawing.Point(12, 217);
            this.profilesLabel.Name = "profilesLabel";
            this.profilesLabel.Size = new System.Drawing.Size(145, 15);
            this.profilesLabel.TabIndex = 7;
            this.profilesLabel.Text = "On Start Select This Profile";
            // 
            // resetToDefaultButton
            // 
            this.resetToDefaultButton.Location = new System.Drawing.Point(224, 305);
            this.resetToDefaultButton.Name = "resetToDefaultButton";
            this.resetToDefaultButton.Size = new System.Drawing.Size(114, 23);
            this.resetToDefaultButton.TabIndex = 8;
            this.resetToDefaultButton.Text = "Reset To Default";
            this.resetToDefaultButton.UseVisualStyleBackColor = true;
            // 
            // startMinimisedCheckBox
            // 
            this.startMinimisedCheckBox.AutoSize = true;
            this.startMinimisedCheckBox.Location = new System.Drawing.Point(254, 28);
            this.startMinimisedCheckBox.Name = "startMinimisedCheckBox";
            this.startMinimisedCheckBox.Size = new System.Drawing.Size(109, 19);
            this.startMinimisedCheckBox.TabIndex = 9;
            this.startMinimisedCheckBox.Text = "Start Minimised";
            this.startMinimisedCheckBox.UseVisualStyleBackColor = true;
            // 
            // unloadImagesCheckBox
            // 
            this.unloadImagesCheckBox.AutoSize = true;
            this.unloadImagesCheckBox.Location = new System.Drawing.Point(12, 66);
            this.unloadImagesCheckBox.Name = "unloadImagesCheckBox";
            this.unloadImagesCheckBox.Size = new System.Drawing.Size(198, 19);
            this.unloadImagesCheckBox.TabIndex = 10;
            this.unloadImagesCheckBox.Text = "Unload Images When Minimised";
            this.unloadImagesCheckBox.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 338);
            this.Controls.Add(this.unloadImagesCheckBox);
            this.Controls.Add(this.startMinimisedCheckBox);
            this.Controls.Add(this.resetToDefaultButton);
            this.Controls.Add(this.profilesLabel);
            this.Controls.Add(this.profilesComboBox);
            this.Controls.Add(this.changePathButton);
            this.Controls.Add(this.onStartProfileCheckBox);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.profilesLocationLabel);
            this.Controls.Add(this.profilesLocationTextBox);
            this.Controls.Add(this.startWithWindowsCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OptionsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox startWithWindowsCheckBox;
        private System.Windows.Forms.FolderBrowserDialog profilesLocationFileBrowserDialog;
        private System.Windows.Forms.TextBox profilesLocationTextBox;
        private System.Windows.Forms.Label profilesLocationLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox onStartProfileCheckBox;
        private System.Windows.Forms.Button changePathButton;
        private System.Windows.Forms.ComboBox profilesComboBox;
        private System.Windows.Forms.Label profilesLabel;
        private System.Windows.Forms.Button resetToDefaultButton;
        private System.Windows.Forms.CheckBox startMinimisedCheckBox;
        private System.Windows.Forms.CheckBox unloadImagesCheckBox;
    }
}