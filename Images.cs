using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;


namespace KeyCounter
{
    /// <summary>
    /// Class responsable with loading into memory, creation and provision of images corresponding with the 
    /// keys of the keyboard
    /// </summary>
    static class KeyboardImages
    {
        private static string _execDirectoryPath = Path.GetDirectoryName(Application.ExecutablePath);
        private static FontFamily _fontFamily = new FontFamily("Candara");
        private static string _imagesPath = "\\Icons\\Keyboard\\";
        private static Font _font;
        private static Image _tempImage;
        private static Dictionary<string, Image> _keyboardImages;

        /// <summary>
        /// initialize dictionary with all images in the corresponding folder
        /// </summary>
        public static void Initialize()
        {
            _keyboardImages = new Dictionary<string, Image>();
            foreach (string image in Directory.EnumerateFiles(_execDirectoryPath + _imagesPath))
            {
                if (image.EndsWith(".png"))
                {
                    _keyboardImages.Add(Path.GetFileNameWithoutExtension(image), Bitmap.FromFile(image));
                }
            }
        }

        /// <summary>
        /// Return an image for the <paramref name="str"/>, the image is created and saved in the corresponding folder if
        /// it does not already exist
        /// </summary>
        /// <param name="str">string for which an image is created/ returned </param>
        /// <returns>an Image corresponding to the provided key</returns>
        public static Image GetImageForKey(string str)
        {
            //verify if the image is in the dictionary, if yes return it
            if (_keyboardImages.ContainsKey(str))
            {
                return _keyboardImages[str];
            }
            // else create it using the base_key or base_key_long
            else
            {
                //if the length of str is <=2 use base_key else base_key_long
                if (str.Length > 2)
                {
                    _tempImage = (Image)_keyboardImages["base_key_long"].Clone();
                }
                else
                {
                    _tempImage = (Image)_keyboardImages["base_key"].Clone();
                }
                using (var g = Graphics.FromImage(_tempImage))
                {
                    int size = 150;
                    _font = new Font(_fontFamily, size);
                    // keep making the font smaller until it fits vertically and horizontaly on the white space of the key image
                    while (g.MeasureString(str, _font).Width > _tempImage.Width || g.MeasureString(str, _font).Height > _tempImage.Height)
                    {
                        _font.Dispose();
                        size -= 5;
                        _font = new Font(_fontFamily, size);
                    }

                    // draw on the image the str
                    g.DrawString(str, _font, Brushes.Gray, new Point((int)((_tempImage.Width - g.MeasureString(str, _font).Width) / 2), (int)((_tempImage.Height - g.MeasureString(str, _font).Height) / 2)));

                }
                _keyboardImages.Add(str, _tempImage);
                _tempImage.Save(_execDirectoryPath + _imagesPath + str + ".png", ImageFormat.Png);
                _font.Dispose();
                return _keyboardImages[str];
            }
            
        }

    }


    /// <summary>
    /// Class responsable with loading into memory and provision of images corresponding with the 
    /// button or movement of the mouse
    /// </summary>
    public static class MouseImages
    {
        private static Image _moveDown;
        private static Image _moveDownLeft;
        private static Image _moveDownRight;
        private static Image _moveLeft;
        private static Image _moveRight;
        private static Image _moveUp;
        private static Image _moveUpLeft;
        private static Image _moveUpRight;
        private static Image _mouseButton1;
        private static Image _mouseButton2;
        private static Image _mouseButton4;
        private static Image _mouseButton5;
        private static Image _mouseScrollBackwards;
        private static Image _mouseScrollForward;
        private static Image _mouseScrollPress;
        private static string execDirectoryPath = Path.GetDirectoryName(Application.ExecutablePath);
        private static string _imagePath = "\\Icons\\Mouse\\";


        /// <summary>
        ///  Load into memory the images from the corresponding folder
        /// </summary>
        public static void Initialize()
        {

            _moveDown = Bitmap.FromFile(execDirectoryPath + _imagePath + "d.png");
            _moveUp = Bitmap.FromFile(execDirectoryPath + _imagePath + "u.png");
            _moveLeft = Bitmap.FromFile(execDirectoryPath + _imagePath + "l.png");
            _moveRight = Bitmap.FromFile(execDirectoryPath + _imagePath + "r.png");
            _moveDownLeft = Bitmap.FromFile(execDirectoryPath + _imagePath + "dl.png");
            _moveDownRight = Bitmap.FromFile(execDirectoryPath + _imagePath + "dr.png");
            _moveUpLeft = Bitmap.FromFile(execDirectoryPath + _imagePath + "ul.png");
            _moveUpRight = Bitmap.FromFile(execDirectoryPath + _imagePath + "ur.png");
            _mouseScrollPress = Bitmap.FromFile(execDirectoryPath + _imagePath + "mb3.png");
            _mouseButton1 = Bitmap.FromFile(execDirectoryPath + _imagePath + "mb1.png");
            _mouseButton2 = Bitmap.FromFile(execDirectoryPath + _imagePath + "mb2.png");
            _mouseButton4 = Bitmap.FromFile(execDirectoryPath + _imagePath + "mb4.png");
            _mouseButton5 = Bitmap.FromFile(execDirectoryPath + _imagePath + "mb5.png");
            _mouseScrollBackwards = Bitmap.FromFile(execDirectoryPath + _imagePath + "scroll_up.png");
            _mouseScrollForward = Bitmap.FromFile(execDirectoryPath + _imagePath + "scroll_down.png");
        }

