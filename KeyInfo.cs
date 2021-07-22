using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace KeyCounter
{
    public partial class KeyInfo : Form
    {
        private Image _newImage;
        public KeyInfo()
        {
            InitializeComponent();
        }



        /// <summary>
        /// If needed tell teh main form to rename a key and change its icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var owner = this.Owner as keyCounterMainFrame_frame;
                if (!Text.Equals(KeyName_textBox.Text) && KeyName_textBox.BackColor == SystemColors.Window)
                {
                    //if the user selected a new icon and renamed the key at the same time then the key receives the new name with a zero width space at the end and the icon given by the user
                    if (_newImage != null)
                    {
                        owner?.RenameAndChangeIcon(Text, KeyName_textBox.Text + " ",_newImage);
                    }
                    else
                    {
                        owner?.RenameAndChangeIcon(Text, KeyName_textBox.Text,null);
                    }

                
                    _newImage?.Dispose();
                    KeyImage_pictureBox.Image.Dispose();
                    return;
                }

                if (_newImage!=null)
                {
                    owner?.RenameAndChangeIcon(Text, KeyName_textBox.Text + " ",_newImage);
                    _newImage.Dispose();
                }
                KeyImage_pictureBox.Dispose();
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to close info form");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to close info form");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Resize and center a given image while keeping the initial image aspect ratio to the same dimensions that the key images usually use,
        /// surplus space from resizing the image is transparent
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public void ResizeImage(ref Image image)
        {
            try
            {
                float ratio = Math.Min( (float)130/image.Width , (float)69/image.Height);
                int newWidth = (int) (image.Width * ratio);
                int newHeight = (int)(image.Height*ratio);

                var finalImage = new Bitmap(130, 69);
                using (var graphics = Graphics.FromImage(finalImage))
                {
                    int x = (finalImage.Width / 2) - newWidth / 2;
                    int y = (finalImage.Height / 2) - newHeight / 2;
                    graphics.DrawImage(image, x, y, newWidth,newHeight);

                }

                image.Dispose();
                image = null;
                image = finalImage;
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to resize an image");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to resize an image");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Upload the newly selected png 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newIcon_FileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (FileStream stream = new FileStream(newIcon_FileDialog.FileName, FileMode.Open, FileAccess.Read))
            {
                _newImage = Image.FromStream(stream);
            }
            
            KeyImage_pictureBox.Image.Dispose();
            KeyImage_pictureBox.Image = null;
            ResizeImage(ref _newImage);
            
            KeyImage_pictureBox.Image = _newImage;

        }


        /// <summary>
        /// Open a file dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeKeyIcon_button_Click(object sender, System.EventArgs e)
        {
            newIcon_FileDialog.ShowDialog();
        }


        /// <summary>
        /// Check if the user wants to rename the key to its original name
        /// </summary>
        /// <param name="rename"></param>
        /// <param name="maybeOriginal"></param>
        /// <returns></returns>
        private bool IsRenamedTo(string maybeOriginal, string rename)
        {
            
            try
            {
                if (ProfileManager.CurrentDevice.RenamesContainsKey(maybeOriginal))
                {
                    return ProfileManager.CurrentDevice.KeysRename[maybeOriginal] == rename; 
                }

                return false;
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to see if a key is in keys rename");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to see if a key is in keys rename");
                    
                }
                throw exception;
            }

        }

        /// <summary>
        /// Checks the validity of the new name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyName_textBox_TextChanged(object sender, EventArgs e)
        {
            //new name is valid if the text is same as the window name, the original name of the key, or if the new name is not one of the original key names and
            // not present in the rename dictionary and has length > 0
            if (KeyName_textBox.Text.Equals(Text) || IsRenamedTo(KeyName_textBox.Text,Text) || (!ProfileManager.CurrentDevice.ImagesContainKey(KeyName_textBox.Text) && !ProfileManager.CurrentDevice.KeysCount.Keys.ContainsKey(KeyName_textBox.Text) && !ProfileManager.CurrentDevice.RenamesContainsValue(KeyName_textBox.Text) && KeyName_textBox.Text.Length>0))
            {
                KeyName_textBox.BackColor = SystemColors.Window;
            }
            else
            {
                // back color of the text box is also used as a flag to see if the text is valid when needing to replace the key name 
                KeyName_textBox.BackColor = Color.Red;
            }
        }
    }
}
