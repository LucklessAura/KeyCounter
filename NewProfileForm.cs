using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace KeyCounter
{
    /// <summary>
    /// Form to create a new <c>Profile</c> from user input
    /// </summary>
    public partial class NewProfileForm : Form
    {

        public string ProfileName { get; set; }
        public bool NeedsGamepad { get; set; }
        private BindingList<string> _profiles;

        /// <summary>
        /// Constructor for the <c>NewProfileForm</c> class initializing the parameters
        /// </summary>
        /// <param name="options"> the currently loaded options</param>
        public NewProfileForm(Options options)
        {
            InitializeComponent();
            ///<summary>
            /// Give the <c>Save</c> button a dialog result
            ///</summary>
            this.createProfileButton.DialogResult = DialogResult.Yes;

            ///<summary>
            /// Disable the <c>Save button</c>
            /// </summary>
            this.createProfileButton.Enabled = false;

            ///<summary>
            ///Initialize the <c>_profile</c> with the options Profiles List and <c>errorTextBox</c> with an error message
            /// </summary>
            _profiles = options.ProfilesList;
            errorTextBox.Text = "Length must be bigger than 0.";

        }

        /// <summary>
        /// When the text of the <c>profileNameTextBox</c> is changed show an error in the <c>errorTextBox</c> if the length of 
        /// the text is 0 or if the text is already in the profiles list to avoid multiple profiles with the same name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (profileNameTextBox.Text.Length == 0)
            {
                errorTextBox.Text = "Length must be bigger than 0.";
                this.createProfileButton.Enabled = false;
            }

            if (_profiles.Contains(profileNameTextBox.Text))
            {
                this.createProfileButton.Enabled = false;
                errorTextBox.Text = "Can't create multiple profiles with the same name.";
            }

            ///<summary>
            /// if the profile name is not in the profiles list and has a length > 0 activate the <c>Save</c> button
            /// </summary>
            if (profileNameTextBox.Text.Length > 0 && !_profiles.Contains(profileNameTextBox.Text))
            {
                this.createProfileButton.Enabled = true;
                errorTextBox.Text = "";
                this.ProfileName = profileNameTextBox.Text;
            }
            
        }

        /// <summary>
        /// <c>NeedsGamepad</c> receves the <c>needsGamepadCheckBox.Checked</c> value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void needsGamepadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.NeedsGamepad = needsGamepadCheckBox.Checked;
        }
    }
}
