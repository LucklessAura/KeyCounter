﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using IWshRuntimeLibrary;

namespace KeyCounter
{
    /// <summary>
    /// Class representing the main form of the app
    /// </summary>
    public partial class MainForm : Form
    {
        private KeyboardHookClass _keyboard = new KeyboardHookClass();

        private MouseHookClass _mouse = new MouseHookClass();

        private GamepadClass _gamepad = new GamepadClass();

        public static Profile CurrentProfile = new Profile();

        private ImageList _imageList = new ImageList();

        private Options _options = new Options();

        private string _execDirectoryPath = Path.GetDirectoryName(Application.ExecutablePath);

        private DictionaryWithEvents _currentSelectedDictionary = null;

        private DateTime _profileStartDate;

        private DateTime _profileStopDate;

        /// <summary>
        /// initialize components and proprieties of the class
        /// </summary>
        public MainForm()
        {
            
            InitializeComponent();

            InitialSetUp();

        }

        /// <summary>
        /// Continuously show a new <c>NewProfileForm</c> until at least one valid <c>Profile</c> exists in the <c> Options Profile List</c>
        /// </summary>
        private void ForceCreateProfile()
        {
            while (_options.ProfilesList.Count == 0)
            {
                //Create a new NewProfileForm with the current options and a DialogResult corresponding to the result of the 
                // created from
                NewProfileForm profileForm = new NewProfileForm(_options);

                DialogResult formResult = profileForm.ShowDialog();

                // if the result of the form is DialogResult.Yes try to create a new Profile
                // with the given parameters
                if (formResult == DialogResult.Yes)
                {
                    ProfileManager.CreateProfile(profileForm.ProfileName, profileForm.NeedsGamepad);

                    // add the new profile to the profile list of the current options
                    _options.ProfilesList.Add(profileForm.ProfileName);

                    profileComboBox.DataSource = new BindingSource { DataSource = _options.ProfilesList };

                    //try to open the newly created profile and call profileComboBox_SelectedIndexChanged
                    //if it succeds else if the corresponding file is not found delete the profile from 
                    // the current options profile list 
                    try
                    {
                        CurrentProfile = ProfileManager.SelectProfile(profileForm.ProfileName);

                        profileComboBox.SelectedIndex = 0;

                        profileComboBox_SelectedIndexChanged(null, null);

                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("The file corresponding to " + profileForm.ProfileName + " was not found, it will now be removed from memory.");

                        _options.ProfilesList.Remove(profileForm.ProfileName);
                    }

                    profileForm.Dispose();
                }
            }
        }

