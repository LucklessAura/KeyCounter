using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace KeyCounter
{
    /// <summary>
    /// Structure that defines a profile.
    /// </summary>
    public struct Profile
    {
        public string Name { get; set; }
        public float TimeUsed { get; set; }
        public bool NeedsGamepad { get; set; }
        public DictionaryWithEvents KeyboardKeys {get; set;}
        public DictionaryWithEvents MouseKeys {get; set;}
        public DictionaryWithEvents GamepadKeys {get; set;}
        public DictionaryWithEvents TotalKeys {get; set;}
        public List<string> TypesOfInputList { get; set; }

    }

    /// <summary>
    /// Class that contains operations that can be made on a <c>Profile</c>.
    /// </summary>
    public static class ProfileManager
    {
        /// <summary>
        /// Path to the <c>Profiles</c> folder location on disk.
        /// </summary>
        public static string ProfilesFolder { get; set; }

        /// <summary>
        /// Create a <c>Profile</c> with default values and save it on the disk.
        /// </summary>
        /// <param name="name"> the name that the new <c>Profile</c> should have</param>
        /// <param name="needsGamepad">a bool denoting if the profile should take gamepad inputs</param>
        public static void CreateProfile(string name, bool needsGamepad)
        {
            
            Profile profile = new Profile
            {
                Name = name,
                NeedsGamepad = needsGamepad
            };
            profile.KeyboardKeys = new DictionaryWithEvents();
            profile.MouseKeys = new DictionaryWithEvents();
            profile.GamepadKeys = new DictionaryWithEvents();
            profile.TotalKeys = new DictionaryWithEvents();
            profile.TypesOfInputList = new List<string>();
            profile.TimeUsed = 0.00f;
            profile.TypesOfInputList.Add("Keyboard");
            profile.TypesOfInputList.Add("Mouse");
            if (profile.NeedsGamepad == true)
            {
                profile.TypesOfInputList.Add("Gamepad");
            }
            profile.TypesOfInputList.Add("Total");
                
            ProfileManager.SaveProfile(profile);
            
        }

        /// <summary>
        /// JSON serialize a <c>Profile</c> and save it to a file with the name of the profile.
        /// </summary>
        /// <param name="profile">the <c>Profile</c> to be serialized</param>
        public static void SaveProfile(Profile profile)
        {
            using (StreamWriter sw = File.CreateText(ProfilesFolder + "\\" + profile.Name + ".json"))
            {
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                sw.Write(JsonSerializer.Serialize(profile, jsonSerializerOptions));
            }
        }

        /// <summary>
        /// Deserialize a <c>Profile</c> with the name <c>profileName</c> from a file and return it.
        /// </summary>
        /// <param name="profileName"> string denoting the name of the <c>Profile</c> to be loaded</param>
        /// <returns>
        /// A <c>Profile</c>
        /// </returns>
        /// <exception cref="System.IO.FileNotFoundException">
        /// Thrown when the file with <c>profileName</c> is not found.
        /// </exception>
        public static Profile SelectProfile(string profileName)
        {

            Profile profile = JsonSerializer.Deserialize<Profile>(File.ReadAllText(ProfilesFolder + "\\" + profileName + ".json"));
            List<string> keys = profile.KeyboardKeys.Dictionary.Keys.ToList();
            
            // For each key in the profile create a new CustomPair containing the number of presses and the image for each key of the keyboard dictionary
            foreach (string key in keys)
            {
                if (!profile.KeyboardKeys.Dictionary.ContainsKey(key))
                {
                    profile.KeyboardKeys.Dictionary[key] = new CustomPair(profile.KeyboardKeys.Dictionary[key].Number, KeyboardImages.GetImageForKey(key));
                    profile.TotalKeys.Dictionary[key] = new CustomPair(profile.TotalKeys.Dictionary[key].Number, KeyboardImages.GetImageForKey(key));
                }
                else
                {
                    profile.KeyboardKeys.Dictionary[key].ReplaceImage(KeyboardImages.GetImageForKey(key));
                    profile.TotalKeys.Dictionary[key].ReplaceImage(KeyboardImages.GetImageForKey(key));
                }
                
            }

            // For each key in the profile create a new CustomPair containing the number of presses and the image for each key of the mouse dictionary
            keys = profile.MouseKeys.Dictionary.Keys.ToList();
            foreach (string key in keys)
            {
                if (!profile.MouseKeys.Dictionary.ContainsKey(key))
                {
                    profile.MouseKeys.Dictionary[key] = new CustomPair(profile.MouseKeys.Dictionary[key].Number, MouseImages.GetImageForKey(key));
                    profile.TotalKeys.Dictionary[key] = new CustomPair(profile.TotalKeys.Dictionary[key].Number, MouseImages.GetImageForKey(key));
                }
                else
                {
                    profile.MouseKeys.Dictionary[key].ReplaceImage(MouseImages.GetImageForKey(key));
                    profile.TotalKeys.Dictionary[key].ReplaceImage(MouseImages.GetImageForKey(key));
                }

            }

            // For each key in the profile create a new CustomPair containing the number of presses and the image for each key of the gamepad dictionary
            keys = profile.GamepadKeys.Dictionary.Keys.ToList();
            foreach (string key in keys)
            {
                if (!profile.GamepadKeys.Dictionary.ContainsKey(key))
                {
                    profile.GamepadKeys.Dictionary[key] = new CustomPair(profile.GamepadKeys.Dictionary[key].Number, GamepadImages.GetImageForKey(key));
                    profile.TotalKeys.Dictionary[key] = new CustomPair(profile.TotalKeys.Dictionary[key].Number, GamepadImages.GetImageForKey(key));
                }
                else
                {
                    profile.MouseKeys.Dictionary[key].ReplaceImage(GamepadImages.GetImageForKey(key));
                    profile.TotalKeys.Dictionary[key].ReplaceImage(GamepadImages.GetImageForKey(key));
                }
                
            } 

            return profile;

        }

        /// <summary>
        /// Add a key to the corresponding dictionary in current selected <c>Profile</c> 
        /// </summary>
        /// <param name="key"> The key to be added to a dictionary</param>
        /// <param name="type">The input device of the key (keyboard/ mouse/ gamepad)</param>
        public static void AddToDictionary(string key,string type)
        {
            // the Xbox gamepad returns the middle button press as a keyboard press.
            if(key == "LButton, XButton2")
            {
                type = "gamepad";
            }

            if (type == "keyboard")
            {
                // Add or increase count to the keyboard dictionary
                if (MainForm.CurrentProfile.KeyboardKeys.ContainsKey(key))
                {
                    MainForm.CurrentProfile.KeyboardKeys.AddOne(key);
                }
                else
                {
                    MainForm.CurrentProfile.KeyboardKeys.Add(key, KeyboardImages.GetImageForKey(key), 1);
                }

                // Add or increase count to the total keys dictionary
                if (MainForm.CurrentProfile.TotalKeys.ContainsKey(key))
                {
                 
                    MainForm.CurrentProfile.TotalKeys.AddOne(key);
                }
                else
                {
                    MainForm.CurrentProfile.TotalKeys.Add(key, KeyboardImages.GetImageForKey(key), 1);
                }
            }
            else if(type == "mouse")
            {
                // Add or increase count to the mouse dictionary
                if (MainForm.CurrentProfile.MouseKeys.ContainsKey(key))
                {
                    MainForm.CurrentProfile.MouseKeys.AddOne(key);
                }
                else
                { 
                    MainForm.CurrentProfile.MouseKeys.Add(key, MouseImages.GetImageForKey(key), 1);
                }

                // Add or increase count to the total keys dictionary
                if (MainForm.CurrentProfile.TotalKeys.ContainsKey(key))
                {
                 
                    MainForm.CurrentProfile.TotalKeys.AddOne(key);
                }
                else
                {
                    MainForm.CurrentProfile.TotalKeys.Add(key, MouseImages.GetImageForKey(key), 1);
                }
            }
            else if(type == "gamepad")
            {
                // Add or increase count to the gamepad dictionary
                if (MainForm.CurrentProfile.GamepadKeys.ContainsKey(key))
                {
                    MainForm.CurrentProfile.GamepadKeys.AddOne(key);
                }
                else
                {
                    MainForm.CurrentProfile.GamepadKeys.Add(key, GamepadImages.GetImageForKey(key), 1);
                }

                // Add or increase count to the total keys dictionary
                if (MainForm.CurrentProfile.TotalKeys.ContainsKey(key))
                {
                    
                    MainForm.CurrentProfile.TotalKeys.AddOne(key);
                }
                else
                {
                    MainForm.CurrentProfile.TotalKeys.Add(key, GamepadImages.GetImageForKey(key), 1);
                }
            }
        }

        ///<summary>
        /// Delete a <c>Profile</c> from the disk and from the Options Profile List.
        /// <para>The deleted profile is usually the currently active profile</para>
        /// </summary>
        /// <param name="currentProfile"> the profile to be deleted</param>
        /// <param name="options"> the options loaded in memory</param>
        public static void DeleteProfile(Profile currentProfile,Options options)
        {
            File.Delete(ProfilesFolder + "\\" + currentProfile.Name + ".json");
            options.ProfilesList.Remove(currentProfile.Name);
        }

        ///<summary>
        /// Move the <c>Profiles Folder</c> to a new location.
        /// <para> This moves the <c>Profiles Folder</c>, no data is deleted in the process</para>
        /// </summary>
        /// <param name="newProfilesLocation">the new location of the <c> Profiles Folder</c></param>
        /// <exception cref="ArgumentException"> Thrown when the new location already has a folder named <c>Profiles</c>
        /// </exception>
        internal static void MoveProfiles(string newProfilesLocation)
        {
            if (!Directory.Exists(newProfilesLocation + "\\" + "Profiles"))
            {
                Directory.CreateDirectory(newProfilesLocation + "\\" + "Profiles");
                foreach (string item in Directory.GetFiles(ProfilesFolder))
                {
                    Console.WriteLine(item);
                    File.Copy(item, newProfilesLocation + "\\" + "Profiles" + "\\" + Path.GetFileName(item), true);
                    File.Delete(item);
                }
                Directory.Delete(ProfilesFolder, true);
            }
            else
            {
                throw new ArgumentException("Folder already Exists");
            }
        }
    }
}
