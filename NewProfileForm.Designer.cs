namespace KeyCounter
{
    partial class NewProfileForm
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
            this.createProfileButton = new System.Windows.Forms.Button();
            this.needsGamepadCheckBox = new System.Windows.Forms.CheckBox();
            this.profileNameTextBox = new System.Windows.Forms.TextBox();
            this.profileNameLabel = new System.Windows.Forms.Label();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // createProfileButton
            // 
            this.createProfileButton.Location = new System.Drawing.Point(12, 137);
            this.createProfileButton.Name = "createProfileButton";
            this.createProfileButton.Size = new System.Drawing.Size(175, 25);
            this.createProfileButton.TabIndex = 3;
            this.createProfileButton.Text = "Create Profile";
            this.createProfileButton.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.needsGamepadCheckBox.AutoSize = true;
            this.needsGamepadCheckBox.Location = new System.Drawing.Point(12, 67);
            this.needsGamepadCheckBox.Name = "checkBox1";
            this.needsGamepadCheckBox.Size = new System.Drawing.Size(104, 19);
            this.needsGamepadCheckBox.TabIndex = 2;
            this.needsGamepadCheckBox.Text = "Uses Gamepad";
            this.needsGamepadCheckBox.UseVisualStyleBackColor = true;
            this.needsGamepadCheckBox.CheckedChanged += new System.EventHandler(this.needsGamepadCheckBox_CheckedChanged);
            // 
            // profileNameTextBox
            // 
            this.profileNameTextBox.Location = new System.Drawing.Point(12, 38);
            this.profileNameTextBox.Name = "profileNameTextBox";
            this.profileNameTextBox.Size = new System.Drawing.Size(175, 23);
            this.profileNameTextBox.TabIndex = 1;
            this.profileNameTextBox.TextChanged += new System.EventHandler(this.profileNameTextBox_TextChanged);
            // 
            // profileNameLabel
            // 
            this.profileNameLabel.AutoSize = true;
            this.profileNameLabel.Location = new System.Drawing.Point(12, 20);
            this.profileNameLabel.Name = "profileNameLabel";
            this.profileNameLabel.Size = new System.Drawing.Size(76, 15);
            this.profileNameLabel.TabIndex = 3;
            this.profileNameLabel.Text = "Profile Name";
            // 
            // errorTextBox
            // 
            this.errorTextBox.BackColor = System.Drawing.SystemColors.MenuBar;
            this.errorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.errorTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.errorTextBox.ForeColor = System.Drawing.Color.Red;
            this.errorTextBox.Location = new System.Drawing.Point(12, 92);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.Size = new System.Drawing.Size(173, 39);
            this.errorTextBox.TabIndex = 0;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NewProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(197, 174);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.profileNameLabel);
            this.Controls.Add(this.profileNameTextBox);
            this.Controls.Add(this.needsGamepadCheckBox);
            this.Controls.Add(this.createProfileButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProfileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create New Profile";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createProfileButton;
        private System.Windows.Forms.CheckBox needsGamepadCheckBox;
        private System.Windows.Forms.TextBox profileNameTextBox;
        private System.Windows.Forms.Label profileNameLabel;
        private System.Windows.Forms.TextBox errorTextBox;
    }
}