        /// <summary>
        /// Set up parameters, read needed files and initialize proprieties
        /// </summary>
        /// <exception cref="ArgumentException"> thrown when no profile could be selected or created</exception>
        private void InitialSetUp()
        {

            // initialize the image classes for all supported input devices
            KeyboardImages.Initialize();
            MouseImages.Initialize();
            GamepadImages.Initialize();

            //read the options.cfg or create a new one if it does not exist
            _options.ReadOrCreateOptionsFile();

            //give to the profile manager the location of the Profiles folder on disk
            ProfileManager.ProfilesFolder = _options.ProfilesLocation;

            //initialize start date for measuring how much time a profile was used 
            _profileStartDate = DateTime.UtcNow;

            // if the number of profiles is 0 force the creation of one
            ForceCreateProfile();

            //if the CurrentProfile is not initialized try to initialize it according to the current settings
            if (CurrentProfile.Name == null)
            {
                profileComboBox.DataSource = new BindingSource { DataSource = _options.ProfilesList };

                //if the option UseLastProfile is true try setting the CurrentProfile to the last used
                // profile before the app closing 
                if (_options.LastSelectedProfile != "" && _options.UseLastProfile == true)
                {
                    //try opening the LastSelectedProfile profile and call profileComboBox_SelectedIndexChanged
                    //if it succeds else if it does not succed open the first profile in the 
                    // current options profile list, if the current options profile list is empty force
                    // the creation of a new profile
                    try
                    {
                        CurrentProfile = ProfileManager.SelectProfile(_options.LastSelectedProfile);

                        profileComboBox.SelectedItem = CurrentProfile.Name;

                        profileComboBox_SelectedIndexChanged(null, null);
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("The file corresponding to " + _options.LastSelectedProfile + " was not found, it will now be removed from memory.");

                        _options.ProfilesList.Remove(_options.LastSelectedProfile);

                        if (_options.ProfilesList.Count == 0)
                        {
                            ForceCreateProfile();
                        }
                        else
                        {
                            profileComboBox.SelectedIndex = 0;

                            CurrentProfile = ProfileManager.SelectProfile(profileComboBox.Text);

                            profileComboBox_SelectedIndexChanged(null, null);
                        }
                    }
                    
                }
                // if the option UseLastProfile is false and there is a OnStartProfile in the current options
                //try to open it 
                else if (_options.UseLastProfile == false && _options.OnStartProfile != "")
                {
                    //try setting the CurrentProfile to the OnStartProfile and call profileComboBox_SelectedIndexChanged
                    //if it succeds else if this fails try to set the CurrentProfile 
                    //to the first element in the options profiles list, if the list is empty force the creation of a
                    // new valid profile
                    try
                    {
                        CurrentProfile = ProfileManager.SelectProfile(_options.OnStartProfile);
                        
                        profileComboBox.Text = CurrentProfile.Name;

                        profileComboBox_SelectedIndexChanged(null, null);
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("3 The file corresponding to " + _options.OnStartProfile + " was not found, it will now be removed from memory.");

                        _options.ProfilesList.Remove(_options.OnStartProfile);
                        if (_options.ProfilesList.Count == 0)
                        {
                            ForceCreateProfile();
                        }
                        else
                        {
                            profileComboBox.SelectedIndex = 0;

                            CurrentProfile = ProfileManager.SelectProfile(profileComboBox.Text);

                            profileComboBox_SelectedIndexChanged(null, null);
                        }
                    }

                    
                }
                // throw an exception if no profile could be selected or created
                else
                {
                    throw new ArgumentException("Could not select a profile");
                }

            }
            
            // set the tool strip for the taskbar icon with the current list of profiles
            ToolStripMenuItem[] toolStripMenuItems = new ToolStripMenuItem[_options.ProfilesList.Count];
            
            for (int i = 0; i < _options.ProfilesList.Count; i++)
            {
                toolStripMenuItems[i] = new ToolStripMenuItem(_options.ProfilesList[i], null,null, _options.ProfilesList[i]);
            }
            
            profilesContextMenu.Items.AddRange(toolStripMenuItems);
            
            taskBarIcon.ContextMenuStrip = profilesContextMenu;

            // set up the keyboard and mouse hooks
            _keyboard.Initialize();
            _mouse.Initialize();

            // set up the handlers for the gamepad events related to the connection to the computer
            _gamepad.OnGamepadFoundStatus += () => { MessageBox.Show("Gamepad Found"); };
            _gamepad.OnGamepadDisconnectStatus += () => { MessageBox.Show("Gamepad disconnected"); };
            _gamepad.OnNoGamepadFoundStatus += () => { MessageBox.Show("No gamepad connected found"); };

            _imageList.ImageSize = new Size(150, 80);

            keysListView.LargeImageList = _imageList;
        }

        /// <summary>
        /// When the form is resized verify if it was minimised, if it was hide the form from view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Form1_Resize(object sender, EventArgs e)
        {
            
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
            }

        }

