using System.Windows.Forms;

namespace KeyCounter
{
    partial class keyCounterMainFrame_frame
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(keyCounterMainFrame_frame));
            this.clock_label = new System.Windows.Forms.Label();
            this.imagesList_listView = new System.Windows.Forms.ListView();
            this.imageLoader_backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.profile_comboBox = new System.Windows.Forms.ComboBox();
            this.inputDevice_comboBox = new System.Windows.Forms.ComboBox();
            this.clearProfile_button = new System.Windows.Forms.Button();
            this.newProfile_button = new System.Windows.Forms.Button();
            this.deleteProfile_button = new System.Windows.Forms.Button();
            this.options_button = new System.Windows.Forms.Button();
            this.taskbar_notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIcon_main_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyIcon_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon_main_toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.profiles_label = new System.Windows.Forms.Label();
            this.devices_label = new System.Windows.Forms.Label();
            this.notifyIcon_main_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // clock_label
            // 
            this.clock_label.AutoSize = true;
            this.clock_label.Location = new System.Drawing.Point(12, 627);
            this.clock_label.Name = "clock_label";
            this.clock_label.Size = new System.Drawing.Size(95, 15);
            this.clock_label.TabIndex = 0;
            this.clock_label.Text = "Time spent: 0:0:0";
            // 
            // imagesList_listView
            // 
            this.imagesList_listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.imagesList_listView.HideSelection = false;
            this.imagesList_listView.Location = new System.Drawing.Point(12, 139);
            this.imagesList_listView.Name = "imagesList_listView";
            this.imagesList_listView.Size = new System.Drawing.Size(725, 470);
            this.imagesList_listView.TabIndex = 1;
            this.imagesList_listView.UseCompatibleStateImageBehavior = false;
            this.imagesList_listView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.imagesList_listView_MouseClick);
            // 
            // imageLoader_backgroundWorker
            // 
            this.imageLoader_backgroundWorker.WorkerSupportsCancellation = true;
            this.imageLoader_backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundLoader_DoWork);
            this.imageLoader_backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundLoader_RunWorkerCompleted);
            // 
            // profile_comboBox
            // 
            this.profile_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profile_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.profile_comboBox.FormattingEnabled = true;
            this.profile_comboBox.Location = new System.Drawing.Point(12, 40);
            this.profile_comboBox.Name = "profile_comboBox";
            this.profile_comboBox.Size = new System.Drawing.Size(239, 23);
            this.profile_comboBox.TabIndex = 2;
            this.profile_comboBox.SelectedIndexChanged += new System.EventHandler(this.profile_comboBox_SelectedIndexChanged);
            // 
            // inputDevice_comboBox
            // 
            this.inputDevice_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputDevice_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inputDevice_comboBox.FormattingEnabled = true;
            this.inputDevice_comboBox.Location = new System.Drawing.Point(12, 98);
            this.inputDevice_comboBox.Name = "inputDevice_comboBox";
            this.inputDevice_comboBox.Size = new System.Drawing.Size(239, 23);
            this.inputDevice_comboBox.TabIndex = 3;
            this.inputDevice_comboBox.TextChanged += new System.EventHandler(this.inputDevice_comboBox_TextChanged);
            // 
            // clearProfile_button
            // 
            this.clearProfile_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearProfile_button.Location = new System.Drawing.Point(352, 40);
            this.clearProfile_button.Name = "clearProfile_button";
            this.clearProfile_button.Size = new System.Drawing.Size(126, 23);
            this.clearProfile_button.TabIndex = 4;
            this.clearProfile_button.Text = "Clear Profile";
            this.clearProfile_button.UseVisualStyleBackColor = true;
            this.clearProfile_button.Click += new System.EventHandler(this.clearProfile_button_Click);
            // 
            // newProfile_button
            // 
            this.newProfile_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newProfile_button.Location = new System.Drawing.Point(352, 97);
            this.newProfile_button.Name = "newProfile_button";
            this.newProfile_button.Size = new System.Drawing.Size(126, 23);
            this.newProfile_button.TabIndex = 6;
            this.newProfile_button.Text = "New Profile";
            this.newProfile_button.UseVisualStyleBackColor = true;
            this.newProfile_button.Click += new System.EventHandler(this.newProfile_button_Click);
            // 
            // deleteProfile_button
            // 
            this.deleteProfile_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteProfile_button.Location = new System.Drawing.Point(584, 40);
            this.deleteProfile_button.Name = "deleteProfile_button";
            this.deleteProfile_button.Size = new System.Drawing.Size(126, 23);
            this.deleteProfile_button.TabIndex = 4;
            this.deleteProfile_button.Text = "Delete Profile";
            this.deleteProfile_button.UseVisualStyleBackColor = true;
            this.deleteProfile_button.Click += new System.EventHandler(this.deleteProfile_button_Click);
            // 
            // options_button
            // 
            this.options_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.options_button.Location = new System.Drawing.Point(584, 98);
            this.options_button.Name = "options_button";
            this.options_button.Size = new System.Drawing.Size(126, 24);
            this.options_button.TabIndex = 6;
            this.options_button.Text = "Options";
            this.options_button.UseVisualStyleBackColor = true;
            this.options_button.Click += new System.EventHandler(this.options_button_Click);
            // 
            // taskbar_notifyIcon
            // 
            this.taskbar_notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("taskbar_notifyIcon.Icon")));
            this.taskbar_notifyIcon.Text = "Key Counter";
            this.taskbar_notifyIcon.Click += new System.EventHandler(this.taskbar_notifyIcon_Click);
            this.taskbar_notifyIcon.DoubleClick += new System.EventHandler(this.taskbar_notifyIcon_DoubleClick);
            // 
            // notifyIcon_main_contextMenuStrip
            // 
            this.notifyIcon_main_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifyIcon_toolStripMenuItem,
            this.notifyIcon_main_toolStripSeparator});
            this.notifyIcon_main_contextMenuStrip.Name = "notifyIcon_contextMenuStrip";
            this.notifyIcon_main_contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.notifyIcon_main_contextMenuStrip.ShowImageMargin = false;
            this.notifyIcon_main_contextMenuStrip.Size = new System.Drawing.Size(89, 32);
            this.notifyIcon_main_contextMenuStrip.Text = "Profies";
            // 
            // notifyIcon_toolStripMenuItem
            // 
            this.notifyIcon_toolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.notifyIcon_toolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.notifyIcon_toolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.notifyIcon_toolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.notifyIcon_toolStripMenuItem.Name = "notifyIcon_toolStripMenuItem";
            this.notifyIcon_toolStripMenuItem.Size = new System.Drawing.Size(88, 22);
            this.notifyIcon_toolStripMenuItem.Text = "Profiles";
            // 
            // notifyIcon_main_toolStripSeparator
            // 
            this.notifyIcon_main_toolStripSeparator.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.notifyIcon_main_toolStripSeparator.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.notifyIcon_main_toolStripSeparator.Name = "notifyIcon_main_toolStripSeparator";
            this.notifyIcon_main_toolStripSeparator.Size = new System.Drawing.Size(85, 6);
            // 
            // profiles_label
            // 
            this.profiles_label.AutoSize = true;
            this.profiles_label.Location = new System.Drawing.Point(12, 21);
            this.profiles_label.Name = "profiles_label";
            this.profiles_label.Size = new System.Drawing.Size(46, 15);
            this.profiles_label.TabIndex = 7;
            this.profiles_label.Text = "Profiles";
            // 
            // devices_label
            // 
            this.devices_label.AutoSize = true;
            this.devices_label.Location = new System.Drawing.Point(12, 76);
            this.devices_label.Name = "devices_label";
            this.devices_label.Size = new System.Drawing.Size(42, 15);
            this.devices_label.TabIndex = 8;
            this.devices_label.Text = "Device";
            // 
            // keyCounterMainFrame_frame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(750, 654);
            this.Controls.Add(this.devices_label);
            this.Controls.Add(this.profiles_label);
            this.Controls.Add(this.options_button);
            this.Controls.Add(this.newProfile_button);
            this.Controls.Add(this.deleteProfile_button);
            this.Controls.Add(this.clearProfile_button);
            this.Controls.Add(this.inputDevice_comboBox);
            this.Controls.Add(this.profile_comboBox);
            this.Controls.Add(this.imagesList_listView);
            this.Controls.Add(this.clock_label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "keyCounterMainFrame_frame";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Key Counter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.keyCounterMainFrame_frame_FormClosing);
            this.Load += new System.EventHandler(this.keyCounterMainFrame_frame_Load);
            this.Resize += new System.EventHandler(this.keyCounterMainFrame_frame_Resize);
            this.notifyIcon_main_contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label clock_label;
        private System.Windows.Forms.ListView imagesList_listView;
        private System.ComponentModel.BackgroundWorker imageLoader_backgroundWorker;
        private System.Windows.Forms.ComboBox profile_comboBox;
        private System.Windows.Forms.ComboBox inputDevice_comboBox;
        private System.Windows.Forms.Button clearProfile_button;
        private System.Windows.Forms.Button newProfile_button;
        private System.Windows.Forms.Button deleteProfile_button;
        private System.Windows.Forms.Button options_button;
        private System.Windows.Forms.NotifyIcon taskbar_notifyIcon;
        private ContextMenuStrip notifyIcon_main_contextMenuStrip;
        private ToolStripSeparator notifyIcon_main_toolStripSeparator;
        private ToolStripMenuItem notifyIcon_toolStripMenuItem;
        private Label profiles_label;
        private Label devices_label;
    }
}

