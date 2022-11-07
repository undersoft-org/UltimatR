/// <summary>
/// The System namespace.
/// </summary>
namespace System
{
    using System.Text;


    /// <summary>
    /// Class HexEncodingExtension.
    /// </summary>
    public static class HexEncodingExtension
    {
        #region Methods


        /// <summary>
        /// Froms the hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <returns>Byte[].</returns>
        public static Byte[] FromHex(this String hex)
        {
            return hexToByte(hex);
        }


        /// <summary>
        /// Converts to hex.
        /// </summary>
        /// <param name="ba">The ba.</param>
        /// <param name="trim">if set to <c>true</c> [trim].</param>
        /// <returns>String.</returns>
        public static String ToHex(this Byte[] ba, bool trim = false)
        {
            return byteToHex(ba, trim);
        }


        /// <summary>
        /// Bytes to hexadecimal.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="trim">if set to <c>true</c> [trim].</param>
        /// <returns>String.</returns>
        private static String byteToHex(Byte[] bytes, bool trim = false)
        {
            StringBuilder s = new StringBuilder();
            int length = bytes.Length;
            if (trim)
            {
                foreach (byte b in bytes)
                    if (b == 0)
                        length--;
                    else break;
            }
            for (int i = 0; i < length; i++)
                s.Append(bytes[i].ToString("x2").ToUpper());
            return s.ToString();
        }


        /// <summary>
        /// Gets the hexadecimal.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>Byte.</returns>
        private static Byte getHex(Char x)
        {
            if (x <= '9' && x >= '0')
            {
                return (byte)(x - '0');
            }
            else if (x <= 'z' && x >= 'a')
            {
                return (byte)(x - 'a' + 10);
            }
            else if (x <= 'Z' && x >= 'A')
            {
                return (byte)(x - 'A' + 10);
            }
            return 0;
        }


        /// <summary>
        /// Hexadecimals to byte.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <param name="length">The length.</param>
        /// <returns>Byte[].</returns>
        private static Byte[] hexToByte(String hex, int length = -1)
        {
            if (length < 0)
                length = hex.Length;
            byte[] r = new byte[length / 2];
            for (int i = 0; i < length - 1; i += 2)
            {
                byte a = getHex(hex[i]);
                byte b = getHex(hex[i + 1]);
                r[i / 2] = (byte)(a * 16 + b);
            }
            return r;
        }

        #endregion
    }
}