        /// <summary>
        /// Set the form to visible when the icon in the taskbar is Double Clicked 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Visible == false)
            {
                Show();
            }
            WindowState = FormWindowState.Normal;

        }

        /// <summary>
        /// Save the current profile, options, remove keyboard and mouse hooks, stop the gamepad operations and dispose of the 
        /// taskbar icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //register the current date and calculate how may hours( with two fractional digits) has the profile been used for
            //and add it to it's count so far 
            _profileStopDate = DateTime.UtcNow;
            CurrentProfile.TimeUsed += (float)Math.Round((_profileStopDate - _profileStartDate).TotalHours, 2);

            ProfileManager.SaveProfile(CurrentProfile);

            _options.SaveOptionsToFile();

            _keyboard.DeleteKeyboardHook();
            _mouse.DeleteMouseHook();
            _gamepad.Stop();

            taskBarIcon.Dispose();
        }


        /// <summary>
        /// Save the <c>CurrentProfile</c>, select the new one according to the selected one in the <c>profileComboBox</c>  
        /// , set up the handlers for the newly selected profile and start/ stop gamepad handling according to the profiles settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (profileComboBox.Text != CurrentProfile.Name || collectionsComboBox.DataSource == null)
            {
                //register the current date and calculate how may hours( with two fractional digits) has the profile been used for
                //and add it to it's count so far and save the profile
                _profileStopDate = DateTime.UtcNow;

                CurrentProfile.TimeUsed += (float)Math.Round((_profileStopDate - _profileStartDate).TotalHours,2);

                ProfileManager.SaveProfile(CurrentProfile);

                //try setting the CurrentProfile according to the value of the profileComboBox 
                // if this fails try to set the CurrentProfile 
                // to the first element in the options profiles list, if the list is empty force the creation of a
                // new valid profile
                try
                {
                    CurrentProfile = ProfileManager.SelectProfile(profileComboBox.SelectedItem.ToString());
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("4 The file corresponding to " + profileComboBox.SelectedItem.ToString() + " was not found, it will now be removed from memory.");

                    _options.ProfilesList.Remove(profileComboBox.Text);
                    if (_options.ProfilesList.Count == 0)
                    {
                        ForceCreateProfile();
                    }
                    else
                    {
                        profileComboBox.SelectedIndex = 0;

                        CurrentProfile = ProfileManager.SelectProfile(profileComboBox.Text);

                    }
                }

                // set the start date of this profile to the closing one of the last one
                _profileStartDate = _profileStopDate;

                //give the new values for the CurrentProfile, list view and image list to the class
                // handling the dictionary events setup 
                DictionaryWithEventsHandlers.SetInternals(CurrentProfile, _imageList, keysListView);

                //set up the handlsers for the dictionaries of the newly selected profile
                DictionaryWithEventsHandlers.SetUpHandlersForCurrentProfile();

                // if needed start the gamepad connection and handling processes
                if (!CurrentProfile.NeedsGamepad)
                {
                    _gamepad.Stop();
                }
                else
                {
                    _gamepad.Start();
                }


                collectionsComboBox.DataSource = new BindingSource { DataSource = CurrentProfile.TypesOfInputList };

                _options.LastSelectedProfile = CurrentProfile.Name;

                profileComboBox.SelectedItem = CurrentProfile.Name;
            }
            

            this.ActiveControl = null;
        }

        /// <summary>
        /// Clear the list view in the main form, enable events for the dictionary that is selected in the 
        /// <c>collectionsComboBox</c> adn load the images and count for the current dictionary in the list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentException"> Thrown when the value of <c>collectionsComboBox.SelectedItem</c> 
        /// is not keyboard/ mouse/ gamepad/ total </exception>
        private void collectionsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = null;

            // disable events for all dictionaries
            CurrentProfile.KeyboardKeys.DisableEvents();
            CurrentProfile.MouseKeys.DisableEvents();
            CurrentProfile.GamepadKeys.DisableEvents();

            //check which dictionary needs to have its events enabled, clear the list view and load the images and counts
            // for the corresponding dictionary
            switch (collectionsComboBox.SelectedItem.ToString())
            {
                case "Keyboard":
                    {

                        keysListView.Items.Clear();
                        
                        _currentSelectedDictionary = CurrentProfile.KeyboardKeys;
                        _currentSelectedDictionary.EnableEvents();
                        _currentSelectedDictionary.InitialLoad();
                        break;
                    }
                case "Mouse":
                    {
                        keysListView.Items.Clear();
                        
                        _currentSelectedDictionary = CurrentProfile.MouseKeys;
                        _currentSelectedDictionary.EnableEvents();
                        CurrentProfile.MouseKeys.InitialLoad();
                        break;
                    }
                case "Gamepad":
                    {
                        keysListView.Items.Clear();
                        
                        _currentSelectedDictionary = CurrentProfile.GamepadKeys;
                        _currentSelectedDictionary.EnableEvents();
                        CurrentProfile.GamepadKeys.InitialLoad();
                        break;
                    }
                case "Total":
                    {

                        keysListView.Items.Clear();
                        CurrentProfile.KeyboardKeys.EnableEvents();
                        CurrentProfile.MouseKeys.EnableEvents();
                        CurrentProfile.GamepadKeys.EnableEvents();

                        
                        _imageList.Images.Clear();

                        foreach (KeyValuePair<string, CustomPair> item in CurrentProfile.TotalKeys)
                        {
                            _imageList.Images.Add(item.Key, item.Value.Image);
                            keysListView.Items.Add(item.Key, item.Value.Number.ToString(), item.Key);
                        }
                        break;
                    }
               default:
                    {
                        throw new ArgumentException("Unknown collection");

                    }
            }

        }

        /// <summary>
        /// Createa a new OptionsForm and based on its result change the current options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsButton_Click(object sender, EventArgs e)
        {
            OptionsForm optionsForm = new OptionsForm(_options);
            DialogResult formResult = optionsForm.ShowDialog();
            //if the user selected "reset to default" reset options to their default value
            // but don't delete the Profiles
            if (formResult == DialogResult.No)
            {
                if (_options.ProfilesLocation != _execDirectoryPath + "\\" + "Profiles")
                {
                    ProfileManager.MoveProfiles(_execDirectoryPath);
                    ProfileManager.ProfilesFolder = _execDirectoryPath + "\\" + "Profiles";

                }
                _options.ResetToDefaultOptions(false);

            }
            // if the user selected to save the settings
            else if(formResult == DialogResult.Yes)
            {
                // if the user wanted to change the location of the Profiles folder move the folder to the new location
                if (optionsForm.ProfilesLocation != _options.ProfilesLocation)
                {
                    ProfileManager.MoveProfiles(optionsForm.ProfilesLocation);
                    ProfileManager.ProfilesFolder = optionsForm.ProfilesLocation + "\\" + "Profiles";
                    _options.ProfilesLocation = optionsForm.ProfilesLocation + "\\" + "Profiles";
                }
                // retrive the other option set by the user
                _options.OnStartProfile = optionsForm.OnStartProfile;
                _options.UseLastProfile = optionsForm.UseLastProfile;
                _options.StartMinimised = optionsForm.StartMinimised;

                // if autostart is set to true put a shortcut of the app in the Startup folder of the current computer
                // else remove the shortcut from the Startup folder
                if (_options.AutoStart != optionsForm.StartWithWindows)
                {
                    _options.AutoStart = optionsForm.StartWithWindows;
                    if (_options.AutoStart == true)
                    { 
                        Console.WriteLine(Application.ExecutablePath.Replace(".dll", ".exe"));
                        var startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                        WshShellClass shell = new WshShellClass();
                        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(startupFolder + "\\" + "KeyCounter.lnk");
                        shortcut.TargetPath = Application.ExecutablePath.Replace(".dll", ".exe");
                        shortcut.Description = "KeyCounter shortcut";
                        shortcut.Save();

                    }
                    else
                    {
                        var startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                        if (System.IO.File.Exists(startupFolder + "\\" + "KeyCounter.lnk"))
                        {
                            System.IO.File.Delete(startupFolder + "\\" + "KeyCounter.lnk");
                        }
                    }
                }
                

            }
        }

        
        /// <summary>
        /// Show a message box with a warning, if the user selects to reset the profile clear all dictionaries and reset timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearProfileButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This will reset all buttons counts for the current profile","Clear counts", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                CurrentProfile.GamepadKeys.Clear();
                CurrentProfile.KeyboardKeys.Clear();
                CurrentProfile.MouseKeys.Clear();
                CurrentProfile.TotalKeys.Clear();
                CurrentProfile.TimeUsed = 0.00f;
            }
        }


        /// <summary>
        /// Create a <c>newProfileForm</c>, if the user clicks the create button create a new profile with the given parameters
        /// , change <c>profileComboBox.SelectedItem</c>, and call profileComboBox_SelectedIndexChanged so the created profile 
        /// is set as the CurrentProfile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newProfileButton_Click(object sender, EventArgs e)
        {
            NewProfileForm @newProfileForm = new NewProfileForm(_options);
            DialogResult formResult = @newProfileForm.ShowDialog();
            if (formResult == DialogResult.Yes)
            {
                ProfileManager.CreateProfile(@newProfileForm.ProfileName, @newProfileForm.NeedsGamepad);
                _options.ProfilesList.Add(@newProfileForm.ProfileName);
                taskBarIcon.ContextMenuStrip.Items.Add(@newProfileForm.ProfileName);
                profileComboBox.SelectedItem = @newProfileForm.ProfileName;
                profileComboBox_SelectedIndexChanged(null, null);
            }
            @newProfileForm.Dispose();
        }

        /// <summary>
        /// Try to delete the CurrentProfile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteProfileButton_Click(object sender, EventArgs e)
        {
            // if the CurrentProfile is the only existing one show a MessageBox telling the user that the last profile cannot 
            // be deleted 
            if (_options.ProfilesList.Count == 1)
            {
                MessageBox.Show("You cannot delete the last profile");
            }
            // else show a MessageBox asking the user if it is sure that it wants to delete the profile
            else
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete the current profile?", "Delete Profile", MessageBoxButtons.YesNo);
                // if the user selects Delete then the CurrentProfile is deleted and the first profile in the 
                // profiles list becomes the CurrentProfile
                if (result == DialogResult.Yes)
                {
                    taskBarIcon.ContextMenuStrip.Items.RemoveByKey(CurrentProfile.Name);
                    ProfileManager.DeleteProfile(CurrentProfile, _options);
                    profileComboBox.SelectedIndex = 0;
                    profileComboBox_SelectedIndexChanged(null, null);
                }
            }
            
        }

        /// <summary>
        /// The firt time the form is shown hide it if the option to start minimised is set to true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (_options.StartMinimised == true)
            {
                this.Hide();
            }
        }

        /// <summary>
        /// When a item in the context menu of the icon in the taskbar is pressed change the CurrentProfile to that 
        /// item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profilesContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            profileComboBox.Text = e.ClickedItem.Text;
            profileComboBox_SelectedIndexChanged(null, null);
        }
    }
}