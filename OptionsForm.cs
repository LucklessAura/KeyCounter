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
        public bool StartMinimised { get; set; }
        public string OnStartProfile { get; set; }

        /// <summary>
        /// Constructor for the <c>OptionsForm</c> class initializing the parameters
        /// </summary>
        /// <param name="options"> the currently loaded options</param>
        public OptionsForm(Options options)
        {
            InitializeComponent();

            ///<summary>
            /// Initialize each of the forms proprieties according to the receved <c>options </c> object
            /// </summary>
            this.profilesLocationTextBox.Text = options.ProfilesLocation;
            this.ProfilesLocation = options.ProfilesLocation;
            this.startWithWindowsCheckBox.Checked = this.StartWithWindows = options.AutoStart;
            this.onStartProfileCheckBox.Checked = this.UseLastProfile = options.UseLastProfile;
            this.OnStartProfile = options.OnStartProfile;
            this.startMinimisedCheckBox.Checked = this.StartMinimised = options.StartMinimised;

            this.profilesComboBox.DataSource = new BindingSource { DataSource = options.ProfilesList };

            this.profilesLocationFileBrowserDialog.SelectedPath = options.ProfilesLocation;


            ///<summary>
            /// Give the two button a dialog result that is used to determine if the new options should be saved or discarded
            /// <para>Yes - currently loaded options should be overwritten with the newly receved ones</para>
            /// <para>No - the newly receved options should be discarded, the loaded options will not be modified</para>
            /// </summary>
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
            if (this.onStartProfileCheckBox.Checked)
            {
                this.profilesComboBox.Enabled = false;
            }
            else
            {
                this.profilesComboBox.Enabled = true;
            }
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
        /// When a new selection is made int the <c>ProfilesComboBox</c> change the <c>OnStartProfile</c> to the new selection text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProfilesComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.OnStartProfile = profilesComboBox.SelectedItem.ToString();
        }

        /// <summary>
        /// <para>Take the </para>
        /// <para>
        /// <c>StartMinimised</c>, <c>OnStartProfile</c>, <c>StartWithWindows</c>,<c>UseLastProfile</c> 
        /// </para>
        /// values from the 
        /// <para><c>startMinimisedCheckBox.Checked</c>,<c> profilesComboBox.SelectedItem</c>,<c>tartWithWindowsCheckBox.Checked</c>,<c>onStartProfileCheckBox.Checked</c>
        /// </para>
        /// <para>
        /// elements of the form
        /// </para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            StartMinimised = this.startMinimisedCheckBox.Checked;
            this.OnStartProfile = profilesComboBox.SelectedItem.ToString();
            StartWithWindows = startWithWindowsCheckBox.Checked;
            this.UseLastProfile = onStartProfileCheckBox.Checked;
        }
    }
}
