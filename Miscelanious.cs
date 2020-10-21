using System.Drawing;

namespace KeyCounter
{
    /// <summary>
    /// Class for Miscelanoius operations that had no place in other classes
    /// </summary>
    public static class Miscelanious
    {
        /// <summary>
        /// Verify if a value exists in a vector
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns>true if the value is in vector, false otherwise</returns>
        public static bool IsInVector(object[] vector, object value)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i].Equals(value))
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Class having two proprieties, a <c>uint </c>and an <c>Image</c>
    /// </summary>
    public class CustomPair
    {
        public uint Number { get; set; }
        public Image Image;

        /// <summary>
        /// Create a <c>CustomPair</c> with <paramref name="image"/> and <paramref name="number"/> values
        /// </summary>
        /// <param name="number">a uint representing a key count</param>
        /// <param name="image">an image representing the image for the key</param>
        public CustomPair(uint number, Image image)
        {
            this.Number = number;
            this.Image = image;
        }

        /// <summary>
        /// Create a Custom pair with default values
        /// </summary>
        public CustomPair()
        {
            this.Number = 0;
            this.Image = null;
        }
    }


}