        /// <summary>
        /// Retun an image corresponding to the <see cref="key"/>
        /// </summary>
        /// <param name="key"> string describing the needed image</param>
        /// <returns> an Image corresponding to the provided key</returns>
        public static Image GetImageForKey(string key)
        {
            switch (key)
            {
                case "Mouse button 1":
                    {
                        return _mouseButton1;
                    }
                case "Mouse button 2":
                    {
                        return _mouseButton2;
                    }
                case "Mouse button 4":
                    {
                        return _mouseButton4;
                    }
                case "Mouse button 5":
                    {
                        return _mouseButton5;
                    }
                case "Middle mouse button":
                    {
                        return _mouseScrollPress;
                    }
                case "Mouse wheel forward":
                    {
                        return _mouseScrollForward;
                    }
                case "Mouse wheel backwards":
                    {
                        return _mouseScrollBackwards;
                    }
                case "mouse moved down":
                    {
                        return _moveDown;
                    }
                case "mouse moved up":
                    {
                        return _moveUp;
                    }
                case "mouse moved left":
                    {
                        return _moveLeft;
                    }
                case "mouse moved right":
                    {
                        return _moveRight;
                    }
                case "mouse moved down left":
                    {
                        return _moveDownLeft;
                    }
                case "mouse moved down right":
                    {
                        return _moveDownRight;
                    }
                case "mouse moved up left":
                    {
                        return _moveUpLeft;
                    }
                case "mouse moved up right":
                    {
                        return _moveUpRight;
                    }
                default:
                    {
                        return SystemIcons.Error.ToBitmap();
                    }
            }
        }
    }

    /// <summary>
    /// Class responsable with loading into memory and provision of images corresponding with the 
    /// button press of the gamepad
    /// </summary>
    public static class GamepadImages
    {

        private static Image _leftTrigger;
        private static Image _rightTrigger;
        private static Image _leftShoulder;
        private static Image _rightShoulder;
        private static Image _leftThumbLeft;
        private static Image _leftThumbRight;
        private static Image _leftThumbDownright;
        private static Image _leftThumbDownLeft;
        private static Image _leftThumbUpLeft;
        private static Image _leftThumbUpright;
        private static Image _leftThumbDown;
        private static Image _leftThumbUp;
        private static Image _leftThumbPress;
        private static Image _dPadLeft;
        private static Image _dPadRight;
        private static Image _dPadUp;
        private static Image _dPadDown;
        private static Image _back;
        private static Image _main;
        private static Image _start;
        private static Image _rightThumbLeft;
        private static Image _rightThumbUp;
        private static Image _rightThumbRight;
        private static Image _rightThumbDown;
        private static Image _x;
        private static Image _y;
        private static Image _a;
        private static Image _b;
        private static Image _rightThumbPress;
        private static Image _rightThumbDownright;
        private static Image _rightThumbDownLeft;
        private static Image _rightThumbUpLeft;
        private static Image _rightThumbUpright;

        private static string _execDirectoryPath = Path.GetDirectoryName(Application.ExecutablePath);
        private static string _imagePath = "\\Icons\\Gamepad\\";

