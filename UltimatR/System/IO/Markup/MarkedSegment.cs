/// <summary>
/// The IO namespace.
/// </summary>
namespace System.IO
{
    #region Enums

    /// <summary>
    /// Enum MarkupType
    /// </summary>
    public enum MarkupType
    {
        /// <summary>
        /// The none
        /// </summary>
        None = (byte)0xFF,
        /// <summary>
        /// The block
        /// </summary>
        Block = (byte)0x17,
        /// <summary>
        /// The end
        /// </summary>
        End = (byte)0x04,
        /// <summary>
        /// The empty
        /// </summary>
        Empty = (byte)0x00,
        /// <summary>
        /// The line
        /// </summary>
        Line = (byte)0x10,
        /// <summary>
        /// The space
        /// </summary>
        Space = (byte)0x32,
        /// <summary>
        /// The semi
        /// </summary>
        Semi = (byte)0x59,
        /// <summary>
        /// The coma
        /// </summary>
        Coma = (byte)0x44,
        /// <summary>
        /// The colon
        /// </summary>
        Colon = (byte)0x58,
        /// <summary>
        /// The dot
        /// </summary>
        Dot = (byte)0x46,
        /// <summary>
        /// The cancel
        /// </summary>
        Cancel = (byte)0x18,
    }
    /// <summary>
    /// Enum SeekDirection
    /// </summary>
    public enum SeekDirection
    {
        /// <summary>
        /// The forward
        /// </summary>
        Forward,
        /// <summary>
        /// The backward
        /// </summary>
        Backward
    }

    #endregion




    /// <summary>
    /// Struct MarkedSegment
    /// </summary>
    public struct MarkedSegment
    {
        #region Fields

        /// <summary>
        /// The item size
        /// </summary>
        public int ItemSize;
        /// <summary>
        /// The length
        /// </summary>
        public long Length;
        /// <summary>
        /// The offset
        /// </summary>
        public long Offset;

        #endregion

        #region Properties




        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => (int)(Length / ItemSize);

        #endregion

        #region Methods






        /// <summary>
        /// Items the offset.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>System.Int64.</returns>
        public long ItemOffset(int index)
        {
            return (Offset + (index * ItemSize));
        }

        #endregion
    }
}
