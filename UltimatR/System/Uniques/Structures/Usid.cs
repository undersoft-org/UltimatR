/// <summary>
/// The Uniques namespace.
/// </summary>
namespace System.Uniques
{
    using System;
    using System.Extract;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Struct Usid
    /// Implements the <see cref="System.IFormattable" />
    /// Implements the <see cref="System.IComparable" />
    /// Implements the <see cref="System.IComparable{System.IUnique}" />
    /// Implements the <see cref="System.IEquatable{System.IUnique}" />
    /// Implements the <see cref="System.IUnique" />
    /// </summary>
    /// <seealso cref="System.IFormattable" />
    /// <seealso cref="System.IComparable" />
    /// <seealso cref="System.IComparable{System.IUnique}" />
    /// <seealso cref="System.IEquatable{System.IUnique}" />
    /// <seealso cref="System.IUnique" />
    [Serializable]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 8)]
    public unsafe struct Usid : IFormattable, IComparable
        , IComparable<IUnique>, IEquatable<IUnique>, IUnique
    {
        /// <summary>
        /// The bytes
        /// </summary>
        private byte[] bytes;

        /// <summary>
        /// Gets or sets the key block.
        /// </summary>
        /// <value>The key block.</value>
        private ulong _KeyBlock
        {
            get
            {
                ulong block = UniqueKey;
                return (block << 32) | ((block >> 16) & 0xffff0000) | (block >> 48);
            }
            set
            {
                UniqueKey = (value >> 32) | (((value & 0x0ffff0000) << 16)) | (value << 48);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Usid" /> struct.
        /// </summary>
        /// <param name="l">The l.</param>
        public Usid(ulong l)
        {
            bytes = new byte[8];
            UniqueKey = l;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Usid" /> struct.
        /// </summary>
        /// <param name="l">The l.</param>
        public Usid(long l) : this((ulong)l)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Usid" /> struct.
        /// </summary>
        /// <param name="ca">The ca.</param>
        public Usid(string ca)
        {
            bytes = new byte[8];
            this.FromHexTetraChars(ca.ToCharArray());
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Usid" /> struct.
        /// </summary>
        /// <param name="b">The b.</param>
        public Usid(byte[] b)
        {
            bytes = new byte[8];
            if (b != null)
            {
                int l = b.Length;
                if (l > 8)
                    l = 8;
                fixed (byte* dbp = bytes)
                fixed (byte* sbp = b)
                {
                    Extractor.CopyBlock(dbp, sbp, l);
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Usid" /> struct.
        /// </summary>
        /// <param name="z">The z.</param>
        /// <param name="y">The y.</param>
        /// <param name="x">The x.</param>
        public Usid(ushort z, ushort y, uint x)
        {
            bytes = new byte[8];
            fixed (byte* pbytes = bytes)
            {
                *((uint*)pbytes) = x;
                *((uint*)(pbytes + 4)) = y;
                *((uint*)(pbytes + 6)) = z;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Usid" /> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        public Usid(object key)
        {
            bytes = new byte[8];
            fixed (byte* n = bytes)
                *((ulong*)n) = key.UniqueKey64();
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
                if (offset > 0 && offset < 8)
                {
                    int l = (8 - offset);
                    byte[] r = new byte[l];
                    fixed (byte* pbyte = sbytes)
                    fixed (byte* rbyte = r)
                        Extractor.CopyBlock(rbyte, pbyte + offset, l);
                    return r;
                }
                return GetBytes();
            }
            set
            {
                int l = value.Length;
                if (offset > 0 || l < 8)
                {
                    int count = 8 - offset;
                    if (l < count)
                        count = l;
                    fixed (byte* pbyte = sbytes)
                    fixed (byte* rbyte = value)
                    {
                        Extractor.CopyBlock(pbyte, rbyte, offset, l);
                    }
                }
                else
                {
                    fixed (byte* v = value)
                    fixed (byte* b = sbytes)
                        *(ulong*)b = *(ulong*)v;
                }
            }
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetBytes()
        {
            byte[] r = new byte[8];
            fixed (byte* rbyte = r)
            fixed (byte* pbyte = sbytes)
            {
                *((ulong*)rbyte) = *((ulong*)pbyte);
            }
            return r;
        }

        /// <summary>
        /// Gets the unique bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetUniqueBytes()
        {
            return GetBytes();
        }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public ulong UniqueKey
        {
            get
            {
                fixed (byte* pbyte = sbytes)
                    return *((ulong*)pbyte);
            }
            set
            {

                fixed (byte* b = sbytes)
                    *((ulong*)b) = value;
            }
        }

        /// <summary>
        /// Gets or sets the block z.
        /// </summary>
        /// <value>The block z.</value>
        public ushort BlockZ
        {
            get
            {
                fixed (byte* pbyte = sbytes)
                    return *((ushort*)(pbyte + 6));
            }
            set
            {
                fixed (byte* pbyte = sbytes)
                    *((ushort*)(pbyte + 6)) = value;
            }
        }

        /// <summary>
        /// Gets or sets the block y.
        /// </summary>
        /// <value>The block y.</value>
        public ushort BlockY
        {
            get
            {

                fixed (byte* pbyte = sbytes)
                    return *((ushort*)(pbyte + 4));
            }
            set
            {
                fixed (byte* pbyte = sbytes)
                    *((ushort*)(pbyte + 4)) = value;
            }
        }

        /// <summary>
        /// Gets or sets the block x.
        /// </summary>
        /// <value>The block x.</value>
        public ushort BlockX
        {
            get
            {
                fixed (byte* pbyte = sbytes)
                    return *((ushort*)pbyte);
            }
            set
            {
                fixed (byte* pbyte = sbytes)
                    *((ushort*)pbyte) = value;
            }
        }

        /// <summary>
        /// Gets the sbytes.
        /// </summary>
        /// <value>The sbytes.</value>
        private byte[] sbytes
        {
            get => bytes ??= new byte[8];
        }

        /// <summary>
        /// Gets a value indicating whether this instance is not empty.
        /// </summary>
        /// <value><c>true</c> if this instance is not empty; otherwise, <c>false</c>.</value>
        public bool IsNotEmpty
        {
            get { return (UniqueKey > 0); }
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
            fixed (byte* pbyte = sbytes)
            {
                return (int)Hasher32.ComputeKey(pbyte, 8);
            }
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
                return -1;
            if (!(value is Usid))
                throw new Exception();

            return (int)(UniqueKey - value.UniqueKey64());
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns>System.Int32.</returns>
        public int CompareTo(Usid g)
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
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object value)
        {
            if (value == null)
                return false;
            if ((value is string))
                return new Usid(value.ToString()).UniqueKey == UniqueKey;

            return (UniqueKey == ((Usid)value).UniqueKey);
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
        /// Equalses the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Equals(Usid g)
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
            return (UniqueKey == g.UniqueKey);
        }
        /// <summary>
        /// Equalses the specified g.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Equals(string g)
        {
            return (UniqueKey == new Usid(g).UniqueKey);
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
        public static bool operator ==(Usid a, Usid b)
        {
            return (a.UniqueKey == b.UniqueKey);
        }
        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Usid a, Usid b)
        {
            return (a.UniqueKey != b.UniqueKey);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="String" /> to <see cref="Usid" />.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Usid(String s)
        {
            return new Usid(s);
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Usid" /> to <see cref="String" />.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator String(Usid s)
        {
            return s.ToString();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Byte[]" /> to <see cref="Usid" />.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Usid(byte[] l)
        {
            return new Usid(l);
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="Usid" /> to <see cref="System.Byte[]" />.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator byte[] (Usid s)
        {
            return s.GetBytes();
        }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public static Usid Empty
        {
            get { return new Usid() { bytes = new byte[8] }; }
        }

        /// <summary>
        /// Gets the new.
        /// </summary>
        /// <value>The new.</value>
        public static Usid New
        {
            get { return new Usid(Unique.New); }
        }

        /// <summary>
        /// Converts to hextetrachars.
        /// </summary>
        /// <returns>System.Char[].</returns>
        public char[] ToHexTetraChars()
        {
            char[] pchchar = new char[10];
            ulong pchulong;
            byte pchbyte;
            int pchlength = 0;
            ulong _ulongValue = _KeyBlock;
            
            
            pchulong = ((_ulongValue & 0x3fffffff00000000L) >> 6) | ((_ulongValue & 0xffff0000L) >> 6) | (_ulongValue & 0x03ffL);
            for (int i = 0; i < 5; i++)
            {
                pchbyte = (byte)(pchulong & 0x003fL);
                pchchar[i] = (pchbyte).ToHexTetraChar();
                pchulong = pchulong >> 6;
            }

            pchlength = 5;

            
            for (int i = 5; i < 10; i++)
            {
                pchbyte = (byte)(pchulong & 0x003fL);
                if (pchbyte != 0x00) pchlength = i + 1;
                pchchar[i] = (pchbyte).ToHexTetraChar();
                pchulong = pchulong >> 6;
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
            ulong pchulong = 0;
            byte pchbyte;
            int pchlength = 0;

            
            pchlength = pchchar.Length;
            pchbyte = (pchchar[pchlength - 1]).ToHexTetraByte();
            pchulong = pchbyte & 0x3fUL;
            for (int i = pchlength - 2; i >= 0; i--)
            {
                pchbyte = (pchchar[i]).ToHexTetraByte();
                pchulong = pchulong << 6;
                pchulong = pchulong | (pchbyte & 0x3fUL);
            }
            _KeyBlock = ((pchulong << 6) & 0x3fffffff00000000L) | ((pchulong << 6) & 0xffff0000L) | (pchulong & 0x03ffL);
        }

        /// <summary>
        /// Gets or sets the unique seed.
        /// </summary>
        /// <value>The unique seed.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        public ulong UniqueSeed
        {
            get => 0;
            set => throw new NotImplementedException();
        }
        /// <summary>
        /// Sets the unique seed.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SetUniqueSeed(ulong seed)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the unique seed.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        public ulong GetUniqueSeed()
        {
            return 0;
        }
    }
}
