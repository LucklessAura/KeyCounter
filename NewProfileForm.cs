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
        private readonly BindingList<string> _profiles;

        /// <summary>
        /// Constructor for the <c>NewProfileForm</c> class initializing the parameters
        /// </summary>
        /// <param name="options"> the currently loaded options</param>
        public NewProfileForm(Options options)
        {
            InitializeComponent();
            
            // Give the Save button a dialog result
            this.createProfileButton.DialogResult = DialogResult.Yes;

            this.createProfileButton.Enabled = false;

            //Initialize the _profile with the options Profiles List and errorTextBox with an error message
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

            // if the profile name is not in the profiles list and has a length > 0 activate the Save button
            if (profileNameTextBox.Text.Length > 0 && !_profiles.Contains(profileNameTextBox.Text))
            {
                this.createProfileButton.Enabled = true;
                errorTextBox.Text = "";
                this.ProfileName = profileNameTextBox.Text;
            }
            
        }

        /// <summary>
        /// <c>NeedsGamepad</c> receives the <c>needsGamepadCheckBox.Checked</c> value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void needsGamepadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.NeedsGamepad = needsGamepadCheckBox.Checked;
        }
    }
}