        /// <summary>
        ///  Load into memory the images from the corresponding folder
        /// </summary>
        public static void Initialize()
        {
            _leftTrigger = Bitmap.FromFile(_execDirectoryPath + _imagePath + "LeftTrigger.png");
            _rightTrigger = Bitmap.FromFile(_execDirectoryPath + _imagePath + "RightTrigger.png");
            
            _leftShoulder = Bitmap.FromFile(_execDirectoryPath + _imagePath + "LeftShoulder.png");
            _rightShoulder = Bitmap.FromFile(_execDirectoryPath + _imagePath + "RightShoulder.png");

            _leftThumbUp = Bitmap.FromFile(_execDirectoryPath + _imagePath + "LeftThumbUp.png");
            _leftThumbDown = Bitmap.FromFile(_execDirectoryPath + _imagePath + "LeftThumbDown.png");
            _leftThumbLeft = Bitmap.FromFile(_execDirectoryPath + _imagePath + "LeftThumbLeft.png");
            _leftThumbRight = Bitmap.FromFile(_execDirectoryPath + _imagePath + "LeftThumbRight.png");
            _leftThumbPress = Bitmap.FromFile(_execDirectoryPath + _imagePath + "LeftThumbPress.png");
            _leftThumbDownLeft = Bitmap.FromFile(_execDirectoryPath + _imagePath + "LeftThumbDownLeft.png");
            _leftThumbDownright = Bitmap.FromFile(_execDirectoryPath + _imagePath + "LeftThumbDownright.png");
            _leftThumbUpLeft = Bitmap.FromFile(_execDirectoryPath + _imagePath + "LeftThumbUpLeft.png");
            _leftThumbUpright = Bitmap.FromFile(_execDirectoryPath + _imagePath + "LeftThumbUpright.png");

            _rightThumbUp = Bitmap.FromFile(_execDirectoryPath + _imagePath + "RightThumbUp.png");
            _rightThumbDown = Bitmap.FromFile(_execDirectoryPath + _imagePath + "RightThumbDown.png");
            _rightThumbLeft = Bitmap.FromFile(_execDirectoryPath + _imagePath + "RightThumbLeft.png");
            _rightThumbRight = Bitmap.FromFile(_execDirectoryPath + _imagePath + "RightThumbRight.png");
            _rightThumbPress = Bitmap.FromFile(_execDirectoryPath + _imagePath + "RightThumbPress.png");
            _rightThumbDownLeft = Bitmap.FromFile(_execDirectoryPath + _imagePath + "RightThumbDownLeft.png");
            _rightThumbDownright = Bitmap.FromFile(_execDirectoryPath + _imagePath + "RightThumbDownright.png");
            _rightThumbUpLeft = Bitmap.FromFile(_execDirectoryPath + _imagePath + "RightThumbUpLeft.png");
            _rightThumbUpright = Bitmap.FromFile(_execDirectoryPath + _imagePath + "RightThumbUpright.png");
            
            _x = Bitmap.FromFile(_execDirectoryPath + _imagePath + "X.png");
            _y = Bitmap.FromFile(_execDirectoryPath + _imagePath + "Y.png");
            _a = Bitmap.FromFile(_execDirectoryPath + _imagePath + "A.png");
            _b = Bitmap.FromFile(_execDirectoryPath + _imagePath + "B.png");

            _dPadDown = Bitmap.FromFile(_execDirectoryPath + _imagePath + "DPadDown.png");
            _dPadLeft = Bitmap.FromFile(_execDirectoryPath + _imagePath + "DPadLeft.png");
            _dPadRight = Bitmap.FromFile(_execDirectoryPath + _imagePath + "DPadRight.png");
            _dPadUp = Bitmap.FromFile(_execDirectoryPath + _imagePath + "DPadUp.png");
            
            _start = Bitmap.FromFile(_execDirectoryPath + _imagePath + "Start.png");
            _back = Bitmap.FromFile(_execDirectoryPath + _imagePath + "Back.png");
            
            _main = Bitmap.FromFile(_execDirectoryPath + _imagePath + "Main.png");
        }


        /// <summary>
        /// Retun an image corresponding to the <see cref="key"/>
        /// </summary>
        /// <param name="key"> string describing the needed image</param>
        /// <returns> an Image corresponding to the provided key</returns>
        public static Image GetImageForKey(string key)
        {
            switch (key)
            {
                case "LeftTrigger":
                    return _leftTrigger;
                case "RightTrigger":
                    return _rightTrigger;
                case "LeftShoulder":
                    return _leftShoulder;
                case "RightShoulder":
                    return _rightShoulder;
                case "LeftThumbLeft":
                    return _leftThumbLeft;
                case "LeftThumbRight":
                    return _leftThumbRight;
                case "LeftThumbUp":
                    return _leftThumbUp;
                case "LeftThumbDown":
                    return _leftThumbDown;
                case "LeftThumbDownLeft":
                    return _leftThumbDownLeft;
                case "LeftThumbDownright":
                    return _leftThumbDownright;
                case "LeftThumbUpright":
                    return _leftThumbUpright;
                case "LeftThumbUpLeft":
                    return _leftThumbUpLeft;
                case "RightThumbUp":
                    return _rightThumbUp;
                case "RightThumbDown":
                    return _rightThumbDown;
                case "RightThumbLeft":
                    return _rightThumbLeft;
                case "RightThumbRight":
                    return _rightThumbRight;
                case "RightThumbDownleft":
                    return _rightThumbDownLeft;
                case "RightThumbDownRight":
                    return _rightThumbDownright;
                case "RightThumbUpleft":
                    return _rightThumbUpLeft;
                case "RightThumbUpRight":
                    return _rightThumbUpright;
                case "LeftThumbPress":
                    return _leftThumbPress;
                case "RightThumbPress":
                    return _rightThumbPress;
                case "gamepadX":
                    return _x;
                case "gamepadY":
                    return _y;
                case "gamepadA":
                    return _a;
                case "gamepadB":
                    return _b;
                case "Start":
                    return _start;
                case "Back":
                    return _back;
                case "DPadUp":
                    return _dPadUp;
                case "DPadLeft":
                    return _dPadLeft;
                case "DPadRight":
                    return _dPadRight;
                case "DPadDown":
                    return _dPadDown;
                case "LButton, XButton2":
                    return _main;
                default:
                    {
                        return SystemIcons.Error.ToBitmap();
                    }
            }
        }
    }
}
