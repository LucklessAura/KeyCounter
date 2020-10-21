using System;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace KeyCounter
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private NotifyIcon taskBarIcon;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.taskBarIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.optionsButton = new System.Windows.Forms.Button();
            this.profileComboBox = new System.Windows.Forms.ComboBox();
            this.profileLabel = new System.Windows.Forms.Label();
            this.keysCountLabel = new System.Windows.Forms.Label();
            this.clearProfileButton = new System.Windows.Forms.Button();
            this.keysListView = new System.Windows.Forms.ListView();
            this.collectionsComboBox = new System.Windows.Forms.ComboBox();
            this.collectionLabel = new System.Windows.Forms.Label();
            this.deleteProfileButton = new System.Windows.Forms.Button();
            this.newProfileButton = new System.Windows.Forms.Button();
            this.profilesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // taskBarIcon
            // 
            this.taskBarIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.taskBarIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("taskBarIcon.Icon")));
            this.taskBarIcon.Text = "notifyIcon1";
            this.taskBarIcon.Visible = true;
            this.taskBarIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // optionsButton
            // 
            this.optionsButton.Location = new System.Drawing.Point(457, 110);
            this.optionsButton.Name = "optionsButton";
            this.optionsButton.Size = new System.Drawing.Size(175, 25);
            this.optionsButton.TabIndex = 0;
            this.optionsButton.Text = "Options";
            this.optionsButton.UseVisualStyleBackColor = true;
            this.optionsButton.Click += new System.EventHandler(this.optionsButton_Click);
            // 
            // profileComboBox
            // 
            this.profileComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.profileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileComboBox.FormattingEnabled = true;
            this.profileComboBox.Location = new System.Drawing.Point(12, 41);
            this.profileComboBox.Name = "profileComboBox";
            this.profileComboBox.Size = new System.Drawing.Size(225, 23);
            this.profileComboBox.TabIndex = 1;
            this.profileComboBox.SelectionChangeCommitted += new System.EventHandler(this.profileComboBox_SelectedIndexChanged);
            // 
            // profileLabel
            // 
            this.profileLabel.AutoSize = true;
            this.profileLabel.Location = new System.Drawing.Point(12, 21);
            this.profileLabel.Name = "profileLabel";
            this.profileLabel.Size = new System.Drawing.Size(41, 15);
            this.profileLabel.TabIndex = 2;
            this.profileLabel.Text = "Profile";
            // 
            // keysCountLabel
            // 
            this.keysCountLabel.AutoSize = true;
            this.keysCountLabel.Location = new System.Drawing.Point(12, 163);
            this.keysCountLabel.Name = "keysCountLabel";
            this.keysCountLabel.Size = new System.Drawing.Size(67, 15);
            this.keysCountLabel.TabIndex = 4;
            this.keysCountLabel.Text = "Keys Count";
            // 
            // clearProfileButton
            // 
            this.clearProfileButton.Location = new System.Drawing.Point(260, 39);
            this.clearProfileButton.Name = "clearProfileButton";
            this.clearProfileButton.Size = new System.Drawing.Size(175, 25);
            this.clearProfileButton.TabIndex = 5;
            this.clearProfileButton.Text = "Clear Profile";
            this.clearProfileButton.UseVisualStyleBackColor = true;
            this.clearProfileButton.Click += new System.EventHandler(this.clearProfileButton_Click);
            // 
            // keysListView
            // 
            this.keysListView.BackColor = System.Drawing.SystemColors.Window;
            this.keysListView.HideSelection = false;
            this.keysListView.Location = new System.Drawing.Point(12, 181);
            this.keysListView.MultiSelect = false;
            this.keysListView.Name = "keysListView";
            this.keysListView.Size = new System.Drawing.Size(627, 375);
            this.keysListView.TabIndex = 6;
            this.keysListView.UseCompatibleStateImageBehavior = false;
            // 
            // collectionsComboBox
            // 
            this.collectionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.collectionsComboBox.FormattingEnabled = true;
            this.collectionsComboBox.Location = new System.Drawing.Point(12, 110);
            this.collectionsComboBox.Name = "collectionsComboBox";
            this.collectionsComboBox.Size = new System.Drawing.Size(225, 23);
            this.collectionsComboBox.TabIndex = 7;
            this.collectionsComboBox.SelectedIndexChanged += new System.EventHandler(this.collectionsComboBox_SelectedIndexChanged);
            this.collectionsComboBox.DataSourceChanged += new System.EventHandler(this.collectionsComboBox_SelectedIndexChanged);
            // 
            // collectionLabel
            // 
            this.collectionLabel.AutoSize = true;
            this.collectionLabel.Location = new System.Drawing.Point(12, 92);
            this.collectionLabel.Name = "collectionLabel";
            this.collectionLabel.Size = new System.Drawing.Size(61, 15);
            this.collectionLabel.TabIndex = 8;
            this.collectionLabel.Text = "Collection";
            // 
            // deleteProfileButton
            // 
            this.deleteProfileButton.Location = new System.Drawing.Point(457, 38);
            this.deleteProfileButton.Name = "deleteProfileButton";
            this.deleteProfileButton.Size = new System.Drawing.Size(175, 25);
            this.deleteProfileButton.TabIndex = 5;
            this.deleteProfileButton.Text = "Delete Profile";
            this.deleteProfileButton.UseVisualStyleBackColor = true;
            this.deleteProfileButton.Click += new System.EventHandler(this.deleteProfileButton_Click);
            // 
            // newProfileButton
            // 
            this.newProfileButton.Location = new System.Drawing.Point(260, 110);
            this.newProfileButton.Name = "newProfileButton";
            this.newProfileButton.Size = new System.Drawing.Size(175, 25);
            this.newProfileButton.TabIndex = 5;
            this.newProfileButton.Text = "New Profile";
            this.newProfileButton.UseVisualStyleBackColor = true;
            this.newProfileButton.Click += new System.EventHandler(this.newProfileButton_Click);
            // 
            // profilesContextMenu
            // 
            this.profilesContextMenu.Name = "profilesContextMenu";
            this.profilesContextMenu.Size = new System.Drawing.Size(61, 4);
            this.profilesContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.profilesContextMenu_ItemClicked);
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(653, 575);
            this.Controls.Add(this.newProfileButton);
            this.Controls.Add(this.deleteProfileButton);
            this.Controls.Add(this.collectionLabel);
            this.Controls.Add(this.collectionsComboBox);
            this.Controls.Add(this.keysListView);
            this.Controls.Add(this.clearProfileButton);
            this.Controls.Add(this.keysCountLabel);
            this.Controls.Add(this.profileLabel);
            this.Controls.Add(this.profileComboBox);
            this.Controls.Add(this.optionsButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }




        #endregion

        private Button optionsButton;
        private Label profileLabel;
        private Label keysCountLabel;
        private ComboBox profileComboBox;
        private Button clearProfileButton;
        private ListView keysListView;
        private ComboBox collectionsComboBox;
        private Label collectionLabel;
        private Button deleteProfileButton;
        private Button newProfileButton;
        private ContextMenuStrip profilesContextMenu;
    }
}

