using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace KeyCounter
{
    public class Options : INotifyPropertyChanged 
    {
        public bool AutoStart { get; set; }
        public string ProfilesLocation { get; set; }
        public string LastUsedProfile { get; set; }
        public bool UseLastProfile { get; set; }
        public bool StartMinimized { get; set; }
        public string OnStartProfile { get; set; }
        public BindingList<string> ProfileList { get; set; }
        
        public Options()
        {
            //Console.WriteLine("New options object created");
        }

        /// <summary>
        /// Creates default options, if resetProfileList is false then the current list of profiles is kept, else it is reset to default
        /// </summary>
        /// <param name="resetProfileList"></param>
        public void MakeOptionsDefault(bool resetProfileList)
        {
            try
            {
                AutoStart = false;
                ProfilesLocation = Path.Join(Program.AppDataPath, "Profiles");
                LastUsedProfile = null;
                UseLastProfile = true;
                StartMinimized = false;
                OnStartProfile = null;
            
                if(resetProfileList)
                    // don't also delete the files of the previous profiles in case the options were reset to default cause of a corrupted options file
                    ProfileList = new BindingList<string>(){"Default"};
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to default options");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to default options");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// JSON serializes the current options
        /// </summary>
        public void WriteOptions()
        {
            try
            {
                using (StreamWriter sw = File.CreateText(Program.AppDataPath + "\\" + "options.cfg"))
                {
                    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    sw.Write(JsonSerializer.Serialize(this, jsonSerializerOptions));
                }
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to write options");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to write options");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Tries to read the options, if the file does not exist then create new options with default values and save them
        /// </summary>
        public void ReadOrCreateOptions()
        {
            try
            {
                if (File.Exists(Program.AppDataPath + "\\" + "options.cfg"))
                {
                    string textOptions = File.ReadAllText(Program.AppDataPath + "\\" + "options.cfg");
                    try
                    {
                        //Console.WriteLine("Trying to read options");
                        // am making options to create options, creates recursive behaviour and stack overflows
                        var options = JsonSerializer.Deserialize<Options>(textOptions);
                        AutoStart = options.AutoStart;
                        ProfilesLocation = options.ProfilesLocation ?? throw new Exception("Null option");
                        LastUsedProfile = options.LastUsedProfile ;
                        UseLastProfile = options.UseLastProfile;
                        StartMinimized = options.StartMinimized;
                        OnStartProfile = options.OnStartProfile;
                        ProfileList = options.ProfileList ?? throw new Exception("Null option");
                    }
                    catch (Exception)
                    {
                        MakeOptionsDefault(true);
                        WriteOptions();
                        throw new ChainingException("Options file corrupted, created new file with default settings");
                    }
                }
                else
                {
                    //assume that it is a first time setup, probably could have created files using the installer, but meh
                    ProfileManager.CreateNewProfile(Path.Join(Program.AppDataPath, "Profiles"), "Default", false);
                    MakeOptionsDefault(true);
                    WriteOptions();
                }
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to read options");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to read options");
                    
                }
                throw exception;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
