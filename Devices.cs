using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace KeyCounter
{
    class Keyboard : BaseDevice
    {
        public Keyboard()
        {
            ImagesPath = Path.Join(Program.CommonAppsData,"/Icons/Keyboard/");
            KeysCount = new DictionaryWithEvents();
            KeysRename = new Dictionary<string, string>();
        }
    }

    class Mouse : BaseDevice
    {
        public Mouse()
        {
            ImagesPath = Path.Join(Program.CommonAppsData,"/Icons/Mouse/");
            KeysCount = new DictionaryWithEvents();
            KeysRename = new Dictionary<string, string>();
        }

        protected override bool CreateImageForKey(string key)
        {
            // Since the majority of buttons are unique and predefined and easily recognizable the images for them are in templates,
            // this makes is so that those "template" images are returned without writing anything on them if needed
            try
            {
                if (ImagesLoaded == false)
                {
                    return false;
                }

                if (Templates.Keys.Contains(key))
                {
                    Images.GetOrAdd(key,(Image)Templates[key].Clone());
                }
                else
                {
                    Images.GetOrAdd(key, WriteOnTemplate((Image)Templates["key"].Clone(), key));
                }

                return true;
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying create a mouse image");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying create a mouse image");
                    
                }
                throw exception;
            }
        }

    }

    class Gamepad : BaseDevice
    {
        public Gamepad()
        {
            ImagesPath = Path.Join(Program.CommonAppsData,"/Icons/Gamepad/");
            KeysCount = new DictionaryWithEvents();
            KeysRename = new Dictionary<string, string>();
        }

        protected override bool CreateImageForKey(string key)
        {
            // Since the majority of buttons are unique and predefined and easily recognizable the images for them are in templates,
            // this makes is so that those "template" images are returned without writing anything on them if needed
            try
            {
                if (ImagesLoaded == false)
                {
                    return false;
                }

                if (Templates.Keys.Contains(key))
                {
                    Images.GetOrAdd(key,(Image)Templates[key].Clone());
                }
                else
                {
                    Images.GetOrAdd(key, WriteOnTemplate((Image)Templates["key"].Clone(), key));
                }

                return true;
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to create a gamepad image");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to create a gamepad image");
                    
                }
                throw exception;
            }
        }
    }

}
