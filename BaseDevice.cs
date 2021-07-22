using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KeyCounter
{
    internal class BaseDevice : KeysImages
    {
        [JsonInclude]
        public DictionaryWithEvents KeysCount = new DictionaryWithEvents();
        [JsonInclude]
        public Dictionary<string,string> KeysRename { get; set; } = new Dictionary<string, string>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns>value at index in the key counts dictionary</returns>
        public uint this[string index] => KeysCount[index];

        /// <summary>
        /// 
        /// </summary>
        /// <returns>An enumerator over the key counts dictionary</returns>
        public IEnumerator<KeyValuePair<string, uint>> GetEnumerator()
        {
            return KeysCount.GetEnumerator();
        }


        /// <summary>
        /// Increment the count of the key if it exists, else adds it to the dictionary of keys with a count of 1
        /// </summary>
        /// <param name="key"></param>
        public void AddOne(string key)
        {
            KeysCount.AddOne(key);
        }


        /// <summary>
        /// For each key in the list increment its count if it exists, else adds it to the dictionary of keys with a count of 1
        /// </summary>
        /// <param name="keys"></param>
        public void AddList(List<string> keys)
        {
            KeysCount.AddList(keys);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if the key is in the list of keys of the rename dictionary, false otherwise</returns>
        public bool RenamesContainsKey(string key)
        {
            return KeysRename.ContainsKey(key);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true if the value is in the list of values of the rename dictionary, false otherwise</returns>
        public bool RenamesContainsValue(string value)
        {
            return KeysRename.ContainsValue(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>true if the text is in either the images dictionary or templates dictionary</returns>
        public bool ImagesContainKey(string text)
        {
            return Images.ContainsKey(text) || Templates.ContainsKey(text);
        }
    }
}
