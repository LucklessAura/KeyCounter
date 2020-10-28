using System.Collections.Generic;
using System.Drawing;

namespace KeyCounter
{
    /// <summary>
    /// Dictionary class that raises events when items are added, modified or removed
    /// </summary>
    public class DictionaryWithEvents
    {
        public Dictionary<string, CustomPair> Dictionary { get; set; }

        public delegate void DictionaryEvent();
        public event DictionaryEvent OnUpdateStatus;
        
        public event DictionaryEvent OnAddStatus;

        public event DictionaryEvent OnInitialLoadStatus;

        private string _lastIntroducedKey = "";
        private string _lastUpdatedKey = "";

        private bool _shouldRaiseEvents = false;

        /// <summary>
        /// Create new dictionary with events
        /// </summary>
        public DictionaryWithEvents()
        {
            Dictionary = new Dictionary<string, CustomPair>();
        }

        /// <summary>
        /// Add to the dictionary the <paramref name="key"/> with a corresponding <c>CustomPair</c> formed from the <paramref name="image"/>
        /// and <paramref name="value"/>
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="image">image corresponding to the key</param>
        /// <param name="value">number of presses</param>
        public void Add(string key, Image image, uint value)
        {

            Dictionary.Add(key, new CustomPair(value, image));
            _lastIntroducedKey = key;
            if (this._shouldRaiseEvents)
            {
                OnAddStatus();
            }
        }

        /// <summary>
        /// Get the enumerator for the dictionary
        /// </summary>
        /// <returns>An enumerator for the dictionary</returns>
        public Dictionary<string, CustomPair>.Enumerator GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        /// <summary>
        /// Verify if a key is in the dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if the key exists in the dictionary false otherwise</returns>
        public bool ContainsKey(string key)
        {
            return Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Remove the key from the dictionary, raises the <c>OnUpdateStatus</c> event if the operation is successful and the 
        /// should raise events flag is true
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if the operation was successful, false otherwise</returns>
        public bool Remove(string key)
        {

            bool result = Dictionary.Remove(key);
            if (this._shouldRaiseEvents == true && result == true)
            {
                OnUpdateStatus();
            }
            return result;
        }

        /// <summary>
        /// Clears the content of the dictionary, 
        /// if the should raise event flag is true it raises the <c>OnInitialLoadStatus</c> event
        /// </summary>
        public void Clear()
        {

            Dictionary.Clear();
            if (this._shouldRaiseEvents == true)
            {
                OnInitialLoadStatus();
            }
        }

        /// <summary>
        /// Adds one to the count of the key,
        /// if the should raise events flag is true raises the event <c>OnUpdateStatus</c>
        /// </summary>
        /// <param name="key"></param>
        public void AddOne(string key)
        {
            _lastUpdatedKey = key;
            Dictionary[key].AddOne();
            if (this._shouldRaiseEvents == true)
            {
                OnUpdateStatus();
            }
            
        }

        /// <summary>
        /// Get the last added key in the dictionary
        /// </summary>
        /// <returns>return the last key added</returns>
        public string GetLastAddedKey()
        {
            return this._lastIntroducedKey;
        }

        /// <summary>
        /// Get the last updated key in the dictionary
        /// </summary>
        /// <returns>return the last updated key</returns>
        public string GetLastUpdatedKey()
        {
            return this._lastUpdatedKey;
        }

        /// <summary>
        /// if the should raise events flag is set to true it raises the event <c>OnInitialLoadStatus</c>
        /// </summary>
        public void InitialLoad()
        {
            if (this._shouldRaiseEvents == true)
            {
                OnInitialLoadStatus();
            }
        }

        /// <summary>
        /// set the should raise events flag to true
        /// </summary>
        public void EnableEvents()
        {
            this._shouldRaiseEvents = true;
        }

        /// <summary>
        /// set the should raise events flag to false
        /// </summary>
        public void DisableEvents()
        {
            this._shouldRaiseEvents = false;
        }

        
    }

}
