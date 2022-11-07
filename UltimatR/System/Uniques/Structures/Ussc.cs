/// <summary>
/// The Uniques namespace.
/// </summary>
namespace System.Uniques
{
    using System.Extract;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Struct Ussc
    /// Implements the <see cref="System.IFormattable" />
    /// Implements the <see cref="System.IComparable" />
    /// Implements the <see cref="System.IComparable{System.Uniques.Ussc}" />
    /// Implements the <see cref="System.IEquatable{System.Uniques.Ussc}" />
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IFormattable" />
    /// <seealso cref="System.IComparable" />
    /// <seealso cref="System.IComparable{System.Uniques.Ussc}" />
    /// <seealso cref="System.IEquatable{System.Uniques.Ussc}" />
    /// <seealso cref="System.IUnique" />
    [Serializable]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public unsafe struct Ussc : IFormattable, IComparable
        , IComparable<Ussc>, IEquatable<Ussc>, IUnique
    {
        /// <summary>
        /// The bytes
        /// </summary>
        private fixed byte bytes[16];

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public ulong UniqueKey
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((ulong*)pbyte);

            }
            set
            {
                fixed (byte* b = bytes)
                    *((ulong*)b) = value;
            }
        }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        public ulong UniqueSeed
        {
            get
            {
                fixed (byte* pbyte = bytes)
                    return *((ulong*)(pbyte + 8));
            }
            set
            {
                fixed (byte* b = bytes)
                    *((ulong*)(b + 8)) = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ussc" /> struct.
        /// </summary>
        /// <param name="l">The l.</param>
        public Ussc(ulong l)
        {
            fixed (byte* b = bytes)
            {
                *((ulong*)b) = l;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ussc" /> struct.
        /// </summary>
        /// <param name="s">The s.</param>
        public Ussc(string s)
        {
            this.FromHexTetraChars(s.ToCharArray());    
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ussc" /> struct.
        /// </summary>
        /// <param name="b">The b.</param>
        public Ussc(byte[] b)
        {
            if (b != null)
            {
                int l = b.Length;
                if (l > 16)
                    l = 16;
                fixed (byte* dbp = bytes)
                fixed (byte* sbp = b)
                {
                    Extractor.CopyBlock(dbp, sbp, l);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ussc" /> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        public Ussc(ulong key, uint seed)
        {
            fixed (byte* n = bytes)
            {
                *((ulong*)n) = key;
                *((uint*)n + 8) = seed;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ussc" /> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="time">The time.</param>
        public Ussc(ulong key, long time)
        {
            byte[] shah = key.UniqueBytes64();
            fixed (byte* n = bytes)
            {
                fixed (byte* s = shah)
                    *((ulong*)n) = *((ulong*)s);
                *((long*)(n + 8)) = time;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ussc" /> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        public Ussc(byte[] key, ulong seed)
        {
            fixed (byte* n = bytes)
            {
                fixed (byte* s = key)
                    *((ulong*)n) = *((ulong*)s);
                *((ulong*)(n + 8)) = seed;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ussc" /> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="seed">The seed.</param>
        public Ussc(object key, ulong seed)
        {
            byte[] shah = key.UniqueBytes64();
            fixed (byte* n = bytes)
            {
                fixed (byte* s = shah)
                    *((ulong*)n) = *((ulong*)s);
                *((ulong*)(n + 8)) = seed;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ussc" /> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        public Ussc(object key)
        {
            fixed (byte* n = bytes)
            {
                *((ulong*)n) = key.UniqueKey64();
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
                    int l = 16 - offset;
                    byte[] r = new byte[l];
                    fixed (byte* pbyte = bytes)
                    fixed (byte* rbyte = r)
                    {
                        Extractor.CopyBlock(rbyte, pbyte + offset, l);
                    }
                    return r;
                }
                return null;
            }
            set
            {
                int l = value.Length;
                if (offset > 0 && l < 16)
                {
                    int count = 16 - offset;
                    if (l < count)
                        count = l;
                    fixed (byte* pbyte = bytes)
                    fixed (byte* rbyte = value)
                    {
                        Extractor.CopyBlock(pbyte, rbyte, offset, count);
                    }
                }
                else
                {
                    fixed (byte* pbyte = bytes)
                    fixed (byte* rbyte = value)
                    {
                        Extractor.CopyBlock(pbyte, rbyte, 16);
                    }
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
                if (offset < 16)
                {
                    if ((16 - offset) > length)
                        length = 16 - offset;

                    byte[] r = new byte[length];
                    fixed (byte* pbyte = bytes)
                    fixed (byte* rbyte = r)
                    {
                        Extractor.CopyBlock(rbyte, pbyte + offset, length);
                    }
                    return r;
                }
                return null;

            }
            set
            {
                if (offset < 16)
                {
                    if ((16 - offset) > length)
                        length = 16 - offset;
                    if (value.Length < length)
                        length = value.Length;

                    fixed (byte* rbyte = value)
                    fixed (byte* pbyte = bytes)
                    {
                        Extractor.CopyBlock(pbyte, rbyte, offset, length);
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
            byte[] r = new byte[16];
            fixed (byte* rbyte = r)
            fixed (byte* pbyte = bytes)
            {
                Extractor.CopyBlock(rbyte, pbyte, 16);
            }
            return r;
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetUniqueBytes()
        {
            byte[] kbytes = new byte[8];
            fixed (byte* b = bytes)
            fixed (byte* k = kbytes)
                *((ulong*)k) = *((ulong*)b);
            return kbytes;
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
        /// Sets the unique seed.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public void SetUniqueSeed(ulong seed)
        {
            UniqueSeed = seed;
        }

        /// <summary>
        /// Gets the unique seed.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        public ulong GetUniqueSeed()
        {
            return UniqueSeed;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is not empty.
        /// </summary>
        /// <value><c>true</c> if this instance is not empty; otherwise, <c>false</c>.</value>
        public bool IsNotEmpty
        {
            get { return (UniqueKey != 0); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return (UniqueKey == 0); }
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
        /// Compares to.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.Logs.ILogSate.Exception"></exception>
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (!(value is Ussc))
                throw new Exception();

            return (int)(UniqueKey - ((Ussc)value).UniqueKey);
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns>System.Int32.</returns>
        public int CompareTo(Ussc g)
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
            return (int)(UniqueKey - g.UniqueKey());
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
            if (value == null)
                return false;
            if ((value is string))
                return new Ussc(value.ToString()).UniqueKey == UniqueKey;

            return (UniqueKey == ((Ussc)value).UniqueKey);
        }

        /// <summary>
        /// Equalses the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Equals(Ussc g)
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
            return new string(this.ToHexTetraChars());  
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Ussc a, Ussc b)
        {
            return (a.UniqueKey == b.UniqueKey);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Ussc a, Ussc b)
        {
            return (a.UniqueKey != b.UniqueKey);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="String" /> to <see cref="Ussc" />.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Ussc(String s)
        {
            return new Ussc(s);
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Ussc" /> to <see cref="String" />.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator String(Ussc s)
        {
            return s.ToString();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Byte[]" /> to <see cref="Ussc" />.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Ussc(byte[] l)
        {
            return new Ussc(l);
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Ussc" /> to <see cref="System.Byte[]" />.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator byte[] (Ussc s)
        {
            return s.GetBytes();
        }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public static Ussc Empty
        {
            get { return new Ussc(new byte[24]); }
        }

        /// <summary>
        /// Gets the new.
        /// </summary>
        /// <value>The new.</value>
        public static Ussc New
        {
            get { return new Ussc(Unique.New, DateTime.Now.Ticks); }
        }

        /// <summary>
        /// Converts to hextetrachars.
        /// </summary>
        /// <returns>System.Char[].</returns>
        public char[] ToHexTetraChars()
        {
            char[] pchchar = new char[16];
            ulong pchblock;
            int pchlength = 16;
            byte pchbyte;
            int idx = 0;

            for (int j = 0; j < 2; j++)
            {
                fixed (byte* pbyte = bytes)
                {
                    pchblock = *((ulong*)(pbyte + (j * 6)));
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

            for (int j = 0; j < 2; j++)
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
                    if (j == 1) 
                    {
                        pchblock_short = (ushort)(pchblock & 0x00ffffUL);
                        pchblock_int = (uint)(pchblock >> 8);
                        *((ulong*)&pbyte[6]) = pchblock_short;
                        *((ulong*)&pbyte[8]) = pchblock_int;
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
        public bool EqualsContent(Ussc g)
        {
            ulong pchblockA, pchblockB;
            bool result;

            if (g.IsEmpty) return false;
            fixed (byte* pbyte = bytes)
            {
                pchblockA = *((ulong*)&pbyte[0]);
                pchblockB = *((uint*)&pbyte[8]);
            }

            result = (pchblockA == *((ulong*)&g.bytes[0]))
            && (pchblockB == *((uint*)&g.bytes[8]));


            return result;
        }


    }
}
