using System.Drawing;
using System.Text;

namespace KeyCounter
{
    
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

        public void ReplaceImage(Image image)
        {
            Image?.Dispose();
            this.Image = image;
        }

        public void AddOne()
        {
            this.Number += 1;
        }
    }


}
