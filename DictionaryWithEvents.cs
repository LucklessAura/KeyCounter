using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KeyCounter
{
    /// <summary>
    /// Dictionary that raises events when items are added, modified, removed or updated
    /// </summary>
    public class DictionaryWithEvents
    {
        [JsonInclude]
        public ConcurrentDictionary<string, uint> Keys { get; private set; }
        

        public delegate void DictionaryEvent(object sender, DictionaryEventArgs e);
        


        public event DictionaryEvent? OnUpdateStatus;
        public event DictionaryEvent? OnAddStatus;
        public event DictionaryEvent? OnAddList;
        public event DictionaryEvent? OnRemoveList;

        [JsonIgnore]
        public bool HasSubscribers;

        public uint this[string index] => Keys[index];

        public DictionaryWithEvents()
        {
            Keys = new ConcurrentDictionary<string, uint>();
            HasSubscribers = false;
        }

        /// <summary>
        /// Add to the dictionary the <paramref name="key"/> with a initialized with <paramref name="value"/>.
        /// Raises the OnAddStatus event
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="value">number of presses</param>
        public void Add(string key, uint value)
        {
            try
            {
                Keys.GetOrAdd(key, value);
                OnAddStatus?.Invoke(this,new DictionaryEventArgs(key,false));
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to add value in the count dictionary");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to add value in the count dictionary");
                    
                }
                throw exception;
            }
        }


        /// <summary>
        /// Get the enumerator for the dictionary
        /// </summary>
        /// <returns>An enumerator for the dictionary</returns>
        public  IEnumerator<KeyValuePair<string,uint>> GetEnumerator()
        {
            return Keys.GetEnumerator();
        }

        /// <summary>
        /// Verify if a key is in the dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if the key exists in the dictionary false otherwise</returns>
        public bool ContainsKey(string key)
        {
            return Keys.ContainsKey(key);
        }

        /// <summary>
        /// Remove the key from the dictionary
        /// Raises the event OnUpdateStatus
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if the operation was successful, false otherwise</returns>
        public bool Remove(string key)
        {
           
            try
            {
                bool result = Keys.TryRemove(key,out _);
                OnUpdateStatus?.Invoke(this,new DictionaryEventArgs(key,true));
                return result;
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to remove key from count dictionary");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to remove key from count dictionary");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Clears the content of the dictionary. 
        /// Raises the OnRemoveList envent.
        /// </summary>
        public void Clear()
        {
            try
            {
                List<string>? keys = Keys.Keys as List<string>;
                Keys.Clear();
                if (keys != null)
                {
                    OnRemoveList?.Invoke(this, new DictionaryEventArgs(keys, true));
                }
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to clear the count dictionary");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to clear the count dictionary");
                    
                }
                throw exception;
            }
        }

        /// <summary>
        /// Adds one to the count of the key, if they are present in the dictionary, else initialize the key with 1.
        ///  Raises the event OnAddStatus if the key was not present and OnUpdateStatus if the key was already in the dictionary.
        /// </summary>
        /// <param name="key"></param>
        public void AddOne(string key)
        {
            
            //Console.WriteLine(key);
            try
            {
                if (Keys.ContainsKey(key))
                {
                    Keys[key]++;
                    OnUpdateStatus?.Invoke(this,new DictionaryEventArgs(key,false));
                }
                else
                {
                    this.Add(key,1);
                }
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to increment or create entry in count dictionary");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to increment or create entry in count dictionary");
                    
                }
                throw exception;
            }

        }

        /// <summary>
        /// Remove the given list from the dictionary.
        /// Raises the OnRemoveList event
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveList(List<string> keys)
        {
            try
            {
                foreach (var key in keys)
                {
                    Keys.TryRemove(key, out _);
                }

                OnRemoveList?.Invoke(this, new DictionaryEventArgs(keys, true));
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to remove key from count dictionary");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to remove key from count dictionary");
                    
                }
                throw exception;
            }
            
        }


        /// <summary>
        /// Add to the count of each key 1 or if they are not in the dictionary initialize them with 1.
        /// Raises the event OnAddList
        /// </summary>
        public void AddList(List<string> keys)
        {
            try
            {
                foreach (var key in keys)
                {
                
                    if (Keys.ContainsKey(key))
                    {
                        Keys[key] += 1;
                    }
                    else
                    {
                        Keys.GetOrAdd(key, 1);
                    }
                }
                OnAddList?.Invoke(this,new DictionaryEventArgs(keys,false));
            }
            catch (Exception e)
            {
                if (e is ChainingException exception)
                {
                    exception.AddErrorToChain("While trying to add a list of keys to the count dictionary");
                }
                else
                {
                    exception = new ChainingException(e.Message);
                    exception.AddErrorToChain("While trying to add a list of keys to the count dictionary");
                    
                }
                throw exception;
            }
        }

    }

    public class DictionaryEventArgs : EventArgs
    {
        public string? Key;
        public bool Removed;
        public List<string>? Keys;
        public bool  IsList;
        public DictionaryEventArgs(string key,bool removed)
        {
            Key = key;
            Removed = removed;
            IsList = false;
        }

        public DictionaryEventArgs(List<string> key,bool removed)
        {
            Keys = key;
            Removed = removed;
            IsList = true;
        }
    }


}
