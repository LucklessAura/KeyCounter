using System;
using System.Drawing;
using System.Windows.Forms;

namespace KeyCounter
{
    public partial class AddProfile : Form
    {
        public AddProfile()
        {
            InitializeComponent();
        }

        private void profileName_textBox_TextChanged(object sender, EventArgs e)
        {
            if (Owner is keyCounterMainFrame_frame owner && owner.IsProfileNameValid(profileName_textBox.Text) && profileName_textBox.Text.Length > 0)
            {
                profileName_textBox.BackColor = SystemColors.Window;
                createProfile_button.Enabled = true;
            }
            else
            {
                profileName_textBox.BackColor = Color.Red;
                createProfile_button.Enabled = false;
            }
        }

        private void createProfile_button_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (Owner is keyCounterMainFrame_frame owner)
                {
                    owner.CreateNewProfile(profileName_textBox.Text, usesGamepad_checkBox.Checked);
                }
                Close();
                Dispose();
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to add a new profile");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to add a new profile");
                    
                }
                throw exception;
            }
        }
    }
}
