
namespace KeyCounter
{
    partial class KeyInfo
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
            this.KeyImage_pictureBox = new System.Windows.Forms.PictureBox();
            this.KeyImage_label = new System.Windows.Forms.Label();
            this.changeKeyIcon_button = new System.Windows.Forms.Button();
            this.KeyName_textBox = new System.Windows.Forms.TextBox();
            this.KeyName_label = new System.Windows.Forms.Label();
            this.newIcon_FileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.KeyImage_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // KeyImage_pictureBox
            // 
            this.KeyImage_pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.KeyImage_pictureBox.Location = new System.Drawing.Point(37, 42);
            this.KeyImage_pictureBox.Name = "KeyImage_pictureBox";
            this.KeyImage_pictureBox.Size = new System.Drawing.Size(130, 69);
            this.KeyImage_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.KeyImage_pictureBox.TabIndex = 0;
            this.KeyImage_pictureBox.TabStop = false;
            // 
            // KeyImage_label
            // 
            this.KeyImage_label.AutoSize = true;
            this.KeyImage_label.Location = new System.Drawing.Point(74, 24);
            this.KeyImage_label.Name = "KeyImage_label";
            this.KeyImage_label.Size = new System.Drawing.Size(52, 15);
            this.KeyImage_label.TabIndex = 1;
            this.KeyImage_label.Text = "Key Icon";
            // 
            // changeKeyIcon_button
            // 
            this.changeKeyIcon_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changeKeyIcon_button.Location = new System.Drawing.Point(57, 117);
            this.changeKeyIcon_button.Name = "changeKeyIcon_button";
            this.changeKeyIcon_button.Size = new System.Drawing.Size(79, 27);
            this.changeKeyIcon_button.TabIndex = 2;
            this.changeKeyIcon_button.Text = "Change";
            this.changeKeyIcon_button.UseVisualStyleBackColor = true;
            this.changeKeyIcon_button.Click += new System.EventHandler(this.changeKeyIcon_button_Click);
            // 
            // KeyName_textBox
            // 
            this.KeyName_textBox.Location = new System.Drawing.Point(5, 184);
            this.KeyName_textBox.Name = "KeyName_textBox";
            this.KeyName_textBox.Size = new System.Drawing.Size(182, 23);
            this.KeyName_textBox.TabIndex = 3;
            this.KeyName_textBox.TextChanged += new System.EventHandler(this.KeyName_textBox_TextChanged);
            // 
            // KeyName_label
            // 
            this.KeyName_label.AutoSize = true;
            this.KeyName_label.Location = new System.Drawing.Point(65, 166);
            this.KeyName_label.Name = "KeyName_label";
            this.KeyName_label.Size = new System.Drawing.Size(61, 15);
            this.KeyName_label.TabIndex = 4;
            this.KeyName_label.Text = "Key Name";
            // 
            // newIcon_FileDialog
            // 
            this.newIcon_FileDialog.Filter = "Images|*.png";
            this.newIcon_FileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.newIcon_FileDialog_FileOk);
            // 
            // KeyInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 219);
            this.Controls.Add(this.KeyName_label);
            this.Controls.Add(this.KeyName_textBox);
            this.Controls.Add(this.changeKeyIcon_button);
            this.Controls.Add(this.KeyImage_label);
            this.Controls.Add(this.KeyImage_pictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KeyInfo";
            this.Text = "KeyInfo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KeyInfo_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.KeyImage_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label KeyImage_label;
        private System.Windows.Forms.Button changeKeyIcon_button;
        private System.Windows.Forms.Label KeyName_label;
        public System.Windows.Forms.PictureBox KeyImage_pictureBox;
        public System.Windows.Forms.TextBox KeyName_textBox;
        private System.Windows.Forms.OpenFileDialog newIcon_FileDialog;
    }
}