
namespace KeyCounter
{
    partial class AddProfile
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
            this.usesGamepad_checkBox = new System.Windows.Forms.CheckBox();
            this.profileName_label = new System.Windows.Forms.Label();
            this.createProfile_button = new System.Windows.Forms.Button();
            this.profileName_textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // usesGamepad_checkBox
            // 
            this.usesGamepad_checkBox.AutoSize = true;
            this.usesGamepad_checkBox.Location = new System.Drawing.Point(12, 109);
            this.usesGamepad_checkBox.Name = "usesGamepad_checkBox";
            this.usesGamepad_checkBox.Size = new System.Drawing.Size(104, 19);
            this.usesGamepad_checkBox.TabIndex = 0;
            this.usesGamepad_checkBox.Text = "Uses Gamepad";
            this.usesGamepad_checkBox.UseVisualStyleBackColor = true;
            // 
            // profileName_label
            // 
            this.profileName_label.AutoSize = true;
            this.profileName_label.Location = new System.Drawing.Point(12, 29);
            this.profileName_label.Name = "profileName_label";
            this.profileName_label.Size = new System.Drawing.Size(76, 15);
            this.profileName_label.TabIndex = 1;
            this.profileName_label.Text = "Profile Name";
            // 
            // createProfile_button
            // 
            this.createProfile_button.Location = new System.Drawing.Point(71, 189);
            this.createProfile_button.Name = "createProfile_button";
            this.createProfile_button.Size = new System.Drawing.Size(75, 23);
            this.createProfile_button.TabIndex = 2;
            this.createProfile_button.Text = "Create";
            this.createProfile_button.UseVisualStyleBackColor = true;
            this.createProfile_button.Click += new System.EventHandler(this.createProfile_button_Click);
            // 
            // profileName_textBox
            // 
            this.profileName_textBox.Location = new System.Drawing.Point(12, 47);
            this.profileName_textBox.Name = "profileName_textBox";
            this.profileName_textBox.Size = new System.Drawing.Size(199, 23);
            this.profileName_textBox.TabIndex = 3;
            this.profileName_textBox.TextChanged += new System.EventHandler(this.profileName_textBox_TextChanged);
            // 
            // AddProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 224);
            this.Controls.Add(this.profileName_textBox);
            this.Controls.Add(this.createProfile_button);
            this.Controls.Add(this.profileName_label);
            this.Controls.Add(this.usesGamepad_checkBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddProfile";
            this.Text = "Add New Profile";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox usesGamepad_checkBox;
        private System.Windows.Forms.Label profileName_label;
        private System.Windows.Forms.Button createProfile_button;
        private System.Windows.Forms.TextBox profileName_textBox;
    }
}