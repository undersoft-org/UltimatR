/// <summary>
/// The Uniques namespace.
/// </summary>
namespace System.Uniques
{
    using System.Extract;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Struct Ursn
    /// Implements the <see cref="System.IFormattable" />
    /// Implements the <see cref="System.IComparable" />
    /// Implements the <see cref="System.IComparable{System.Uniques.Ursn}" />
    /// Implements the <see cref="System.IEquatable{System.Uniques.Ursn}" />
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IFormattable" />
    /// <seealso cref="System.IComparable" />
    /// <seealso cref="System.IComparable{System.Uniques.Ursn}" />
    /// <seealso cref="System.IEquatable{System.Uniques.Ursn}" />
    /// <seealso cref="System.IUnique" />
    [Serializable]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 24)]
    public unsafe struct Ursn : IFormattable, IComparable
        , IComparable<Ursn>, IEquatable<Ursn>, IUnique
    {
        /// <summary>
        /// The bytes
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        private byte[] bytes;

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public ulong UniqueKey
        {
            get
            {
                if (IsNull) return 0;
                fixed (byte* pbyte = bytes)
                    return *((ulong*)pbyte);

            }
            set
            {
                fixed (byte* b = SureBytes)
                    *((ulong*)b) = value;
            }
        }

        /// <summary>
        /// Gets or sets the key block x.
        /// </summary>
        /// <value>The key block x.</value>
        public ulong KeyBlockX
        {
            get
            {
                if (IsNull) return 0;
                fixed (byte* pbyte = &bytes[8])
                    return *((ulong*)pbyte);
            }
            set
            {
                fixed (byte* b = &SureBytes[8])
                    *((ulong*)b) = value;
            }
        }

        /// <summary>
        /// Gets or sets the key block y.
        /// </summary>
        /// <value>The key block y.</value>
        public ulong KeyBlockY
        {
            get
            {
                if (IsNull) return 0;
                fixed (byte* pbyte = &bytes[16])
                    return *((ulong*)pbyte);
            }
            set
            {
                fixed (byte* b = &SureBytes[16])
                    *((ulong*)b) = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ursn" /> struct.
        /// </summary>
        /// <param name="l">The l.</param>
        public Ursn(ulong l)
        {
            bytes = new byte[24];
            fixed (byte* b = bytes)
            {
                *((ulong*)b) = l;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ursn" /> struct.
        /// </summary>
        /// <param name="s">The s.</param>
        public Ursn(string s)
        {
            bytes = new byte[24];
            this.FromHexTetraChars(s.ToCharArray());    
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ursn" /> struct.
        /// </summary>
        /// <param name="b">The b.</param>
        public Ursn(byte[] b)
        {
            bytes = new byte[24];
            if (b != null)
            {
                int l = b.Length;
                if (l > 24)
                    l = 24;
                b.CopyBlock(bytes, (uint)l);
            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ursn" /> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Ursn(ulong x, ulong y, ulong z)
        {
            bytes = new byte[24];

            fixed (byte* n = bytes)
            {
                *((ulong*)n) = x;
                *((ulong*)&n[8]) = y;
                *((ulong*)&n[16]) = z;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ursn" /> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Ursn(byte[] x, byte[] y, byte[] z)
        {
            bytes = new byte[24];
            fixed (byte* n = bytes)
            {
                fixed (byte* s = x)
                    *((ulong*)n) = *((ulong*)s);
                fixed (byte* s = y)
                    *((ulong*)(n + 8)) = *((ulong*)s);
                fixed (byte* s = z)
                    *((ulong*)(n + 16)) = *((ulong*)s);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ursn" /> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Ursn(object x, object[] y, object[] z)
        {
            bytes = new byte[24];

            fixed (byte* n = bytes)
            {
                fixed (byte* s = x.UniqueBytes64())
                    *((ulong*)n) = *((ulong*)s);
                fixed (byte* s = y.UniqueBytes64())
                    *((ulong*)(n + 12)) = *((ulong*)s);
                fixed (byte* s = z.UniqueBytes64())
                    *((ulong*)(n + 16)) = *((ulong*)s);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Byte[]" /> with the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] this[int offset]
        {
            get
            {
                if (offset != 0)
                {
                    byte[] r = new byte[24 - offset];
                    fixed (byte* pbyte = &NewOrBytes[offset])
                    fixed (byte* rbyte = r)
                    {
                        Extractor.CopyBlock(rbyte, pbyte, (uint)(24 - offset));
                    }
                    return r;
                }
                return NewOrBytes;
            }
            set
            {
                int l = value.Length;
                if (offset != 0 || l < 24)
                {
                    int count = 24 - offset;
                    if (l < count)
                        count = l;
                    value.CopyBlock(SureBytes, (uint)offset, (uint)count);
                }
                else
                {
                    value.CopyBlock(SureBytes, 0, 24);
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="System.Byte[]" /> with the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] this[int offset, int length]
        {
            get
            {
                if (offset < 24)
                {
                    if ((24 - offset) > length)
                        length = 24 - offset;

                    byte[] r = new byte[length];
                    fixed (byte* pbyte = &NewOrBytes[offset])
                    fixed (byte* rbyte = r)
                    {
                        Extractor.CopyBlock(rbyte, pbyte, (uint)length);
                    }
                    return r;
                }
                return null;

            }
            set
            {
                if (offset < 24)
                {
                    if ((24 - offset) > length)
                        length = 24 - offset;
                    if (value.Length < length)
                        length = value.Length;

                    byte[] r = new byte[length];
                    fixed (byte* pbyte = value)
                    fixed (byte* rbyte = &SureBytes[offset])
                    {
                        Extractor.CopyBlock(rbyte, pbyte, (uint)length);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        public void SetBytes(byte[] value, int offset)
        {
            this[offset] = value;
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] GetBytes(int offset, int length)
        {
            return this[offset, length];
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetBytes()
        {
            return SureBytes;
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetUniqueBytes()
        {
            byte[] kbytes = new byte[8];
            fixed (byte* b = SureBytes)
            fixed (byte* k = kbytes)
                *((ulong*)k) = *((ulong*)b);
            return kbytes;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is null.
        /// </summary>
        /// <value><c>true</c> if this instance is null; otherwise, <c>false</c>.</value>
        public bool IsNull
        {
            get
            {
                if (bytes == null)
                    return true;
                return false;
            }
            set
            {
                if (value) bytes = null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is not empty.
        /// </summary>
        /// <value><c>true</c> if this instance is not empty; otherwise, <c>false</c>.</value>
        public bool IsNotEmpty
        {
            get { return (!IsNull && UniqueKey != 0); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return (IsNull || UniqueKey == 0); }
        }

        /// <summary>
        /// Gets the sure bytes.
        /// </summary>
        /// <value>The sure bytes.</value>
        public byte[] SureBytes
        {
            get => (bytes == null) ? bytes = new byte[24] : bytes;
        }

        /// <summary>
        /// Creates new orbytes.
        /// </summary>
        /// <value>The new or bytes.</value>
        public byte[] NewOrBytes
        {
            get => (bytes == null) ? new byte[24] : bytes;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            fixed (byte* pbyte = &this[0, 8].BitAggregate64to32()[0])
                return *((int*)pbyte);
        }

        /// <summary>
        /// Sets the unique key.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetUniqueKey(ulong value)
        {
            UniqueKey = value;
        }

        /// <summary>
        /// Gets the unique key.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        public ulong GetUniqueKey()
        {
            return UniqueKey;
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.Logs.ILogSate.Exception"></exception>
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (!(value is Ursn))
                throw new Exception();

            return (int)(UniqueKey - ((Ursn)value).UniqueKey);
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns>System.Int32.</returns>
        public int CompareTo(Ursn g)
        {
            return (int)(UniqueKey - g.UniqueKey);
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns>System.Int32.</returns>
        public int CompareTo(IUnique g)
        {
            return (int)(UniqueKey - g.UniqueKey);
        }

        /// <summary>
        /// Equalses the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Equals(ulong g)
        {
            return (UniqueKey == g);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object value)
        {
            if (value == null || bytes == null)
                return false;
            if ((value is string))
                return new Ursn(value.ToString()).UniqueKey == UniqueKey;

            return (UniqueKey == ((Ursn)value).UniqueKey);
        }

        /// <summary>
        /// Equalses the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Equals(Ursn g)
        {
            return (UniqueKey == g.UniqueKey);
        }
        /// <summary>
        /// Equalses the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Equals(IUnique g)
        {
            return (UniqueKey == g.UniqueKey());
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>The fully qualified type name.</returns>
        public override String ToString()
        {
            if (bytes == null)
                bytes = new byte[24];
            return new string(this.ToHexTetraChars());
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>String.</returns>
        public String ToString(String format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.
        /// -or-
        /// A null reference (<see langword="Nothing" /> in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.
        /// -or-
        /// A null reference (<see langword="Nothing" /> in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (bytes == null)
                bytes = new byte[24];
            return new string(this.ToHexTetraChars());  
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Ursn a, Ursn b)
        {
            return (a.UniqueKey == b.UniqueKey);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Ursn a, Ursn b)
        {
            return (a.UniqueKey != b.UniqueKey);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="String" /> to <see cref="Ursn" />.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Ursn(String s)
        {
            return new Ursn(s);
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Ursn" /> to <see cref="String" />.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator String(Ursn s)
        {
            return s.ToString();
        }


        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Byte[]" /> to <see cref="Ursn" />.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Ursn(byte[] l)
        {
            return new Ursn(l);
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Ursn" /> to <see cref="System.Byte[]" />.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator byte[] (Ursn s)
        {
            return s.GetBytes();
        }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public static Ursn Empty
        {
            get { return new Ursn(new byte[24]); }
        }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public ulong UniqueSeed { get => KeyBlockX; set => KeyBlockX = value; }

        /// <summary>
        /// Converts to hextetrachars.
        /// </summary>
        /// <returns>System.Char[].</returns>
        public char[] ToHexTetraChars()
        {
            char[] pchchar = new char[32];
            ulong pchblock;
            int pchlength = 32;
            byte pchbyte;
            int idx = 0;

            for (int j = 0; j < 4; j++)
            {
                fixed (byte* pbyte = &bytes[j * 6])
                {
                    pchblock = *((ulong*)pbyte);
                }
                pchblock = pchblock & 0x0000ffffffffffffL;  
                for (int i = 0; i < 8; i++)
                {
                    pchbyte = (byte)(pchblock & 0x3fL);
                    pchchar[idx] = (pchbyte).ToHexTetraChar();
                    idx++;
                    pchblock = pchblock >> 6;
                    if (pchbyte != 0x00) pchlength = idx;
                }
            }

            char[] pchchartrim = new char[pchlength];
            Array.Copy(pchchar, 0, pchchartrim, 0, pchlength);

            return pchchartrim;
        }

        /// <summary>
        /// Froms the hexadecimal tetra chars.
        /// </summary>
        /// <param name="pchchar">The pchchar.</param>
        public void FromHexTetraChars(char[] pchchar)
        {
            int pchlength = pchchar.Length;
            int idx = 0;
            byte pchbyte;
            ulong pchblock = 0;
            int blocklength = 8;
            uint pchblock_int;
            ushort pchblock_short;

            for (int j = 0; j < 4; j++)
            {
                pchblock = 0x00L;
                blocklength = Math.Min(8, Math.Max(0, pchlength - 8 * j));        
                idx = Math.Min(pchlength, 8 * (j + 1)) - 1;                           

                for (int i = 0; i < blocklength; i++)     
                {
                    pchbyte = (pchchar[idx]).ToHexTetraByte();
                    pchblock = pchblock << 6;
                    pchblock = pchblock | (pchbyte & 0x3fUL);
                    idx--;
                }
                fixed (byte* pbyte = bytes)
                {
                    if (j == 3) 
                    {
                        pchblock_short = (ushort)(pchblock & 0x00ffffUL);
                        pchblock_int = (uint)(pchblock >> 16);
                        *((ulong*)&pbyte[18]) = pchblock_short;
                        *((ulong*)&pbyte[20]) = pchblock_int;
                        break;
                    }
                    *((ulong*)&pbyte[j * 6]) = pchblock;

                }
            }
        }

        /// <summary>
        /// Equalses the content.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EqualsContent(Ursn g)
        {
            if (g == null) return false;
            fixed (byte* gbyte = g.bytes)
            fixed (byte* pbyte = bytes)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (*((ulong*)&pbyte[i * 8]) != *((ulong*)&pbyte[i * 8]))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Sets the unique seed.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public void SetUniqueSeed(ulong seed)
        {
            KeyBlockX = seed;
        }

        /// <summary>
        /// Gets the unique seed.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        public ulong GetUniqueSeed()
        {
            return KeyBlockX;
        }
    }
}
