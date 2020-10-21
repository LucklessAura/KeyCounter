using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace KeyCounter
{
    /// <summary>
    /// Class defining the options for the program and the corresponding operations
    /// </summary>
    public class Options
    {
    
        public bool AutoStart { get; set; }
        public string ProfilesLocation { get; set; }
        public string LastSelectedProfile { get; set; }
        public bool UseLastProfile { get; set; }
        public bool StartMinimised { get; set; }
        public string OnStartProfile { get; set; }
        public BindingList<string> ProfilesList { get; set; }
        private string _execDirectoryPath;

        /// <summary>
        /// Create a new options instance and set the folder where the executable for the current instance of the options
        /// </summary>
        public Options()
        {
            this._execDirectoryPath = Path.GetDirectoryName(Application.ExecutablePath);
        }

        /// <summary>
        /// Reset the curent options to their default values, if the <c>deleteProfiles</c> value is true also reset the 
        /// profiles list 
        /// </summary>
        /// <param name="deleteProfiles">determines if the profiles list should be reset or kept</param>
        public void ResetToDefaultOptions(bool deleteProfiles)
        {
            AutoStart = false;
            ProfilesLocation = Path.Join(Path.GetDirectoryName(Application.ExecutablePath), "Profiles");
            LastSelectedProfile = "";
            UseLastProfile = true;
            OnStartProfile = "";
            StartMinimised = false;
            if (deleteProfiles == true || ProfilesList == null)
            {
                ProfilesList = new BindingList<string>();
            }
        }

        /// <summary>
        /// JSON serialize the current options to the corresponding file in the app folder
        /// </summary>
        public void SaveOptionsToFile()
        {
            using (StreamWriter sw = File.CreateText(_execDirectoryPath + "\\" + "options.cfg"))
            {
                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                sw.Write(JsonSerializer.Serialize(this, jsonSerializerOptions));
            }
        }

        /// <summary>
        /// Try to deserialize a options file, if the file does not exist create one with default options and a Profiles folder
        /// </summary>
        public void ReadOrCreateOptionsFile()
        {
            string[] files = Directory.GetFiles(_execDirectoryPath);

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }

            /// <summary>
            /// check if an options.cfg file exist in the apps folder, if it exists deserialize it in a <c>readOptions</c>
            /// variable and copy the read options to the current <c>Options</c> object
            /// </summary>
            if (Miscelanious.IsInVector(files, "options.cfg"))
            {
                string textOptions = System.IO.File.ReadAllText(_execDirectoryPath + "\\" + "options.cfg");
                Options readOptions = JsonSerializer.Deserialize<Options>(textOptions);
                this.LastSelectedProfile = readOptions.LastSelectedProfile;
                this.OnStartProfile = readOptions.OnStartProfile;
                this.ProfilesList = readOptions.ProfilesList;
                this.ProfilesLocation = readOptions.ProfilesLocation;
                this.UseLastProfile = readOptions.UseLastProfile;
                this.AutoStart = readOptions.AutoStart;
                this.StartMinimised = readOptions.StartMinimised;
            }
            else
            {
                try
                {
                    if (!Directory.Exists(_execDirectoryPath + "\\" + "Profiles"))
                    {
                        System.IO.Directory.CreateDirectory(_execDirectoryPath + "\\" + "Profiles");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                this.ResetToDefaultOptions(true);
                SaveOptionsToFile();
            }
        }
    }
}
