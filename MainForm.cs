using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace KeyCounter
{
    public partial class keyCounterMainFrame_frame : Form
    {
        private readonly Timer _clockTimer;
        private DateTime? _startTime;
        private readonly ImageList _listOfImages = new ImageList();
        private Options _options = new Options();
        private readonly SelectingRendered _renderer = new SelectingRendered();
        private readonly List<Form> _infoForms = new List<Form>();

        private readonly BackgroundWorker _getImageForOneKey_backgroundWorker = new BackgroundWorker();
        private readonly BackgroundWorker _getImagesForListOfKeys_backgroundWorker = new BackgroundWorker();

        private readonly ConcurrentQueue<string> queueImagesYetHaveToCreate = new ConcurrentQueue<string>();
        private readonly ConcurrentQueue<List<string>> queueListsOfImagesYetHaveToCreate = new ConcurrentQueue<List<string>>();


        public delegate void RestartWorkerEvent(object? sender, DictionaryEventArgs? e);

        public event RestartWorkerEvent RestartImageLoader;
        public event RestartWorkerEvent RestartSingleImageMaker;
        public event RestartWorkerEvent RestartListOfImagesMaker;

        private long _remainingSingleImageRestarts = 0;
        private bool _shouldRestartWorker = false;
        private long _remainingListOfImageRestarts = 0;


        public keyCounterMainFrame_frame()
        {
            
            try
            {
                
                //Console.WriteLine("initializing stuff");
            
                InitializeComponent();

                // prevent an event that would fire when the data source changes while initializing other stuff, will set again the handler in the load function
                profile_comboBox.SelectedIndexChanged -= profile_comboBox_SelectedIndexChanged;

            
                _clockTimer = new Timer {Interval = 500};
                _clockTimer.Tick += UpdateTimeOnInterface;
            
                _options.ReadOrCreateOptions();
                _startTime = DateTime.UtcNow;

            
                notifyIcon_main_contextMenuStrip.Items.Add("Maximize",null,taskbar_notifyIcon_DoubleClick);
                notifyIcon_main_contextMenuStrip.Items.Add("Quit",null,CloseRequest_handler);


                taskbar_notifyIcon.ContextMenuStrip = notifyIcon_main_contextMenuStrip;

                UpdateContextMenuProfiles(_options.ProfileList);
                notifyIcon_toolStripMenuItem.DropDownItemClicked += NotifyIcon_toolStripMenuItemOnDropDownItemClicked;

                // create the object that colors the selected profile and color the one selected now
                notifyIcon_main_contextMenuStrip.Renderer = _renderer;
                

                // initialize keyboard and mouse hooks
                KeyboardHookClass.Initialize();
                MouseHookClass.Initialize();


                _listOfImages.ImageSize = new Size(130, 69);
                imagesList_listView.LargeImageList = _listOfImages;


                
                // set up background workers event handlers and make them accept cancellation 
                _getImageForOneKey_backgroundWorker.WorkerSupportsCancellation = true;
                _getImageForOneKey_backgroundWorker.DoWork += GetImageForNewKeyBackgroundWorker;
                _getImageForOneKey_backgroundWorker.RunWorkerCompleted += UpdateListViewForNewImage;
                

                _getImagesForListOfKeys_backgroundWorker.WorkerSupportsCancellation = true;
                _getImagesForListOfKeys_backgroundWorker.DoWork += GetImagesForListOfNewKeysBackgroundWorker;
                _getImagesForListOfKeys_backgroundWorker.RunWorkerCompleted += UpdateListViewForListOfNewImages;
                


                // sometimes a worker might have to restart immediately after completing its work or might have multiple 
                // separate jobs to do in sequence. This takes care to restart them safely since cancellation in itself 
                // might take a while in some cases
                RestartImageLoader += (sender, args) =>
                {
                    _shouldRestartWorker = false;
                    imageLoader_backgroundWorker.RunWorkerAsync();
                };

                RestartSingleImageMaker += (sender, args) =>
                {
                    queueImagesYetHaveToCreate.TryDequeue(out var key);
                    if (key != null)
                    {
                        Interlocked.Decrement(ref _remainingSingleImageRestarts);
                        _getImageForOneKey_backgroundWorker.RunWorkerAsync(argument: key);
                    }
                };

                RestartListOfImagesMaker += (sender, args) =>
                {
                    queueImagesYetHaveToCreate.TryDequeue(out var keys);
                    if (keys != null)
                    {
                        Interlocked.Decrement(ref _remainingListOfImageRestarts);
                        _getImagesForListOfKeys_backgroundWorker.RunWorkerAsync(argument: keys);
                    }
                };
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to initialize the main form");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to initialize the main form");
                    
                }
                throw exception;
            }
        }


        /// <summary>
        /// Update the label in the form with the new time spent in the current profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimeOnInterface(object? sender, EventArgs e)
        {
            try
            {
                var profileTime = ProfileManager.GetTimeSpent();
                if (!profileTime.HasValue)
                {
                    return;
                }

                var span = (DateTime.UtcNow - _startTime) + new TimeSpan(profileTime.Value);
                if (span.HasValue)
                {
                    clock_label.Text = $@"Time spent: {(int) span.Value.TotalHours}:{span.Value.Minutes}:{span.Value.Seconds}";
                }
                else
                {
                    throw new ArgumentNullException("The difference in time is null");
                }
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to update the time on the interface");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to update the time on the interface");
                    
                }
                throw exception;
            }
            
        }


        /// <summary>
        /// Remove icons from the forms list view 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveListOfKeysFromListView(object sender, DictionaryEventArgs e)
        {
            try
            {
                // this prevents the list view to update its interface until end update is called 
                imagesList_listView.BeginUpdate();

                if (e.Keys != null)
                {
                    foreach (var key in e.Keys)
                    {
                        var keyToUse = ProfileManager.CurrentDevice.RenamesContainsKey(key)
                            ? ProfileManager.CurrentDevice.KeysRename[key]
                            : key;
                        if (imagesList_listView.Items.ContainsKey(keyToUse))
                        {
                            imagesList_listView.Items.RemoveByKey(keyToUse);
                            _listOfImages.Images[keyToUse]?.Dispose();
                            _listOfImages.Images.RemoveByKey(keyToUse);
                        }
                    }
                }

                imagesList_listView.EndUpdate();
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to remove a list of images from the list view");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to remove a list of images from the list view");
                    
                }
                throw exception;
            }
        }


        /// <summary>
        /// Update the counter of the key for an image already present in the list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OldKeyPress_Handler(object sender, DictionaryEventArgs e)
        {
            try
            {
                if (!imageLoader_backgroundWorker.IsBusy)
                {
                    imagesList_listView.BeginUpdate();
                    var keyToUse = e.Key != null && ProfileManager.CurrentDevice.RenamesContainsKey(e.Key) ? ProfileManager.CurrentDevice.KeysRename[e.Key] : e.Key;

                    if (imagesList_listView.Items.ContainsKey(keyToUse))
                    {
                        if (e.Key != null)
                        {
                            imagesList_listView.Items[keyToUse].Text = ProfileManager.CurrentDevice[e.Key].ToString();
                        }
                    }
                    imagesList_listView.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to update the interface count for an old key");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to update the interface count for an old key");
                    
                }
                throw exception;
            }
        }


        /// <summary>
        /// Run worker to get image for the new key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewKeyPress_Handler(object sender, DictionaryEventArgs e)
        {
            
            try
            {
                // if the worker is free then do the job else add it to a queue
                if (!_getImageForOneKey_backgroundWorker.IsBusy)
                {
                    _getImageForOneKey_backgroundWorker.RunWorkerAsync(argument : e.Key);    
                }
                else
                {
                    if (e.Key == null)
                    {
                        return;
                    }
                    queueImagesYetHaveToCreate.Enqueue(e.Key);
                    Interlocked.Increment(ref _remainingSingleImageRestarts);

                }
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to run the worker for making a new image");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to run the worker for making a new image");
                    
                }
                throw exception;
            }
        }


        /// <summary>
        /// Do work for background worker that makes 1 image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetImageForNewKeyBackgroundWorker(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!ProfileManager.CurrentDevice.AreImagesLoaded())
                {
                    return;
                }

                var keyToUse = ProfileManager.CurrentDevice.RenamesContainsKey((string)e.Argument) ? ProfileManager.CurrentDevice.KeysRename[(string)e.Argument] : (string)e.Argument;
                ProfileManager.CurrentDevice.GetOrCreateImageForKey(keyToUse);
                e.Result = e.Argument;
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to add a new image for a key");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to add a new image for a key");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Add to the list view the new image and the count for the respective key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateListViewForNewImage(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (ProfileManager.CurrentDevice.AreImagesLoaded())
                {
                
                    imagesList_listView.BeginUpdate();
                    string key = e.Result as string ?? throw new NullReferenceException("Key for image cannot be null for a new image");
                    //string keyToUse;
                    
                    // get the proper name to write on the image, this name is purely esthetic,
                    // the value in the count dictionary will always remain the same for a key
                    var keyToUse = ProfileManager.CurrentDevice.RenamesContainsKey(key) ? ProfileManager.CurrentDevice.KeysRename[key] : key;
                   
                
                    _listOfImages.Images.Add(keyToUse, ProfileManager.CurrentDevice.GetOrCreateImageForKey(keyToUse) ?? throw new NullReferenceException("Images are null whn updating list"));
                    imagesList_listView.Items.Add(keyToUse,ProfileManager.CurrentDevice[key].ToString(), keyToUse);
                    imagesList_listView.EndUpdate();
                }

                // see if there are more jobs left to do 
                if (Interlocked.Read(ref _remainingSingleImageRestarts) > 0)
                {
                    RestartSingleImageMaker?.Invoke(null,null);
                }
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to update the list view with a new image");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to update the list view with a new image");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Do work for background worker that makes images for a list of keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetImagesForListOfNewKeysBackgroundWorker(object sender, DoWorkEventArgs e)
        {
            try
            {
                // no point in requesting an image to get returned if the images are not in memory
                if (!ProfileManager.CurrentDevice.AreImagesLoaded()) return;
                foreach (var key in (List<string>) e.Argument)
                {
                    ProfileManager.CurrentDevice.GetOrCreateImageForKey(key);
                }  
                e.Result = e.Argument;
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to get images for a list of keys");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to get images for a list of keys");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Add to the list view images and values for a list of keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateListViewForListOfNewImages(object sender, RunWorkerCompletedEventArgs e)
        {
            // comments pretty much the same as in UpdateListViewForNewImage
            try
            {
                if (ProfileManager.CurrentDevice.AreImagesLoaded())
                {
                    imagesList_listView.BeginUpdate();
                    foreach (var key in (List<string>) e.Result )
                    {
                        var keyToUse = ProfileManager.CurrentDevice.RenamesContainsKey(key) ? ProfileManager.CurrentDevice.KeysRename[key] : key;
                        
                        if (!_listOfImages.Images.ContainsKey(keyToUse))
                        {

                            _listOfImages.Images.Add(keyToUse, ProfileManager.CurrentDevice.GetOrCreateImageForKey(keyToUse));
                            imagesList_listView.Items.Add(keyToUse,ProfileManager.CurrentDevice[key].ToString(), keyToUse);
                        }
                        else
                        {
                            imagesList_listView.Items[keyToUse].Text = ProfileManager.CurrentDevice[key].ToString();
                        }
                    }  
                    imagesList_listView.EndUpdate();
                }

                if (Interlocked.Read(ref _remainingListOfImageRestarts) > 0)
                {
                    RestartListOfImagesMaker?.Invoke(null,null);
                }
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to update list view with a list of new images");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to update list view with a list of new images");
                }
                throw exception;
            }
        }

        /// <summary>
        /// Run worker to get images for a list of keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListOfKeysAddedHandler(object sender, DictionaryEventArgs e)
        {
            try
            {
                if (!_getImagesForListOfKeys_backgroundWorker.IsBusy)
                {
                    _getImagesForListOfKeys_backgroundWorker.RunWorkerAsync(argument : e.Keys);
                }
                else
                {
                    // add new job in queue if the worker is busy
                    if (e.Keys == null)
                    {
                        return;
                    }
                    queueListsOfImagesYetHaveToCreate.Enqueue(e.Keys);
                    Interlocked.Increment(ref _remainingListOfImageRestarts);

                }
                
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to run the worker for making images for a list of key");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to run the worker for making images for a list of key");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Add to the list view all the images and values inside of the current devices keys count dictionary. Used when
        /// a new device is selected.
        /// </summary>
        private void InitialListViewUpdate()
        {
            try
            {
                imagesList_listView.BeginUpdate();

                imagesList_listView.Items.Clear();

                for (int i = 0; i < _listOfImages.Images.Count; i++)
                {
                    _listOfImages.Images[i].Dispose();
                    _listOfImages.Images.RemoveAt(i);
                }
            
                _listOfImages.Images.Clear();
                foreach (var button in ProfileManager.CurrentDevice)
                {
                    //if the background worker is busy then the form was minimized or a new device was selected so there is no point in adding the images since they
                    // will be removed later
                    if (imageLoader_backgroundWorker.IsBusy || imageLoader_backgroundWorker.CancellationPending)
                    {
                        imagesList_listView.Items.Clear();
                        // might be disposing a bit to often images, but if I'm not making sure they are eliminated from memory leaks happen, so ya know, better safe than sorry
                        for (int i = 0; i < _listOfImages.Images.Count; i++)
                        {
                            _listOfImages.Images[i].Dispose();
                            _listOfImages.Images.RemoveAt(i);
                        }
                        _listOfImages.Images.Clear();
                        imagesList_listView.EndUpdate();
                        return;
                    }
                
                    var keyToUse = ProfileManager.CurrentDevice.RenamesContainsKey(button.Key) ? ProfileManager.CurrentDevice.KeysRename[button.Key] : button.Key;
                    
                    if (ProfileManager.CurrentDevice.GetOrCreateImageForKey(keyToUse) != null)
                    {
                        _listOfImages.Images.Add(keyToUse,
                            ProfileManager.CurrentDevice.GetOrCreateImageForKey(keyToUse) ?? throw new NullReferenceException("Images are not loaded while initializing list after load worker completed"));
                        imagesList_listView.Items.Add(keyToUse, button.Value.ToString(), keyToUse);
                    }
                }

                imagesList_listView.EndUpdate();
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to update the list view for all keys");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to update the list view for all keys");
                    
                }
                throw exception;
            }
            
        }

        private void NotifyIcon_toolStripMenuItemOnDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
             profile_comboBox.SelectedItem = e.ClickedItem.ToString();
        }

        /// <summary>
        /// used to highlight the menu item corresponding to the currently selected profile
        /// </summary>
        private class SelectingRendered : ToolStripProfessionalRenderer
        {
            internal string? SelectedProfile;

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {

                if (e.ToolStrip.Name == "" && SelectedProfile == e.Item.ToString())
                {
                    // colors don't look overly harmonic, but I'm no designer so watevs
                    if (e.Item.Selected)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.LightPink))
                        {
                            e.Graphics.FillRectangle(brush, 0,0,e.Item.Size.Width,e.Item.Size.Height);
                        }
                    }
                    else
                    {
                        using (SolidBrush brush = new SolidBrush(Color.Plum))
                        {
                            e.Graphics.FillRectangle(brush, 0,0,e.Item.Size.Width,e.Item.Size.Height);
                        }
                    }
                }
                else
                {

                    if (e.Item.Selected)
                    {
                        using (SolidBrush brush = new SolidBrush(SystemColors.MenuHighlight))
                        {
                            e.Graphics.FillRectangle(brush, 0,0,e.Item.Size.Width,e.Item.Size.Height);
                        }
                    }
                    else
                    {
                        using (SolidBrush brush = new SolidBrush(SystemColors.Menu))
                        {
                            e.Graphics.FillRectangle(brush, 0,0,e.Item.Size.Width,e.Item.Size.Height);
                        }
                    }
                }
                
            } 
        } 


        /// <summary>
        /// Load all images for the current device in memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            
            //Console.WriteLine("Background worker loading images");
            
            try
            {
                if (!ProfileManager.CurrentDevice.AreImagesLoaded())
                {
                    if (ProfileManager.CurrentDevice.LoadImages((BackgroundWorker) sender) == false)
                    {
                        
                        ProfileManager.CurrentDevice.UnloadImages();
                        //Console.WriteLine(" Background worker cancelled");
                        e.Cancel = true;
                        return;

                    }
                }

                foreach (var button in ProfileManager.CurrentDevice)
                {
                    if (((BackgroundWorker) sender).CancellationPending)
                    {
                        ProfileManager.CurrentDevice.UnloadImages();
                        //Console.WriteLine(" Background worker cancelled");
                        e.Cancel = true;
                        return;
                    }
                    ProfileManager.CurrentDevice.GetOrCreateImageForKey(button.Key);
                }


                //Console.WriteLine("Background worker done loading images");
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to load images with background loader");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to load images with background loader");
                    
                }
                throw exception;
            }

        }


        /// <summary>
        /// Update the available devices of the current profile
        /// </summary>
        /// <param name="newItems"></param>
        private void UpdateContextMenuProfiles(BindingList<string> newItems)
        {
            notifyIcon_toolStripMenuItem.DropDownItems.Clear();
            foreach (var item in newItems)
            {
                notifyIcon_toolStripMenuItem.DropDownItems.Add(item, null);
            }
            
        }


        /// <summary>
        /// Decide what to do with the list view depending on the completion status of the background image loader 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                
                if (e.Cancelled)
                {
                    imagesList_listView.BeginUpdate();

                    imagesList_listView.Items.Clear();
                    for (int i = 0; i < _listOfImages.Images.Count; i++)
                    {
                        _listOfImages.Images[i].Dispose();
                        _listOfImages.Images.RemoveAt(i);
                    }
                    _listOfImages.Images.Clear();
                    

                    imagesList_listView.EndUpdate();
                }
                else
                {
                    InitialListViewUpdate();
                }

                if (_shouldRestartWorker)
                {
                    RestartImageLoader?.Invoke(null,null);
                }
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to handle the background worker finishing its work");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to handle the background worker finishing its work");
                }
                throw exception;
            }
        }

        /// <summary>
        /// Dispose the options form if it is no longer needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisposeOptionsForm(object sender,EventArgs e)
        {
            var form = sender as OptionsForm;
            form?.Dispose();
        }

        /// <summary>
        /// Make a new options form and if the user wanted to move the profile files start a mover background worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void options_button_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("Options button clicked");
            try
            {
                var optionsForm = new OptionsForm(ref _options);

                optionsForm.fileMover_backgroundWorker.RunWorkerCompleted += DisposeOptionsForm;
            
                DialogResult result = optionsForm.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    optionsForm.fileMover_backgroundWorker.RunWorkerAsync(new Tuple<string,string> (optionsForm.StartProfilesLocations,optionsForm.DestinationProfilesLocations));
                }
                else
                {
                    optionsForm.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to change options");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to change options");
                    
                }
                throw exception;
            }
            
        }

        /// <summary>
        /// Show the context strip on click of the taskbar icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void taskbar_notifyIcon_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("taskbar icon button clicked");
            
            if (!(e is MouseEventArgs mEvent))
            {
                return;
            }
            
            if (mEvent.Button == MouseButtons.Right)
            {
                notifyIcon_main_contextMenuStrip.Show();
            }

        }

        /// <summary>
        /// Show the main form on double click of the taskbar icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void taskbar_notifyIcon_DoubleClick(object? sender, EventArgs e)
        {
            if (!(e is MouseEventArgs mEvent))
            {
                return;
            }

            if (mEvent.Button != MouseButtons.Left)
            {
                return;
            }
            //Console.WriteLine("should show form");
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }


        private void keyCounterMainFrame_frame_Resize(object sender, EventArgs e)
        {
            //Console.WriteLine("in resize");
            try
            {
                switch (WindowState)
                {
                    // if the form was minimized then do not show it in the apps bar, hide it from the alt+tab feature, stop the update of the 
                    // list view and stop the time update on the interface, also close all the info forms for keys
                    case FormWindowState.Minimized:
                        ProfileManager.RemoveHandlers(NewKeyPress_Handler,OldKeyPress_Handler,ListOfKeysAddedHandler,RemoveListOfKeysFromListView);
                        foreach (var form in _infoForms)
                        {
                            form.Close();
                            RemoveOwnedForm(form);
                            form.Dispose();
                        }
                        
                        _infoForms.Clear();
                        _shouldRestartWorker = false;
                        if (imageLoader_backgroundWorker.IsBusy)
                        {
                            imageLoader_backgroundWorker.CancelAsync();
                        }
                        ProfileManager.CurrentDevice.UnloadImages();


                        imagesList_listView.Items.Clear();

                        for (int i = 0; i < _listOfImages.Images.Count; i++)
                        {
                            _listOfImages.Images[i].Dispose();
                            _listOfImages.Images.RemoveAt(i);
                        }

                        _listOfImages.Images.Clear();


                        Interlocked.Exchange(ref _remainingSingleImageRestarts, 0);
                        Interlocked.Exchange(ref _remainingListOfImageRestarts, 0);

                        queueListsOfImagesYetHaveToCreate.Clear();
                        queueImagesYetHaveToCreate.Clear();

                        _getImageForOneKey_backgroundWorker.CancelAsync();
                        _getImagesForListOfKeys_backgroundWorker.CancelAsync();



                        //should also clear list view here
                        _clockTimer.Enabled = false;
                        this.ShowInTaskbar = false;
                        taskbar_notifyIcon.Visible = true;
                        this.Visible = false;
                        break;

                    // opposite of the other case
                    case FormWindowState.Normal:
                        ProfileManager.SetupDeviceHandlers(NewKeyPress_Handler,OldKeyPress_Handler,ListOfKeysAddedHandler,RemoveListOfKeysFromListView);
                        if (imageLoader_backgroundWorker.IsBusy)
                        {
                            imageLoader_backgroundWorker.CancelAsync();
                            _shouldRestartWorker = true;
                        }
                        else
                        {
                            imageLoader_backgroundWorker.RunWorkerAsync();
                        }
                        this.ShowInTaskbar = true;
                        _clockTimer.Enabled = true;
                        taskbar_notifyIcon.Visible = false;
                        this.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to show or hide the main window");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to show or hide the main window");
                    
                }
                throw exception;
            }
        }


        /// <summary>
        /// Save profile, remove hooks and write options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keyCounterMainFrame_frame_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //Console.WriteLine("cleaning up and closing form");
                ProfileManager.CurrentDevice.UnloadImages();
                ProfileManager.UpdateTime(DateTime.UtcNow - _startTime);
                ProfileManager.SaveCurrentProfile(_options.ProfilesLocation);

                KeyboardHookClass.DeleteKeyboardHook();
                MouseHookClass.DeleteMouseHook();

                GamepadHookClass.DestroyTimer();

                var profName = ProfileManager.GetName();
                if (profName != null)
                {
                    _options.LastUsedProfile = profName;
                }
            
                _options.WriteOptions();
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to close the app");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to close the app");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Close form when 'Exit' options used in taskbar menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseRequest_handler(object? sender, EventArgs e)
        {

            //Console.WriteLine("requested to close");
            this.Close();
        }

        /// <summary>
        /// Set profile combo box data source, currently selected profile, window state and re-enable the handler for profile changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keyCounterMainFrame_frame_Load(object sender, EventArgs e)
        {
            //Console.WriteLine("In load");
            try
            {
                profile_comboBox.DataSource = _options.ProfileList;
                profile_comboBox.SelectedIndex = -1;
                profile_comboBox.SelectedIndexChanged += profile_comboBox_SelectedIndexChanged;
                
                // select profile depending on user preferences
                if (_options.UseLastProfile)
                {
                    profile_comboBox.SelectedItem = _options.LastUsedProfile ?? _options.ProfileList[0];
                }
                else
                {
                    profile_comboBox.SelectedItem = _options.OnStartProfile ?? _options.ProfileList[0];
                }

                if (_options.StartMinimized)
                {
                    WindowState = FormWindowState.Minimized;
                }
                else
                {
                    _clockTimer.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While in load");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While in load");
                    
                }
                throw exception;
            }

        }

        /// <summary>
        /// Close all key info windows, select new device, update handlers for said device, make background worker load images for new device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputDevice_comboBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (var form in _infoForms)
                {
                    form.Close();
                    RemoveOwnedForm(form);
                    form.Dispose();
                }
                _infoForms.Clear();

                //Console.WriteLine("device changed");

                var device = inputDevice_comboBox.SelectedItem == null ? "Keyboard" : inputDevice_comboBox.SelectedItem.ToString();
            
                ProfileManager.RemoveHandlers(NewKeyPress_Handler,OldKeyPress_Handler,ListOfKeysAddedHandler,RemoveListOfKeysFromListView);
                if (device == null)
                {
                    return;
                }

                ProfileManager.ChangeDevice(device);
                ProfileManager.SetupDeviceHandlers(NewKeyPress_Handler,OldKeyPress_Handler,ListOfKeysAddedHandler,RemoveListOfKeysFromListView);

                if (WindowState == FormWindowState.Normal)
                {
                    if (imageLoader_backgroundWorker.IsBusy)
                    {
                        ProfileManager.RemoveHandlers(NewKeyPress_Handler,OldKeyPress_Handler,ListOfKeysAddedHandler,RemoveListOfKeysFromListView);
                        imageLoader_backgroundWorker.CancelAsync();
                        // since cancelling the worker might take some time raise raise a flag letting the worker know when it finishes that it 
                        // should restart
                        _shouldRestartWorker = true;
                    }
                    else
                    {
                        ProfileManager.CurrentDevice.UnloadImages();

                        imageLoader_backgroundWorker.RunWorkerAsync();
                    }
                
                }

                imagesList_listView.Focus();

            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to switch device type in main form");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to switch device type in main form");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Switch the current profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profile_comboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            //Console.WriteLine("profile changed");
            try
            {
                // don't change the profile if the same profile was selected
                var profName = ProfileManager.GetName();
                if ( profName != null && profName.Equals(profile_comboBox.SelectedItem.ToString()))
                {
                    return;
                }

                GamepadHookClass.StopTimer();
                ProfileManager.UpdateTime(DateTime.UtcNow - _startTime);
                ProfileManager.ChangeProfile(profile_comboBox.SelectedItem.ToString(),_options.ProfilesLocation);

                //start gamepad monitoring if the profile is supposed to 
                var usesGamepad = ProfileManager.UsesGamepad();
                if (usesGamepad.HasValue && usesGamepad.Value)
                {
                    GamepadHookClass.SetUpTimer();
                    GamepadHookClass.StartTimer();
                }

                inputDevice_comboBox.DataSource = ProfileManager.GetListOfDevices();
                if (profile_comboBox.SelectedItem != null)
                { 
                    _renderer.SelectedProfile = profile_comboBox.SelectedItem.ToString();
                }
                
                _startTime = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to switch profile in main form");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to switch profile in main form");
                }
                throw exception;
            }
            
        }

        private void clearProfile_button_Click(object sender, EventArgs e)
        {
            ProfileManager.ClearCurrentDeviceDictionary();
        }

        /// <summary>
        /// Open key info windows for all selected keys in the list view on right click, also add them to a list
        /// to keep track of them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imagesList_listView_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                foreach (ListViewItem? item in imagesList_listView.SelectedItems)
                {
                    if (item == null|| !_listOfImages.Images.ContainsKey(item.ImageKey)  || _listOfImages.Images[item.ImageKey] == null)
                    {
                        continue;
                    }

                    _infoForms.Add(new KeyInfo
                    {
                        KeyImage_pictureBox = {Image = (Image) _listOfImages.Images[item.ImageKey]?.Clone()},
                        Text = item.Name,
                        KeyName_textBox = {Text = item.Name},
                        Owner = this
                    });

                    //show the last added form
                    _infoForms[^1].Show();
                }
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to open info forms");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to open info forms");
                    
                }
                throw exception;
            }
        }


        /// <summary>
        /// Renames the appropriate key and changes its icon
        /// </summary>
        /// <param name="previousKeyName"></param>
        /// <param name="newKeyName"></param>
        /// <param name="newIcon"></param>
        public void RenameAndChangeIcon(string previousKeyName, string newKeyName, Image? newIcon)
        {

            try
            {
                // determine the original name of the key, also remove from the rename dictionary the 
                // key if it is renamed to itself
                string originalKeyName = previousKeyName;
                if (ProfileManager.CurrentDevice.RenamesContainsValue(previousKeyName))
                {
                    foreach (var renamedKey in ProfileManager.CurrentDevice.KeysRename.Keys)
                    {
                        if (ProfileManager.CurrentDevice.KeysRename[renamedKey] == previousKeyName)
                        {
                            if (renamedKey != newKeyName)
                            {
                                ProfileManager.CurrentDevice.KeysRename[renamedKey] = newKeyName;
                            }
                            else
                            {
                                ProfileManager.CurrentDevice.KeysRename.Remove(renamedKey);
                            }
                           
                            originalKeyName = renamedKey;
                            break;
                        }
                    }

                }
                else
                {
                    if (previousKeyName != newKeyName)
                    {
                        ProfileManager.CurrentDevice.KeysRename.Add(previousKeyName,newKeyName);
                    }
                    else
                    {
                        if (ProfileManager.CurrentDevice.RenamesContainsKey(previousKeyName))
                        {
                            ProfileManager.CurrentDevice.KeysRename.Remove(previousKeyName);
                        }
                    }
                    
                }


                if (newIcon!= null)
                {
                    ProfileManager.CurrentDevice.AddOrReplace(newKeyName,newIcon);
                    ProfileManager.CurrentDevice.RemoveImage(originalKeyName);
                    newIcon.Dispose();
                }
                else
                {
                    ProfileManager.CurrentDevice.AddOrReplace(newKeyName,null);
                }

                // would load the image using a worker but that would put the new image at the end of the list, this instead replaces the image in place.
                // could be made to replace in place even using the worker, but it would need 1 or 2 new functions, maybe done at a later date...
                imagesList_listView.BeginUpdate();
                imagesList_listView.Items.RemoveByKey(previousKeyName);
                _listOfImages.Images[previousKeyName]?.Dispose();
                _listOfImages.Images.RemoveByKey(previousKeyName);

                _listOfImages.Images.Add(newKeyName, ProfileManager.CurrentDevice.GetOrCreateImageForKey(newKeyName) ?? throw new NullReferenceException("Image was null whn trying to rename and change"));

                imagesList_listView.Items.Add(newKeyName, ProfileManager.CurrentDevice.KeysCount.Keys[originalKeyName].ToString(), newKeyName);
                imagesList_listView.EndUpdate();
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to change name and icon for a key");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying to change name and icon for a key");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newProfileName"></param>
        /// <returns>true if the given profile name would be valid, false otherwise</returns>
        public bool IsProfileNameValid(string newProfileName)
        {
            foreach (var profileName in _options.ProfileList)
            {
                if (profileName == newProfileName)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Create a new 'new profile' window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newProfile_button_Click(object sender, EventArgs e)
        {
            var newProfileForm = new AddProfile {Owner = this};
            newProfileForm.ShowDialog();
            newProfileForm.Dispose();
            newProfileForm = null;
        }


        /// <summary>
        /// Create new profile file and update options to reflect changes
        /// </summary>
        /// <param name="newProfileName"></param>
        /// <param name="usesGamepad"></param>
        public void CreateNewProfile(string newProfileName, bool usesGamepad)
        {
            if (ProfileManager.CreateNewProfile(_options.ProfilesLocation, newProfileName, usesGamepad))
            {
                _options.ProfileList.Add(newProfileName);
                UpdateContextMenuProfiles(_options.ProfileList);
                profile_comboBox.SelectedItem = _options.ProfileList[^1] ?? _options.ProfileList[0];
            }

        }

        /// <summary>
        /// Deletes a profile and its corresponding file and changes options to reflect this action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteProfile_button_Click(object sender, EventArgs e)
        {
            try
            {
                var profileToDelete = profile_comboBox.SelectedItem.ToString();
                if (profileToDelete != "Default")
                {
                    profile_comboBox.SelectedItem = _options.ProfileList[0];
                    if (File.Exists(Path.Join(_options.ProfilesLocation,"/"+profileToDelete+".txt")))
                    {
                        File.Delete(Path.Join(_options.ProfilesLocation,"/"+profileToDelete+".txt"));
                    }

                    if (profileToDelete != null)
                    {
                        _options.ProfileList.Remove(profileToDelete);
                        UpdateContextMenuProfiles(_options.ProfileList);
                    }
                    
                }
                else
                {
                    MessageBox.Show("THE \"DEFAULT\" PROFILE CANNOT BE DELETED", "Delete Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                if (ex is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying delete a profile");
                }
                else
                {
                    exception = new ChainingException(ex.Message);
                    exception.AddErrorToChain("While trying delete a profile");
                    
                }
                throw exception;
            }
        }
    }
}
