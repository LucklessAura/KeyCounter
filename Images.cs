using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;



namespace KeyCounter
{
    /// <summary>
    /// Class responsible with loading into memory, creation and provision of images corresponding with the 
    /// keys of the keyboard
    /// </summary>
    internal static class KeyboardImages
    {
        private static readonly string _execDirectoryPath = Path.GetDirectoryName(Application.ExecutablePath);
        private static readonly FontFamily _fontFamily = new FontFamily("Candara");
        private const string IMAGES_PATH = "\\Icons\\Keyboard\\";
        private static Font _font;
        private static Image _tempImage;
        private static Dictionary<string, Image> _keyboardImages;


        /// <summary>
        /// initialize dictionary with all images in the corresponding folder
        /// </summary>
        public static void Initialize(ImageList imageList)
        {
            if (_keyboardImages == null)
            {
                _keyboardImages = new Dictionary<string, Image>();
                foreach (string image in Directory.EnumerateFiles(_execDirectoryPath + IMAGES_PATH))
                {
                    if (image.EndsWith(".png"))
                    {
                        Image img = Image.FromFile(image);
                        _keyboardImages.Add(Path.GetFileNameWithoutExtension(image), img);
                        imageList.Images.Add(Path.GetFileNameWithoutExtension(image), img);
                    }
                }
            }
            
        }

        /// <summary>
        /// Return an image for the <paramref name="key"/>, the image is created and saved in the corresponding folder if
        /// it does not already exist
        /// </summary>
        /// <param name="key">string for which an image is created/ returned </param>
        /// <returns>an Image corresponding to the provided key</returns>
        public static Image GetImageForKey(string key)
        {
            if (_keyboardImages == null)
            {
                return null;
            }
            //verify if the image is in the dictionary, if yes return it
            if (_keyboardImages.ContainsKey(key))
            {
                return _keyboardImages[key];
            }
            // else create it using the base_key or base_key_long
            else
            {
                //if the length of key is <=2 use base_key else base_key_long
                if (key.Length > 2)
                { 
                    _tempImage = (Image)_keyboardImages["base_key_long"].Clone();
                }
                else
                {
                    _tempImage = (Image)_keyboardImages["base_key"].Clone();
                }
                using (Graphics g = Graphics.FromImage(_tempImage))
                {
                    int size = 150;
                    _font = new Font(_fontFamily, size);
                    // keep making the font smaller until it fits vertically and horizontally on the white space of the key image
                    while (g.MeasureString(key, _font).Width > _tempImage.Width || g.MeasureString(key, _font).Height > _tempImage.Height)
                    {
                        _font.Dispose();
                        size -= 5;
                        _font = new Font(_fontFamily, size);
                    }

                    // draw on the image the key
                    g.DrawString(key, _font, Brushes.Gray, new Point((int)((_tempImage.Width - g.MeasureString(key, _font).Width) / 2), (int)((_tempImage.Height - g.MeasureString(key, _font).Height) / 2)));

                }
                _keyboardImages.Add(key, _tempImage);
                _tempImage.Save(_execDirectoryPath + IMAGES_PATH + key + ".png", ImageFormat.Png);
                _font.Dispose();
                return _keyboardImages[key];
            }
            
        }

        public static void UnloadImages(ImageList imageList)
        {
            if (_keyboardImages != null && _keyboardImages.Count > 0)
            {

                foreach (KeyValuePair<string, Image> pair in _keyboardImages)
                {
                    pair.Value.Dispose();
                }

                foreach (Image img in imageList.Images)
                {
                    img.Dispose();
                }

                _keyboardImages = null;
            }
        }
    }


