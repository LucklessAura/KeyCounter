using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyCounter
{
    /// <summary>
    /// Class that provides fast handler setup for profile dictionary events
    /// </summary>
    public static class DictionaryWithEventsHandlers
    {

        private static Profile _currentProfile;
        private static ImageList _imageList;
        private static ListView _keysListView;
        private static TextBox _timeTextBox;
        public delegate void UpdateTimeDelegate(Control control, Profile profile);

        /// <summary>
        /// Sets up the profile, image list ans list view with which it should interact
        /// </summary>
        /// <param name="profile">profile for which the events should be set up, usually the CurrentProfile</param>
        /// <param name="imageList">image list where the images for each key should be added</param>
        /// <param name="listView">list view for which elements should be added</param>
        /// <param name="timeTextBox">text box where the time should be displayed</param>
        public static void SetInternals(Profile profile, ImageList imageList, ListView listView, TextBox timeTextBox)
        {
            _currentProfile = profile;
            _imageList = imageList;
            _keysListView = listView;
            _timeTextBox = timeTextBox;
        }

        /// <summary>
        /// Call the setup for the mouse, keyboard and mouse
        /// </summary>
        public static void SetUpHandlersForCurrentProfile()
        {
            MouseSetUp();
            KeyboardSetUp();
            GamepadSetUp();
        }

        /// <summary>
        /// give to the keyboard dictionary the handlers
        /// </summary>
        private static void KeyboardSetUp()
        {
            _currentProfile.KeyboardKeys.OnInitialLoadStatus += HandleKeyboardInitialLoad;
            _currentProfile.KeyboardKeys.OnAddStatus += HandleNewKeyboardKey;
            _currentProfile.KeyboardKeys.OnUpdateStatus += HandleKeyboardKeyPress;

        }

        /// <summary>
        /// give to the mouse dictionary the handlers
        /// </summary>
        private static void MouseSetUp()
        {
            _currentProfile.MouseKeys.OnInitialLoadStatus += HandleMouseInitialLoad;
            _currentProfile.MouseKeys.OnAddStatus += HandleNewMouseKey;
            _currentProfile.MouseKeys.OnUpdateStatus += HandleMouseKeyPress;
        }

        /// <summary>
        /// give to the gamepad dictionary the handlers
        /// </summary>
        private static void GamepadSetUp()
        {
            _currentProfile.GamepadKeys.OnInitialLoadStatus += HandleGamepadInitialLoad;
            _currentProfile.GamepadKeys.OnAddStatus += HandleNewGamepadKey;
            _currentProfile.GamepadKeys.OnUpdateStatus += HandleGamepadKeyPress;
        }

        /// <summary>
        /// Handler for a key that has been already added to the dictionary
        /// </summary>
        private static void HandleGamepadKeyPress()
        {
            // add 1 to the count of the corresponding element, in the case of this app the text of the list element
            string key = _currentProfile.GamepadKeys.GetLastUpdatedKey();
            ListViewItem[] foundIndex = _keysListView.Items.Find(key, false);
            
            for (int i = 0; i < foundIndex.Length; i++)
            {
                foundIndex[i].Text = (int.Parse(foundIndex[i].Text) + 1).ToString();
            }

            UpdateTimeInvoker(_timeTextBox,_currentProfile);

            
        }

        public static void UpdateTimeInvoker(Control control, Profile profile)
        {
            if (_timeTextBox.InvokeRequired)
            {
                _timeTextBox.Invoke(new UpdateTimeDelegate(UpdateTimeInvoker), new object[] { control, profile});
            }
            else
            {
                TimeSpan used = TimeSpan.FromHours(_currentProfile.TimeUsed + (float) Math.Round((DateTime.UtcNow - MainForm.ProfileStartDate).TotalHours, 2));
                control.Text = $"{used.Days} days, {used.Hours} hours, {used.Minutes} minutes";
            }
        }


        /// <summary>
        /// Handler for a key that is new
        /// </summary>
        private static void HandleNewGamepadKey()
        {
            string key = _currentProfile.GamepadKeys.GetLastAddedKey();
            // add to the image list the last element added in the dictionary
            _imageList.Images.Add(key, GamepadImages.GetImageForKey(key));
            //add to the list view the element with the count = 1
            _keysListView.Items.Add(key, 1.ToString(), key);


            UpdateTimeInvoker(_timeTextBox, _currentProfile);
        }

        /// <summary>
        /// Used when the collection of the list view should be changed to the gamepad one
        /// </summary>
        private static void HandleGamepadInitialLoad()
        {
            //clear the image list and list view
            _imageList.Images.Clear();
            _keysListView.Items.Clear();

            // add all the values in the dictionary to the list view and image view respectively
            foreach (KeyValuePair<string, CustomPair> item in _currentProfile.GamepadKeys)
            {
                _imageList.Images.Add(item.Key, item.Value.Image);
                _keysListView.Items.Add(item.Key, item.Value.Number.ToString(), item.Key);
            }


            UpdateTimeInvoker(_timeTextBox, _currentProfile);
        }


        /// <summary>
        /// Handler for a key that has been already added to the dictionary
        /// </summary>
        private static void HandleMouseKeyPress()
        {
            // add 1 to the count of the corresponding element, in the case of this app the text of the list element

            string key = _currentProfile.MouseKeys.GetLastUpdatedKey();
            ListViewItem[] foundIndex = _keysListView.Items.Find(key, false);
            
            for (int i = 0; i < foundIndex.Length; i++)
            {
                foundIndex[i].Text = (int.Parse(foundIndex[i].Text) + 1).ToString();
            }


            UpdateTimeInvoker(_timeTextBox, _currentProfile);
        }


        /// <summary>
        /// Handler for a key that is new
        /// </summary>
        private static void HandleNewMouseKey()
        {

            string key = _currentProfile.MouseKeys.GetLastAddedKey();
            // add to the image list the last element added in the dictionary
            _imageList.Images.Add(key, MouseImages.GetImageForKey(key));
            //add to the list view the element with the count = 1
            _keysListView.Items.Add(key, 1.ToString(), key);


            UpdateTimeInvoker(_timeTextBox, _currentProfile);
        }

        /// <summary>
        /// Used when the collection of the list view should be changed to the mouse one
        /// </summary>
        private static void HandleMouseInitialLoad()
        {
            _imageList.Images.Clear();
            _keysListView.Items.Clear();

            // add all the values in the dictionary to the list view and image view respectively
            foreach (KeyValuePair<string, CustomPair> item in _currentProfile.MouseKeys)
            {

                _imageList.Images.Add(item.Key, item.Value.Image);
                _keysListView.Items.Add(item.Key, item.Value.Number.ToString(), item.Key);
            }


            UpdateTimeInvoker(_timeTextBox, _currentProfile);
        }


        /// <summary>
        /// Used when the collection of the list view should be changed to the keyboard one
        /// </summary>
        private static void HandleKeyboardInitialLoad()
        {
            _imageList.Images.Clear();
            _keysListView.Items.Clear();

            // add all the values in the dictionary to the list view and image view respectively
            foreach (KeyValuePair<string, CustomPair> item in _currentProfile.KeyboardKeys)
            {
                _imageList.Images.Add(item.Key, item.Value.Image);
                _keysListView.Items.Add(item.Key, item.Value.Number.ToString(), item.Key);
            }

            UpdateTimeInvoker(_timeTextBox, _currentProfile);
        }


        /// <summary>
        /// Handler for a key that is new
        /// </summary>
        private static void HandleNewKeyboardKey()
        {
            string key = _currentProfile.KeyboardKeys.GetLastAddedKey();
            // add to the image list the last element added in the dictionary
            _imageList.Images.Add(key, KeyboardImages.GetImageForKey(key));
            //add to the list view the element with the count = 1

            _keysListView.Items.Add(key, 1.ToString(), key);

            UpdateTimeInvoker(_timeTextBox, _currentProfile);
        }

        /// <summary>
        /// Handler for a key that has been already added to the dictionary
        /// </summary>
        private static void HandleKeyboardKeyPress()
        {
            // add 1 to the count of the corresponding element, in the case of this app the text of the list element

            string key = _currentProfile.KeyboardKeys.GetLastUpdatedKey();
            ListViewItem[] foundIndex = _keysListView.Items.Find(key, false);
            
            for (int i = 0; i < foundIndex.Length; i++)
            {
                foundIndex[i].Text = (int.Parse(foundIndex[i].Text) + 1).ToString();
            }

            UpdateTimeInvoker(_timeTextBox, _currentProfile);
        }
    }
}
