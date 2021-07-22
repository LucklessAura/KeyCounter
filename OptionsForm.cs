using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using File = System.IO.File;

namespace KeyCounter
{
    public partial class OptionsForm : Form
    {
        private readonly Options _options;
        private bool _shouldMove;
        public string StartProfilesLocations;
        public string DestinationProfilesLocations;


        public OptionsForm(ref Options options)
        {
            
            InitializeComponent();
            _options = options;
            StartProfilesLocations = options.ProfilesLocation;
            onStartProfile_comboBox.DataSource = options.ProfileList;
            onStartProfile_comboBox.SelectedItem = options.LastUsedProfile;
            autoStart_checkBox.DataBindings.Add("Checked", options,"AutoStart",false, DataSourceUpdateMode.OnPropertyChanged);
            startMinimised_checkBox.DataBindings.Add("Checked", options,"StartMinimized",false, DataSourceUpdateMode.OnPropertyChanged);
            profilesLocation_textBox.DataBindings.Add("Text", options,"ProfilesLocation",false, DataSourceUpdateMode.OnPropertyChanged);
            useLastProfile_checkBox.DataBindings.Add("Checked", options,"UseLastProfile",false, DataSourceUpdateMode.OnPropertyChanged);
            onStartProfile_comboBox.DataBindings.Add("Text", options,"OnStartProfile",false, DataSourceUpdateMode.OnPropertyChanged);
            
        }

        /// <summary>
        /// Enable or disable the start profile combo box depending on the state of the use last profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void useLastProfile_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox x)
            {
                onStartProfile_comboBox.Enabled = !x.Checked;
            }
        }

        /// <summary>
        /// Set the new location of the files and the should move flag to true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeProfilesLocation_button_Click(object sender, EventArgs e)
        {
            profilesLocation_folderBrowserDialog.SelectedPath = profilesLocation_textBox.Text + "\\";
            DialogResult result = profilesLocation_folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _shouldMove = true;
                DestinationProfilesLocations = profilesLocation_folderBrowserDialog.SelectedPath;
            }
            profilesLocation_textBox.Text = profilesLocation_folderBrowserDialog.SelectedPath;
        }



        /// <summary>
        /// Move files tho the new location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileMover_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Console.WriteLine("Called file mover");
            var locationsPaths = e.Argument as Tuple<string,string>;
            if (locationsPaths == null)
                return;

            if (!Directory.Exists(locationsPaths.Item2))
            {
                Directory.CreateDirectory(locationsPaths.Item2);
            }

            foreach (var profile in _options.ProfileList)
            {
                File.Move(locationsPaths.Item1 +"//" + profile + ".txt",locationsPaths.Item2 + "//" + profile + ".txt");
            }
        }


        /// <summary>
        /// Set the dialog result to true if the profiles should be moved to a new location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_shouldMove)
            {
                DialogResult = DialogResult.Yes;
            }
        }


        /// <summary>
        /// Open a window of explorer to the location of the profiles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profilesLocation_textBox_DoubleClick(object sender, EventArgs e)
        {
            ProcessStartInfo processStart = new ProcessStartInfo
            {
                Arguments = _options.ProfilesLocation,
                FileName = "explorer.exe"
            };

            Process.Start(processStart);
        }



        /// <summary>
        /// If the check box is checked then put a link to the apps executable in the startup folder of the OS else try and remove the shortcut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoStart_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            
            if (autoStart_checkBox.Checked)
            {
                string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                if (!File.Exists(Path.Join(startupFolder,"Key Counter.lnk")))
                {
                    File.Copy(Path.Join(Program.CommonAppsData,"Key Counter.lnk"),Path.Join(startupFolder,"Key Counter.lnk"));
                }
            }
            else
            {
                string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                if (File.Exists(startupFolder + "\\" + "Key Counter.lnk"))
                {
                    File.Delete(startupFolder + "\\" + "Key Counter.lnk");
                }
            }
        }
    }
}
