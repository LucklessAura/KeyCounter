using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace KeyCounter
{
    /// <summary>
    /// A form used to show and modify options
    /// </summary>
    public partial class OptionsForm : Form
    {
        public bool StartWithWindows { get; set; }
        public string ProfilesLocation { get; set; }
        public bool UseLastProfile { get; set; }
        public bool StartMinimized { get; set; }
        public string OnStartProfile { get; set; }
        public bool UnloadImages { get; set; }

        /// <summary>
        /// Constructor for the <c>OptionsForm</c> class initializing the parameters
        /// </summary>
        /// <param name="options"> the currently loaded options</param>
        public OptionsForm(Options options)
        {
            InitializeComponent();

            // Initialize each of the forms proprieties according to the received options object
            this.profilesLocationTextBox.Text = options.ProfilesLocation;
            this.ProfilesLocation = options.ProfilesLocation;
            this.startWithWindowsCheckBox.Checked = this.StartWithWindows = options.AutoStart;
            this.onStartProfileCheckBox.Checked = this.UseLastProfile = options.UseLastProfile;
            this.OnStartProfile = options.OnStartProfile;
            this.startMinimisedCheckBox.Checked = this.StartMinimized = options.StartMinimized;
            this.unloadImagesCheckBox.Checked = this.UnloadImages = options.UnloadImages;

            this.profilesComboBox.DataSource = new BindingSource { DataSource = options.ProfilesList };

            this.profilesLocationFileBrowserDialog.SelectedPath = options.ProfilesLocation;


            // Give the two button a dialog result that is used to determine if the new options should be saved or discarded
            // Yes - currently loaded options should be overwritten with the newly received ones
            // No - the newly received options should be discarded, the loaded options will not be modified
            this.saveButton.DialogResult = DialogResult.Yes;

            this.resetToDefaultButton.DialogResult = DialogResult.No;

        }

        /// <summary>
        /// If the user <c>Double Clicks</c> on the text box with the current location of the profiles folder open it in a explorer window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProfilesLocationTextBox_DoubleClick(object sender, EventArgs e)
        {
            ProcessStartInfo processStart = new ProcessStartInfo
            {
                Arguments = this.ProfilesLocation,
                FileName = "explorer.exe"
            };

            Process.Start(processStart);
        }

        /// <summary>
        /// When the <c>OnStartProfileCheckBox</c> Checked propriety changes disable/ enable the <c>profilesComboBox</c>
        /// </summary>
        /// <example>
        /// <code>
        /// OnStartProfileCheckBox.Checked == true
        /// </code>
        /// <c>profilesComboBox</c> is Disabled
        /// <para>
        /// <code>
        /// OnStartProfileCheckBox.Checked == false
        /// </code>
        /// <c>profilesComboBox</c> is Enabled
        /// </para>       
        /// </example>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartProfileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.profilesComboBox.Enabled = !this.onStartProfileCheckBox.Checked;
        }

        /// <summary>
        /// Show a dialog to select the new profiles folder location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePathButton_Click(object sender, EventArgs e)
        {
            this.profilesLocationFileBrowserDialog.ShowDialog();
            this.ProfilesLocation = this.profilesLocationFileBrowserDialog.SelectedPath;
            this.profilesLocationTextBox.Text = this.ProfilesLocation;
        }

        /// <summary>
        /// <para>Take the </para>
        /// <para>
        /// <c>StartMinimized</c>, <c>OnStartProfile</c>, <c>StartWithWindows</c>,<c>UseLastProfile</c> 
        /// </para>
        /// values from the 
        /// <para><c>startMinimizedCheckBox.Checked</c>,<c> profilesComboBox.SelectedItem</c>,<c>tartWithWindowsCheckBox.Checked</c>,<c>onStartProfileCheckBox.Checked</c>
        /// </para>
        /// <para>
        /// elements of the form
        /// </para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            StartMinimized = this.startMinimisedCheckBox.Checked;
            this.OnStartProfile = profilesComboBox.SelectedItem.ToString();
            StartWithWindows = startWithWindowsCheckBox.Checked;
            this.UseLastProfile = onStartProfileCheckBox.Checked;
            this.UnloadImages = unloadImagesCheckBox.Checked;
        }
    }
}
