using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace KeyCounter
{
    abstract class KeysImages
    {
        protected string ImagesPath = "";
        protected ConcurrentDictionary<string, Image> Images;
        private readonly FontFamily _fontFamily = new FontFamily("Candara");
        private static Font? _font;
        protected ConcurrentDictionary<string,Image> Templates;
        protected bool ImagesLoaded;


        protected KeysImages()
        {
            Templates = new ConcurrentDictionary<string, Image>();
            Images = new ConcurrentDictionary<string, Image>();
            ImagesLoaded = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>bool that indicates if the images are currently loaded in memory</returns>
        public bool AreImagesLoaded()
        {
            return ImagesLoaded;
        }


        /// <summary>
        /// Loads all images and templates in memory used in a worker thread
        /// </summary>
        /// <param name="worker">worker argument used to see if loading should be cancelled</param>
        /// <returns>true if the images were loaded, false if the worker was  interrupted</returns>
        public bool LoadImages(BackgroundWorker worker)
        {
            try
            {

                if (ImagesPath == "")
                {
                    throw new NullReferenceException("path to images cannot be empty");
                }

                foreach (var file in Directory.EnumerateFiles(Path.Join(ImagesPath, "templates/")))
                {
                    if (file.EndsWith(".png"))
                    {

                        //frees the image after loading them so they can be freely used in other operations, otherwise the handle to the file would be 
                        // kept by the app
                        var auxImage = Image.FromFile(file);
                        Templates.GetOrAdd(Path.GetFileNameWithoutExtension(file), new Bitmap(auxImage));

                        //Images are evil, they must be both exposed and all all references removed for the GC to take'em 
                        auxImage.Dispose();
                    
                        if (worker.CancellationPending)
                        {
                            ImagesLoaded = false;
                            foreach (var key in Templates.Keys)
                            {
                                Templates[key].Dispose();
                            }
                            Templates.Clear();
                            return false;
                        }
                    }   
                }

                foreach (var file in Directory.EnumerateFiles(ImagesPath))
                {
                    if (file.EndsWith(".png"))
                    {
                        Console.WriteLine(Path.GetFileNameWithoutExtension(file));
                        var auxImage = Image.FromFile(file);
                        Images.GetOrAdd(Path.GetFileNameWithoutExtension(file), new Bitmap(auxImage));
                        auxImage.Dispose();
                        if (worker.CancellationPending)
                        {
                            ImagesLoaded = false;
                            
                            foreach (var key in Templates.Keys)
                            {
                                Templates[key].Dispose();
                            }
                            foreach (var key in Images.Keys)
                            {
                                Images[key].Dispose();
                            }

                            Templates.Clear();
                            Images.Clear();
                            return false;
                        }
                    }   
                }

                ImagesLoaded = true;
                return true;
            }
            catch (Exception e)
            {
                var exception = new ChainingException(e.Message);
                exception.AddErrorToChain("While trying to load images");
                throw exception;

            }
            
        }

        /// <summary>
        /// Disposes of both the templates and images loaded in memory
        /// </summary>
        public void UnloadImages()
        {
            try
            {
                if (Images.Count != 0)
                {
                    foreach (var key in Images.Keys)
                    {
                        Images[key].Dispose();
                    }
                }

                if (Templates.Count != 0)
                {
                    foreach (var key in Templates.Keys)
                    {
                        Templates[key].Dispose();
                    }
                }

                Templates.Clear();
                Images.Clear();
                ImagesLoaded = false;
            }
            catch (Exception e)
            {
                var exception = new ChainingException(e.Message);
                exception.AddErrorToChain("While trying to unload images");
                throw exception;
            }

        }

        /// <summary>
        /// If the image for the specific key is already created it returns it else it creates it and returns it
        /// </summary>
        /// <param name="key">the key for which an image should be returned</param>
        /// <returns>An image for the key or null if the images are not loaded in memory</returns>
        public Image? GetOrCreateImageForKey(string key)
        {
            try
            {

                if (!ImagesLoaded)
                {
                    return null;
                }
                if(Images.ContainsKey(key) && Images[key] != null)
                {
                    return Images[key];
                }

                CreateImageForKey(key);
                return Images[key];
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to get or create image");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to get or create image");
                }
                throw exception;
            }
            
        }


        /// <summary>
        /// Tries to use one of the two templates depending on the length of the input and create a new image for that key
        /// </summary>
        /// <param name="key">the key for which an image should be created</param>
        /// /// <returns>true if the image was created, false otherwise</returns>
        protected virtual bool CreateImageForKey(string key)
        {
            try
            {
                var template = key.Length > 2 ? "big_key" : "key";

                if (ImagesLoaded == false)
                {
                    return false;
                
                }
               
                Images.GetOrAdd(key,WriteOnTemplate((Image)Templates[template].Clone(),key));
                return true;
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to create image");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to create image");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Writes the keys name on a given template
        /// </summary>
        /// <param name="template">Image on which the name of the key should be written</param>
        /// <param name="name">The name of the key</param>
        /// <returns>An image with the name centered on it</returns>
        protected Image WriteOnTemplate(Image template,string name)
        {
            try
            {
                using (Graphics g =  Graphics.FromImage(template))
                {
                    int size = 150;
                    _font = new Font(_fontFamily, size);
                    
                    // measure the size of the font so that the text fits the image
                    while (g.MeasureString(name, _font).Width > template.Width || g.MeasureString(name, _font).Height > template.Height)
                    {
                        _font.Dispose();
                        size -= 5;
                        _font = new Font(_fontFamily, size);
                        if (size < 6)
                        {
                            break;
                        }
                    }

                    //actually write the text, centered, on the image
                    g.DrawString(name, _font, Brushes.Gray, new Point((int)((template.Width - g.MeasureString(name, _font).Width) / 2), (int)((template.Height - g.MeasureString(name, _font).Height) / 2)));
                    try
                    {
                        if (File.Exists(Path.Join(ImagesPath,"/"+name+".png")))
                        {
                            File.Delete(Path.Join(ImagesPath,"/"+name+".png"));
                        }
                        template.Save(Path.Join(ImagesPath,"/"+name+".png"));
                    }
                    catch (Exception e)
                    {
                        var exception = new  ChainingException(e.Message);
                        exception.AddErrorToChain("While trying to delete image");
                        throw exception;
                    }

                    _font.Dispose();
                }

                return template;
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to write on template image");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to write on template image");
                        
                }
                throw exception;
            }
        }

        /// <summary>
        /// removes an entry from the dictionary
        /// </summary>
        /// <param name="originalKey"></param>
        public void RemoveImage(string originalKey)
        {
            try
            {
                if (Images.ContainsKey(originalKey))
                {
                    Images[originalKey].Dispose();
                }
                Images.TryRemove(originalKey, out var aux);
                aux?.Dispose();
            }
            catch (Exception e)
            {
                var exception = new  ChainingException(e.Message);
                exception.AddErrorToChain("While trying to remove image from dictionary");
                throw exception;
            }
            
        }

        /// <summary>
        /// Adds or replaces an image in the folder of images, this removes any image on the Images folder that has the same name as newImageName
        /// </summary>
        /// <param name="newImageName"></param>
        /// <param name="newImage"></param>
        public void AddOrReplace(string newImageName, Image? newImage)
        {

            try
            {
                if (File.Exists(Path.Join(ImagesPath,"/"+newImageName+".png")))
                {
                    File.Delete(Path.Join(ImagesPath,"/"+newImageName+".png"));
                }

                Images.TryRemove(newImageName,out var aux);
            
                aux?.Dispose();

                if (newImage == null)
                {
                    return;
                }

                newImage.Save(Path.Join(ImagesPath,"/"+newImageName+".png"));
                Images.GetOrAdd(newImageName, (Image)newImage.Clone());
                newImage.Dispose();


            }
            catch (Exception e)
            {
                var exception = new  ChainingException(e.Message);
                exception.AddErrorToChain("While trying to replace image");
                throw exception;
            }
            
        }
    }
}
