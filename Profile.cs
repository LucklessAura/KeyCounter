using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KeyCounter
{
    static class ProfileManager
    {
        private static Profile? _currentProfile;
        public static BaseDevice CurrentDevice { get; private set; }


        /// <summary>
        /// Saves the current profile and switches to the new given profile
        /// </summary>
        /// <param name="nameOfProfileToSwitchTo"></param>
        /// <param name="outputFolder"></param>
        public static void ChangeProfile(string? nameOfProfileToSwitchTo,string outputFolder)
        {
            try
            {
                if (_currentProfile != null)
                {
                    SaveCurrentProfile(outputFolder);
                }

                if (nameOfProfileToSwitchTo != null)
                {
                    _currentProfile = new Profile(Path.Join(Program.AppDataPath, "/Profiles/"),
                        nameOfProfileToSwitchTo);

                }
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to switch profile");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to switch profile");
                    
                }
                throw exception;
            }
        }



        /// <summary>
        /// Empties the dictionary of counts of the current device 
        /// </summary>
        public static void ClearCurrentDeviceDictionary()
        {
            try
            {
                var list = new List<string>(CurrentDevice.KeysCount.Keys.Keys);
                CurrentDevice.KeysCount.RemoveList(list);
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying clear the currents device keys counts");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying clear the currents device keys counts");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Increment the count of the key if it exists, else adds it to the dictionary of keys with a count of 1 for the mouse
        /// </summary>
        /// <param name="key"></param>
        public static void MouseInputUpdate(string key)
        {
            try
            {
                _currentProfile?.Mouse?.AddOne(key);
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to update the mouse key counts");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to update the mouse key counts");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Increment the count of the key if it exists, else adds it to the dictionary of keys with a count of 1 for the keyboards
        /// </summary>
        /// <param name="key"></param>
        public static void KeyboardInputUpdate(string key)
        {
            try
            {
                _currentProfile?.Keyboard?.AddOne(key);
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to update the keyboard key counts");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to update the keyboard key counts");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// For each key in the list increment its count if it exists, else adds it to the dictionary of keys with a count of 1 for the gamepads
        /// </summary>
        /// <param name="listOfButtonPresses"></param>
        public static void GamepadInputUpdate(List<string> listOfButtonPresses)
        {
            try
            {
                _currentProfile?.Gamepad?.AddList(listOfButtonPresses);
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to update the gamepad key counts");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to update the gamepad key counts");
                    
                }
                throw exception;
            }
        }


        /// <summary>
        /// Switches the current device to the given type
        /// </summary>
        /// <param name="newDevice"></param>
        public static void ChangeDevice(string newDevice)
        {
            

            try
            {
                if (_currentProfile == null)
                {
                    throw new NullReferenceException("Profile is null");
                }
                switch(newDevice)
                {
                    case "Keyboard":
                    {
                        CurrentDevice = _currentProfile.Keyboard ?? throw new NullReferenceException("Device keyboard is null");
                        break;
                    }
                    case "Mouse":
                    {
                        CurrentDevice = _currentProfile.Mouse ?? throw new NullReferenceException("Device mouse is null");
                        break;
                    }
                    case "Gamepad":
                    {
                        CurrentDevice = _currentProfile.Gamepad ?? throw new NullReferenceException("Device gamepad is null");
                        break;
                    }
                    default:
                    {
                        throw new ChainingException("Unknown device");
                    }
                }
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to change the current device");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to change the current device");
                }
                throw exception;
            }
        }

        /// <summary>
        /// removes the given handlers from the device
        /// </summary>
        /// <param name="newKeyPress"></param>
        /// <param name="oldKeyPress"></param>
        /// <param name="addedList"></param>
        /// <param name="removeList"></param>
        public static void RemoveHandlers(DictionaryWithEvents.DictionaryEvent newKeyPress,DictionaryWithEvents.DictionaryEvent oldKeyPress,DictionaryWithEvents.DictionaryEvent addedList,DictionaryWithEvents.DictionaryEvent removeList )
        {
            try
            {
                if (CurrentDevice != null && CurrentDevice.KeysCount.HasSubscribers)
                {
                    CurrentDevice.KeysCount.OnAddStatus -= newKeyPress;
                    CurrentDevice.KeysCount.OnUpdateStatus -= oldKeyPress;
                    CurrentDevice.KeysCount.OnAddList -= addedList;
                    CurrentDevice.KeysCount.OnRemoveList -= removeList;
                    CurrentDevice.KeysCount.HasSubscribers = false;
                }
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to remove device handlers");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to remove device handlers");
                    
                }
                throw exception;
            }
            
        }


        /// <summary>
        /// Sets up the handlers for the possible events of a device 
        /// </summary>
        /// <param name="newKeyPress"></param>
        /// <param name="oldKeyPress"></param>
        /// <param name="addedList"></param>
        /// <param name="removeList"></param>
        public static void SetupDeviceHandlers(DictionaryWithEvents.DictionaryEvent newKeyPress,DictionaryWithEvents.DictionaryEvent oldKeyPress,DictionaryWithEvents.DictionaryEvent addedList,DictionaryWithEvents.DictionaryEvent removeList )
        {
            try
            {
                CurrentDevice.KeysCount.OnAddStatus += newKeyPress;
                CurrentDevice.KeysCount.OnUpdateStatus += oldKeyPress;
                CurrentDevice.KeysCount.OnAddList += addedList;
                CurrentDevice.KeysCount.OnRemoveList += removeList;
                CurrentDevice.KeysCount.HasSubscribers = true;
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to set up device handlers");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to set up device handlers");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>If a profile is loaded then returns its name else returns null</returns>
        public static string? GetName()
        {
            return _currentProfile?.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if the current profile is supposed to monitor a gamepad, false otherwise, null if there is no currently loaded profile</returns>
        public static bool? UsesGamepad()
        {
            return _currentProfile?.HasGamepad;
        }


        /// <summary>
        /// Get all the devices that the current profile is supposed to monitor
        /// </summary>
        /// <returns>List of strings representing the devices</returns>
        public static  List<string>? GetListOfDevices()
        {
            
            try
            {
                if (_currentProfile == null)
                {
                    return null;
                }

                List<string> devices = new List<string> {"Keyboard", "Mouse"};
                if (_currentProfile.HasGamepad)
                {
                    devices.Add("Gamepad");
                }

                return devices;
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to get the current profiles devices");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to get the current profiles devices");
                    
                }
                throw exception;
            }
        }



        /// <summary>
        /// Updates the time spent in the current profile
        /// </summary>
        /// <param name="timeToAdd">long to add to the count of time</param>
        public static void UpdateTime(long timeToAdd)
        {
            try
            {
                if (timeToAdd > 0 && _currentProfile != null)
                {
                    _currentProfile.TimeSpent += timeToAdd;
                }
               
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to update time spent");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to update time spent");
                    
                }
                throw exception;
            }
        }


        /// <summary>
        /// Updates the time spent in the current profile
        /// </summary>
        /// <param name="newTime">TimeSpan to add to the count of time</param>
        public static void UpdateTime(TimeSpan? newTime)
        {
            
            try
            {
                if (newTime.HasValue && _currentProfile != null)
                {
                    _currentProfile.TimeSpent += newTime.Value.Ticks;
                }
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to update time spent");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to update time spent");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Gets the currents profile time
        /// </summary>
        /// <returns>long representing the time spent in the current profile</returns>
        public static long? GetTimeSpent()
        {
            try
            {
                return _currentProfile?.TimeSpent;
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to get the time spent");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to get the time spent");
                    
                }
                throw exception;
            }
        }
        

        /// <summary>
        /// JSON serializes the currently used profile
        /// </summary>
        /// <param name="outputFolder"></param>
        public static void SaveCurrentProfile(string outputFolder)
        {
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                if (_currentProfile != null)
                {
                    using (StreamWriter sw = File.CreateText(outputFolder + "\\" + _currentProfile.Name + ".txt"))
                    {
                        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            IncludeFields = true
                        };
                        sw.Write(JsonSerializer.Serialize(_currentProfile, jsonSerializerOptions));
                    }
                }
                
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to save the current profile");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to save the current profile");
                    
                }
                throw exception;
            }
        }


        /// <summary>
        /// Creates and saves a new profile
        /// </summary>
        /// <param name="outputFolder"></param>
        /// <param name="newProfileName"></param>
        /// <param name="usesGamepad"></param>
        /// <returns></returns>
        public static bool CreateNewProfile(string outputFolder,string newProfileName, bool usesGamepad)
        {
            try
            {
                var newProfile = new Profile(newProfileName,usesGamepad); 
                SaveProfile(outputFolder,newProfile);
                return true;
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to create new profile");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to create new profile");
                    
                }
                throw exception;
            }
        }


        /// <summary>
        /// JSON serializes a given profile to the given output folder 
        /// </summary>
        /// <param name="outputFolder"></param>
        /// <param name="profile"></param>
        private static void SaveProfile(string outputFolder,Profile profile)
        {
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                using (StreamWriter sw = File.CreateText(outputFolder + "\\" + profile.Name + ".txt"))
                {
                    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        IncludeFields = true
                    };
                    sw.Write(JsonSerializer.Serialize(profile, jsonSerializerOptions));
                }
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to save a given profile");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to save a given profile");
                    
                }
                throw exception;
            }

            
        }


        class Profile 
        {
            [JsonInclude] 
            public string? Name { get; internal set; }
            [JsonInclude]
            public  Mouse? Mouse{ get; internal set; }
            [JsonInclude]
            public Keyboard? Keyboard { get; internal set; }
            [JsonInclude]
            public Gamepad? Gamepad{ get; internal set; }
            [JsonInclude]
            public bool HasGamepad { get; internal set; }
            [JsonInclude]
            public long TimeSpent { get; internal set; }

            [JsonConstructor]
            public Profile()
            {

            }
            

            /// <summary>
            /// make a new Profile with the given name and usesGamepad value
            /// </summary>
            /// <param name="name"></param>
            /// <param name="hasGamepad"></param>
            public Profile(string name, bool usesGamepad)
            {
                this.Name = name;
                this.HasGamepad = usesGamepad;
                this.TimeSpent = 0;
                Keyboard = new Keyboard();
                Mouse = new Mouse();
                Gamepad = new Gamepad();

            }


            /// <summary>
            /// Reads a profile from the folder containing all profiles
            /// </summary>
            /// <param name="containingFolder"></param>
            /// <param name="name"></param>
            public Profile(string containingFolder,string name)
            {
                Keyboard = new Keyboard();
                Mouse = new Mouse();
                Gamepad = new Gamepad();
                if (File.Exists(containingFolder + "\\" + name + ".txt"))
                {

                    string fileContents = File.ReadAllText(containingFolder + "\\" + name + ".txt");
                    try
                    {
                        //Console.WriteLine("trying to read profile");
                        var profile = JsonSerializer.Deserialize<Profile>(fileContents);
                        if (profile == null)
                        {
                            throw new ArgumentNullException("Profile is null");
                        }
                        Name = profile.Name ?? throw new ArgumentNullException("Profile name is null");
                        HasGamepad = profile.HasGamepad;
                        Keyboard.KeysCount = profile.Keyboard?.KeysCount ?? throw new ArgumentNullException("Keyboard dictionary is null");
                        Keyboard.KeysRename = profile.Keyboard.KeysRename ?? throw new ArgumentNullException("Keyboard rename dictionary is null");
                        Mouse.KeysCount = profile.Mouse?.KeysCount ?? throw new ArgumentNullException("Mouse dictionary is null");
                        Mouse.KeysRename = profile.Mouse.KeysRename ?? throw new ArgumentNullException("Mouse rename dictionary is null");
                        Gamepad.KeysCount = profile.Gamepad?.KeysCount ?? throw new ArgumentNullException("Gamepad dictionary is null"); 
                        Gamepad.KeysRename = profile.Gamepad.KeysRename ?? throw new ArgumentNullException("Gamepad rename dictionary is null");
                        TimeSpent = profile.TimeSpent;
                    }
                    catch (Exception e)
                    {
                        var exception = new  ChainingException(e.Message);
                        exception.AddErrorToChain("While trying to read profile file");
                        throw exception;
                    }
                }
            }

        }

        
    }


    
}