    /// <summary>
    /// Class responsible with loading into memory and provision of images corresponding with the 
    /// button or movement of the mouse
    /// </summary>
    [SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
    public static class MouseImages
    {

        private static readonly string _execDirectoryPath = Path.GetDirectoryName(Application.ExecutablePath);
        private const string IMAGES_PATH = "\\Icons\\Mouse\\";
        private static Dictionary<string, Image> _mouseImages;
        private static readonly FontFamily _fontFamily = new FontFamily("Candara");
        private static Font _font;


        /// <summary>
        ///  Load into memory the images from the corresponding folder
        /// </summary>
        public static void Initialize(ImageList imageList)
        {
            if (_mouseImages == null)
            {
                _mouseImages = new Dictionary<string, Image>();
                foreach (string image in Directory.EnumerateFiles(_execDirectoryPath + IMAGES_PATH))
                {
                    if (image.EndsWith(".png"))
                    {
                        Image img = Image.FromFile(image);
                        _mouseImages.Add(Path.GetFileNameWithoutExtension(image), img);
                        imageList.Images.Add(Path.GetFileNameWithoutExtension(image), img);
                    }
                }
            }
           
        }

        /// <summary>
        /// Return an image corresponding to the <paramref name="key" />
        /// </summary>
        /// <param name="key"> string describing the needed image</param>
        /// <returns> an Image corresponding to the provided key</returns>
        public static Image GetImageForKey(string key)
        {
            if (_mouseImages == null)
            {
                return null;
            }
            if (_mouseImages.ContainsKey(key))
            {
                return _mouseImages[key];
            }
            else
            {
                Image tempImage = (Image) _mouseImages["empty button"].Clone();
                
                using (Graphics g = Graphics.FromImage(tempImage))
                {
                    int size = 150;
                    _font = new Font(_fontFamily, size);
                    // keep making the font smaller until it fits vertically and horizontally on the white space of the key image
                    while (g.MeasureString(key, _font).Width > tempImage.Width || g.MeasureString(key, _font).Height > 190)
                    {
                        _font.Dispose();
                        size -= 5;
                        _font = new Font(_fontFamily, size);
                    }
                    g.DrawString(key, _font, Brushes.Gray, new Point((int)((tempImage.Width - g.MeasureString(key, _font).Width) / 2), (int)((tempImage.Height - g.MeasureString(key, _font).Height) )));


                    _mouseImages.Add(key, tempImage);
                    tempImage.Save(_execDirectoryPath + IMAGES_PATH + key + ".png", ImageFormat.Png);
                    _font.Dispose();
                    return _mouseImages[key];
                }
            }
        }

        public static void UnloadImages(ImageList imageList)
        {

            if (_mouseImages != null && _mouseImages.Count > 0)
            {
                foreach (KeyValuePair<string, Image> pair in _mouseImages)
                {
                    pair.Value.Dispose();
                }

                foreach (Image image in imageList.Images)
                {
                    image.Dispose();
                }

                _mouseImages = null;
            }
        }
    }

    /// <summary>
    /// Class responsible with loading into memory and provision of images corresponding with the 
    /// button press of the gamepad
    /// </summary>
    [SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
    public static class GamepadImages
    {

        private static readonly string _execDirectoryPath = Path.GetDirectoryName(Application.ExecutablePath);
        private const string IMAGES_PATH = "\\Icons\\Gamepad\\";
        private static Dictionary<string, Image> _gamepadImages;
        private static readonly FontFamily _fontFamily = new FontFamily("Candara");
        private static Font _font;

        /// <summary>
        ///  Load into memory the images from the corresponding folder
        /// </summary>
        public static void Initialize(ImageList imageList)
        {
            if (_gamepadImages == null)
            {
                _gamepadImages = new Dictionary<string, Image>();
                foreach (string image in Directory.EnumerateFiles(_execDirectoryPath + IMAGES_PATH))
                {
                    if (image.EndsWith(".png"))
                    {
                        Image img = Image.FromFile(image);
                        _gamepadImages.Add(Path.GetFileNameWithoutExtension(image), img);
                        imageList.Images.Add(Path.GetFileNameWithoutExtension(image), img);
                    }
                }
            }
            
        }


        /// <summary>
        /// Return an image corresponding to the <see cref="key"/>
        /// </summary>
        /// <param name="key"> string describing the needed image</param>
        /// <returns> an Image corresponding to the provided key</returns>
        public static Image GetImageForKey(string key)
        {
            if (_gamepadImages == null)
            {
                return null;
            }
            if (_gamepadImages.ContainsKey(key))
            {
                return _gamepadImages[key];
            }
            else
            {
                Image tempImage = (Image)_gamepadImages["empty button"].Clone();

                using (Graphics g = Graphics.FromImage(tempImage))
                {
                    string newKey = key.Replace(" ", "\n");
                    int size = 150;
                    _font = new Font(_fontFamily, size);
                    while (g.MeasureString(key, _font).Width > 280 || g.MeasureString(newKey, _font).Height > tempImage.Height)
                    {
                        _font.Dispose();
                        size -= 5;
                        _font = new Font(_fontFamily, size);
                    }
                    // draw on the image the key
                    g.DrawString(newKey, _font, Brushes.LightGray, new Point((int)((tempImage.Width - g.MeasureString(newKey, _font).Width) / 2), (int)((tempImage.Height - g.MeasureString(newKey, _font).Height) / 2)));


                    _gamepadImages.Add(key, tempImage);

                    tempImage.Save(_execDirectoryPath + IMAGES_PATH + key + ".png", ImageFormat.Png);
                    _font.Dispose();
                    return _gamepadImages[key];
                }
            }
        }

        public static void UnloadImages(ImageList imageList)
        {
            if (_gamepadImages != null && _gamepadImages.Count > 0)
            {

                foreach (KeyValuePair<string, Image> pair in _gamepadImages)
                {
                    pair.Value.Dispose();
                }

                foreach (Image image in imageList.Images)
                {
                    image.Dispose();
                }

                _gamepadImages = null;
            }
        }
    }
}
