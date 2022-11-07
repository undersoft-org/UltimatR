﻿
namespace System.Uniques
{
    using System.Collections.Specialized;
    using System.Extract;
    using System.Runtime.InteropServices;

    [Serializable]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 24)]
    public unsafe struct Ussn : IFormattable, IComparable
        , IComparable<Ussn>, IEquatable<Ussn>, IUnique, ISerialNumber, IDisposable
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        private byte[] bytes;

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

        public ulong UniqueSeed
        {
            get
            {
                fixed (byte* pbyte = sbytes)
                    return *((uint*)(pbyte + 8));
            }
            set
            {
                fixed (byte* b = sbytes)
                    *((ulong*)(b + 8)) = value;
            }
        }

        public uint BlockZY
        {
            get
            {
                fixed (byte* pbyte = sbytes)
                    return *((uint*)(pbyte + 8));
            }
            set
            {
                fixed (byte* b = sbytes)
                    *((uint*)(b + 8)) = value;
            }
        }

        public ulong BlockZYX
        {
            get
            {
                fixed (byte* pbyte = sbytes)
                    return *((ulong*)(pbyte + 8));
            }
            set
            {
                fixed (byte* b = sbytes)
                    *((ulong*)(b + 8)) = value;
            }
        }

        public ushort BlockZ
        {
            get
            {
                fixed (byte* pbyte = sbytes)
                    return *((ushort*)(pbyte + 8));
            }
            set
            {
                fixed (byte* b = sbytes)
                    *((ushort*)(b + 8)) = value;
            }
        }
        public ushort BlockY
        {
            get
            {
                fixed (byte* pbyte = sbytes)
                    return *((ushort*)(pbyte + 10));
            }
            set
            {
                fixed (byte* b = sbytes)
                    *((ushort*)(b + 10)) = value;
            }
        }
        public ushort BlockX
        {
            get
            {
                fixed (byte* pbyte = sbytes)
                    return *((ushort*)(pbyte + 12));
            }
            set
            {
                fixed (byte* b = sbytes)
                    *((ushort*)(b + 12)) = value;
            }
        }

        public byte PriorityBlock
        {
            get
            {
                fixed(byte* pbyte = sbytes)
                    return *(pbyte + 14);
            }
            set
            {
                fixed(byte* b = sbytes)
                    *(b + 14) = value;
            }
        }

        public byte FlagsBlock
        {
            get
            {
                fixed (byte* pbyte = sbytes)
                    return *(pbyte + 15);
            }
            set
            {
                fixed (byte* b = sbytes)
                    *(b + 15) = value;
            }
        }

        public long TimeBlock
        {
            get
            {
                fixed (byte* pbyte = sbytes)
                    return *((long*)(pbyte + 16));
            }
            set
            {
                fixed (byte* b = sbytes)
                    *((long*)(b + 16)) = value;
            }
        }

        public Ussn(ulong l)
        {
            bytes = new byte[24];
            fixed (byte* b = bytes)
            {
                *((ulong*)b) = l;
                *((long*)(b + 16)) = DateTime.Now.ToBinary();
            }
        }
        public Ussn(string s)
        {
            bytes = new byte[24];
            this.FromHexTetraChars(s.ToCharArray());    
        }
        public Ussn(byte[] b)
        {
            bytes = new byte[24];
            if (b != null)
            {
                int l = b.Length;
                if (l > 24)
                    l = 24;
                fixed (byte* dbp = bytes)
                fixed (byte* sbp = b)
                {
                    Extractor.CopyBlock(dbp, sbp, l);
                }
            }
        }

        public Ussn(ulong key, ulong seed)
        {
            bytes = new byte[24];
            fixed (byte* n = bytes)
            {
                *((ulong*)n) = key;
                *((ulong*)n + 8) = seed;
            }
        }
        public Ussn(byte[] key, ulong seed)
        {
            bytes = new byte[24];
            fixed (byte* n = bytes)
            {
                fixed (byte* s = key)
                    *((ulong*)n) = *((ulong*)s);
                *((ulong*)(n + 8)) = seed;
            }
        }
        public Ussn(object key, ulong seed)
        {
            bytes = new byte[24];
            byte[] shah = key.UniqueBytes64();
            fixed (byte* n = bytes)
            {
                fixed (byte* s = shah)
                    *((ulong*)n) = *((ulong*)s);
                *((ulong*)(n + 8)) = seed;
            }
        }

        public Ussn(ulong key, short z, short y, short x, short flags, long time)
        {
            bytes = new byte[24];
            fixed (byte* n = bytes)
            {
                *((ulong*)n) = key;
                *((short*)&n[8]) = z;
                *((short*)&n[10]) = y;
                *((short*)&n[12]) = x;
                *((short*)&n[14]) = flags;
                *((long*)&n[16]) = time;
            }
        }
        public Ussn(byte[] key, short z, short y, short x, short flags, long time)
        {
            bytes = new byte[24];
            fixed (byte* n = bytes)
            {
                fixed (byte* s = key)
                    *((ulong*)n) = *((ulong*)s);
                *((short*)(n + 8)) = z;
                *((short*)(n + 10)) = y;
                *((short*)(n + 12)) = x;
                *((short*)(n + 14)) = flags;
                *((long*)(n + 16)) = time;
            }
        }
        public Ussn(object key, short z, short y, short x, BitVector32 flags, DateTime time)
        {
            bytes = new byte[24];
            byte[] shah = key.UniqueBytes64();
            fixed (byte* n = bytes)
            {
                fixed (byte* s = shah)
                    *((ulong*)n) = *((ulong*)s);
                *((short*)(n + 8)) = z;
                *((short*)(n + 10)) = y;
                *((short*)(n + 12)) = x;
                *((short*)(n + 14)) = *((short*)&flags);
                *((long*)(n + 16)) = time.ToBinary();    
            }
        }
        public Ussn(object key)
        {
            bytes = new byte[24];
            fixed (byte* n = bytes)
            {
                *((ulong*)n) = key.UniqueKey64();
                
            }
        }

        public Ussn(Ussn item)
        {
            bytes = new byte[24];
            fixed (byte* pbyte = bytes)
                fixed(byte* pbytes = item.GetBytes())
                Extractor.CopyBlock(pbyte, pbytes, 24);
        }

        public byte[] this[int offset]
        {
            get
            {
                if (offset != 0)
                {
                    int l = 24 - offset;
                    byte[] r = new byte[l];
                    fixed (byte* pbyte = sbytes)
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
                if (offset > 0 && l < 24)
                {
                    int count = 24 - offset;
                    if (l < count)
                        count = l;
                    fixed (byte* pbyte = sbytes)
                    fixed (byte* rbyte = value)
                    {
                        Extractor.CopyBlock(pbyte, rbyte, offset, count);
                    }
                }
                else
                {
                    fixed (byte* pbyte = sbytes)
                    fixed (byte* rbyte = value)
                    {
                        Extractor.CopyBlock(pbyte, rbyte, 24);
                    }
                }
            }
        }
        public byte[] this[int offset, int length]
        {
            get
            {
                if (offset < 24)
                {
                    if ((24 - offset) > length)
                        length = 24 - offset;

                    byte[] r = new byte[length];
                    fixed (byte* pbyte = sbytes)
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
                if (offset < 24)
                {
                    if ((24 - offset) > length)
                        length = 24 - offset;
                    if (value.Length < length)
                        length = value.Length;

                    fixed (byte* rbyte = value)
                    fixed (byte* pbyte = sbytes)
                    {
                        Extractor.CopyBlock(pbyte, rbyte, offset, length);
                    }
                }
            }
        }

        private byte[] sbytes
        {
            get => bytes ??= new byte[24];
        }

        public void SetBytes(byte[] value, int offset)
        {
            this[offset] = value;
        }
        public byte[] GetBytes(int offset, int length)
        {
            return this[offset, length];
        }
        public byte[] GetBytes()
        {
            byte[] r = new byte[24];
            fixed (byte* rbyte = r)
            fixed (byte* pbyte = sbytes)
            {
                Extractor.CopyBlock(rbyte, pbyte, 24);
            }
            return r;
        }
        public byte[] GetUniqueBytes()
        {
            byte[] kbytes = new byte[8];
            fixed (byte* b = sbytes)
            fixed (byte* k = kbytes)
                *((ulong*)k) = *((ulong*)b);
            return kbytes;
        }

        public void SetUniqueKey(ulong value)
        {
            UniqueKey = value;
        }
        public ulong GetUniqueKey()
        {
            return UniqueKey;
        }

        public void SetUniqueSeed(ulong seed)
        {
            UniqueSeed = seed;
        }
        public ulong GetUniqueSeed()
        {
            return UniqueSeed;
        }

        public ulong ValueFromXYZ(uint vectorZ, uint vectorY)
        {
            return (BlockZ * vectorZ * vectorY) + (BlockY * vectorY) + BlockX;
        }
        public ushort[] ValueToXYZ(ulong vectorZ, ulong vectorY, ulong value)
        {
            if (value > 0)
            {
                ulong vectorYZ = (vectorY * vectorZ);
                ulong blockZdiv = (value / vectorYZ);
                ulong blockYsub = value - (blockZdiv * vectorYZ);
                ulong blockYdiv = blockYsub / vectorY;
                ulong blockZ = (blockZdiv > 0 && (value % vectorYZ) > 0) ? blockZdiv + 1 : blockZdiv;
                ulong blockY = (blockYdiv > 0 && (value % vectorY) > 0) ? blockYdiv + 1 : blockYdiv;
                ulong blockX = value % vectorY;
                return new ushort[] { (ushort)blockZ, (ushort)blockY, (ushort)blockX };
            }
            return null;
        }

        public ushort GetFlags()
        {
            fixed (byte* pbyte = sbytes)
                return *((ushort*)(pbyte + 15));
        }
        public BitVector32 GetFlagsBits()
        {
            fixed (byte* pbyte = sbytes)
                return new BitVector32(*((pbyte + 15)));
        }
        public void SetFlagBits(BitVector32 bits)
        {
            fixed (byte* pbyte = sbytes)
                *((pbyte + 15)) = *((byte*)&bits);
        }
        public void SetFlagBit(ushort flagNumber)
        {
            fixed (byte* pbyte = sbytes)
            {
                *((pbyte + 15)) = (byte) (*((pbyte + 15)) | (1 << flagNumber));
            }
        }
        public void ClearFlagBit(ushort flagNumber)
        {
            fixed (byte* pbyte = sbytes)
            {
                *((pbyte + 15)) = (byte)(*((pbyte + 15)) & ~(1 << flagNumber));
            }
        }
        public bool GetFlagBit(ushort flagNumber)
        {
            fixed (byte* pbyte = sbytes)
            {
               int value = ((*((pbyte + 15)) >> flagNumber) & 1);
               return (value > 0) ? true : false;
            }
        }
        public void SetFlag(bool flag, ushort flagNumber)
        {
            if(flag)
                SetFlagBit(flagNumber);
            else
                ClearFlagBit(flagNumber);
        }

        public byte GetPriority()
        {
            fixed(byte* pbyte = sbytes)
            {
               return *(pbyte + 15);               
            }
        }
        public byte SetPriority(byte priority)
        {
            fixed(byte* pbyte = sbytes)
            {
               return *(pbyte + 15) = priority;
            }
        }
        public byte ComparePriority(byte priority)
        {
            fixed(byte* pbyte = sbytes)
            {
                return (byte)(*(pbyte + 15) - priority);
            }
        }

        public long GetDateLong()
        {
            fixed (byte* pbyte = bytes)
                return *((long*)(pbyte + 16));
        }
        public DateTime GetDateTime()
        {
            fixed (byte* pbyte = sbytes)
                return DateTime.FromBinary(*((long*)(pbyte + 16)));
        }

        public void SetDateLong(long item)
        {
            fixed (byte* pbyte = sbytes)
                *((long*)(pbyte + 16)) = item;
        }

        public Guid GetGuid()
        {
            if (IsEmpty) return Guid.Empty;
            byte[] dst = new byte[16];
            fixed(byte* d = dst)
            fixed (byte* b = sbytes)
            {
                *(ulong*) d = *(ulong*)b;
                *(ulong*)(d + 8) = *(ulong*)(b + 16);
            }

            return new Guid(dst);
        }

        public void SetGuid(Guid guid)
        {
            byte[] src = guid.ToByteArray();
            fixed(byte* s = src)
            fixed (byte* b = sbytes)
            {
                *(ulong*) b = *(ulong*)s;
                *(ulong*)(b + 16) = *(ulong*)(s + 8);
            }
        }

        public Guid GUID
        {
            get => GetGuid();
            set => SetGuid(value);
        }

        public bool IsNotEmpty
        {
            get { return (UniqueKey != 0); }
        }

        public bool IsEmpty
        {
            get { return (UniqueKey == 0); }
        }

        public override int GetHashCode()
        {
            fixed (byte* pbyte = &this[0, 8].BitAggregate64to32()[0])
                return *((int*)pbyte);
        }

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (!(value is Ussn))
                throw new Exception();

            return (int)(UniqueKey - ((Ussn)value).UniqueKey);
        }

        public int CompareTo(Ussn g)
        {
            return (int)(UniqueKey - g.UniqueKey);
        }

        public int CompareTo(IUnique g)
        {
            return (int)(UniqueKey - g.UniqueKey());
        }

        public bool Equals(ulong g)
        {
            return (UniqueKey == g);
        }

        public override bool Equals(object value)
        {
            if (value == null)
                return false;
            if ((value is string))
                return new Ussn(value.ToString()).UniqueKey == UniqueKey;

            return (UniqueKey == ((Ussn)value).UniqueKey);
        }

        public bool Equals(Ussn g)
        {
            return (UniqueKey == g.UniqueKey);
        }
        public bool Equals(IUnique g)
        {
            return (UniqueKey == g.UniqueKey());
        }

        public override String ToString()
        {
            return new string(this.ToHexTetraChars());
        }

        public String ToString(String format)
        {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return new string(this.ToHexTetraChars());  
        }

        public static bool operator ==(Ussn a, Ussn b)
        {
            return (a.UniqueKey == b.UniqueKey);
        }

        public static bool operator !=(Ussn a, Ussn b)
        {
            return (a.UniqueKey != b.UniqueKey);
        }

        public static explicit operator Ussn(String s)
        {
            return new Ussn(s);
        }
        public static implicit operator String(Ussn s)
        {
            return s.ToString();
        }

        public static explicit operator Ussn(byte[] l)
        {
            return new Ussn(l);
        }
        public static implicit operator byte[] (Ussn s)
        {
            return s.GetBytes();
        }

        public static Ussn Empty => new Ussn() 
        { bytes = new byte[24] };

        public static Ussn New => new Ussn(Unique.New) 
        { TimeBlock = DateTime.Now.ToBinary() };

        public Ussn Auto()
        {
            UniqueKey = Unique.New;
            TimeBlock = DateTime.Now.ToBinary();
            return this;
        }

        public char[] ToHexTetraChars()
        {
            char[] pchchar = new char[32];
            ulong pchblock;
            int pchlength = 32;
            byte pchbyte;
            int idx = 0;

            for (int j = 0; j < 4; j++)
            {
                fixed (byte* pbyte = sbytes)
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
                fixed (byte* pbyte = sbytes)
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

        public bool EqualsContent(Ussn g)
        {
            ulong pchblockA, pchblockB, pchblockC;
            bool result;

            if (g.IsEmpty) return false;
            fixed (byte* pbyte = sbytes)
            {
                pchblockA = *((ulong*)&pbyte[0]);
                pchblockB = *((ulong*)&pbyte[8]);
                pchblockC = *((ulong*)&pbyte[16]);
            }

            fixed (byte* pbyte = g.GetBytes())
            {
                result = (pchblockA == *((ulong*) &pbyte[0]))
                         && (pchblockB == *((ulong*) &pbyte[8]))
                         && (pchblockC == *((ulong*) &pbyte[16]));
            }

            return result;
        }

        public bool Equals(BitVector32 other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(DateTime other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ISerialNumber other)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            bytes = null;
        }
    }
}
