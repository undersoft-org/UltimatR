﻿
// <copyright file="ByteArrayMarkupExtension.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The IO namespace.
/// </summary>
namespace System.IO
{



    /// <summary>
    /// Class ByteArrayMarkupExtension.
    /// </summary>
    public static class ByteArrayMarkupExtension
    {
        #region Methods







        /// <summary>
        /// Initializes the specified default value.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] Initialize(this byte[] array, byte defaultValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = defaultValue;
            }
            return array;
        }







        /// <summary>
        /// Initializes the specified default value.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] Initialize(this byte[] array, char defaultValue)
        {
            byte[] charbyte = BitConverter.GetBytes(defaultValue);
            int charlength = charbyte.Length;
            int length = (array.Length % charlength) + array.Length;
            int counter = 0;
            for (int i = 0; i < length; i++)
            {
                array[i] = charbyte[counter];
                counter = (charlength > counter) ? counter++ : 0;
            }
            return array;
        }










        /// <summary>
        /// Markups the specified blocksize.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="blocksize">The blocksize.</param>
        /// <param name="bytenoise">The bytenoise.</param>
        /// <param name="written">The written.</param>
        /// <param name="byteprefix">The byteprefix.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] Markup(this byte[] array, int blocksize, MarkupType bytenoise, int written = 0, int byteprefix = 0)
        {
            int arrayLength = (written > 0) ? written : array.Length;
            int blockSize = blocksize;
            long blockLeft = arrayLength % blockSize;
            long resize = (blockSize - blockLeft >= 28) ? blockSize - blockLeft - byteprefix : blockSize + (blockSize - blockLeft) - byteprefix;
            byte[] byteMarkup = new byte[resize].Initialize((byte)bytenoise);
            Array.Resize<byte>(ref array, arrayLength + (int)resize);
            byteMarkup.CopyTo(array, arrayLength);
            return array;
        }










        /// <summary>
        /// Markups the specified blocksize.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="blocksize">The blocksize.</param>
        /// <param name="bytenoise">The bytenoise.</param>
        /// <param name="written">The written.</param>
        /// <param name="byteprefix">The byteprefix.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] Markup(this byte[] array, long blocksize, MarkupType bytenoise, int written = 0, int byteprefix = 0)
        {
            int arrayLength = (written > 0) ? written : array.Length;
            long blockSize = blocksize;
            long blockLeft = arrayLength % blockSize;
            long resize = (blockSize - blockLeft >= 28) ? blockSize - blockLeft - byteprefix : blockSize + (blockSize - blockLeft) - byteprefix;
            byte[] byteMarkup = new byte[resize].Initialize((byte)bytenoise);
            Array.Resize<byte>(ref array, arrayLength + (int)resize);
            byteMarkup.CopyTo(array, arrayLength);
            byteMarkup = null;
            return array;
        }










        /// <summary>
        /// Seeks the markup.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="position">The position.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="_length">The length.</param>
        /// <returns>MarkupType.</returns>
        public static MarkupType SeekMarkup(this byte[] array, out long position, SeekDirection direction = SeekDirection.Forward, int offset = 0, int _length = -1)
        {
            bool isFwd = (direction != SeekDirection.Forward) ? false : true;
            short noiseFlag = 0;
            MarkupType lastKind = MarkupType.None;
            if (array.Length > 0)
            {
                long length = (_length <= 0 || _length > array.Length) ? array.Length : _length;
                int arraylength = array.Length;
                offset += (!isFwd) ? 1 : 0;
                length -= ((!isFwd) ? 0 : 1);

                for (int i = offset; i < length; i++)
                {
                    byte checknoise = 0;
                    if (!isFwd)
                        checknoise = array[arraylength - i];
                    else
                        checknoise = array[i];

                    if (checknoise.IsMarkup(out MarkupType tempKind))
                    {
                        lastKind = tempKind;
                        noiseFlag++;
                    }
                    else if (noiseFlag >= 16)
                    {
                        position = (!isFwd) ? arraylength - i + 1 : i;
                        return lastKind;
                    }
                    else
                    {
                        lastKind = tempKind;
                        noiseFlag = 0;
                    }
                }
            }
            position = (!isFwd && noiseFlag != 0) ? array.Length - noiseFlag + 1 : 0;
            return lastKind;
        }







        /// <summary>
        /// Seeks the segment.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="itemSize">Size of the item.</param>
        /// <returns>MarkedSegment.</returns>
        public static MarkedSegment SeekSegment(this byte[] array, int itemSize = 0)
        {
            int noiseCount = (array.Length > 4096) ? 1024 : 4096;

            long endPosition = array.Length;

            MarkupType endNoise = array.SeekMarkup(out long endTempPosition, SeekDirection.Backward, 0, noiseCount);
            if (endNoise != MarkupType.None)
                endPosition = endTempPosition;

            long startPosition = 0;

            MarkupType startNoise = array.SeekMarkup(out long startTempPosition, SeekDirection.Forward, 0, noiseCount);
            if (startNoise != MarkupType.None)
                startPosition = startTempPosition;

            return new MarkedSegment() { Offset = startPosition, Length = endPosition - startPosition, ItemSize = itemSize };
        }

        #endregion
    }
}