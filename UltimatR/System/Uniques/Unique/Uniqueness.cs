/// <summary>
/// The Uniques namespace.
/// </summary>
namespace System.Uniques
{
    using System.Collections;

    #region Enums

    /// <summary>
    /// Enum HashBits
    /// </summary>
    public enum HashBits
    {
        /// <summary>
        /// The bit64
        /// </summary>
        bit64,

        /// <summary>
        /// The bit32
        /// </summary>
        bit32
    }

    #endregion

    #region Interfaces

    /// <summary>
    /// Interface IUniqueness
    /// </summary>
    public interface IUniqueness
    {
        #region Methods

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public Byte[] Bytes(Byte[] obj, ulong seed = 0);

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public Byte[] Bytes(IList obj, ulong seed = 0);

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Byte[].</returns>
        public Byte[] Bytes(IUnique obj);

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public Byte[] Bytes(Object obj, ulong seed = 0);

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public Byte[] Bytes(string obj, ulong seed = 0);

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public UInt64 Key(Byte[] obj, ulong seed = 0);

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public UInt64 Key(IList obj, ulong seed = 0);

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>UInt64.</returns>
        public UInt64 Key(IUnique obj);

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public UInt64 Key(IUnique obj, ulong seed);

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>UInt64.</returns>
        public UInt64 Key<V>(IUnique<V> obj);

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public UInt64 Key(Object obj, ulong seed = 0);

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public UInt64 Key(string obj, ulong seed = 0);

        #endregion
    }

    #endregion

    /// <summary>
    /// Class Unique32.
    /// Implements the <see cref="System.Uniques.Uniqueness" />
    /// </summary>
    /// <seealso cref="System.Uniques.Uniqueness" />
    public class Unique32 : Uniqueness
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Unique32" /> class.
        /// </summary>
        public Unique32() : base(HashBits.bit32)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override Byte[] Bytes(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override Byte[] Bytes(IList obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override unsafe Byte[] Bytes(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeBytes((byte*)obj.ToPointer(), length, seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Byte[].</returns>
        public override Byte[] Bytes(IUnique obj)
        {
            return obj.GetBytes();
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override Byte[] Bytes(Object obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override
                        Byte[] Bytes(string obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override
            Byte[] Bytes(Type obj, ulong seed = 0)
        {
            return obj.UniqueBytes32(seed);
        }

        /// <summary>
        /// Computes the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override unsafe Byte[] ComputeBytes(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher32.ComputeBytes(bytes, length, seed);
        }

        /// <summary>
        /// Computes the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            return Hasher32.ComputeBytes(bytes, seed);
        }

        /// <summary>
        /// Computes the key.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override unsafe UInt64 ComputeKey(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher32.ComputeKey(bytes, length, seed);
        }

        /// <summary>
        /// Computes the key.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override unsafe UInt64 ComputeKey(byte[] bytes, ulong seed = 0)
        {
            return Hasher32.ComputeKey(bytes, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(IList obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override unsafe UInt64 Key(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeKey((byte*)obj.ToPointer(), length, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(IUnique obj)
        {
            return obj.UniqueKey();
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(IUnique obj, ulong seed)
        {
            return Key(obj.GetBytes(), seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key<V>(IUnique<V> obj)
        {
            return obj.CompactKey();
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key<V>(IUnique<V> obj, ulong seed)
        {
            return Key(obj.UniqueValues(), seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(Object obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(string obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(Type obj, ulong seed = 0)
        {
            return obj.UniqueKey32(seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        protected override unsafe Byte[] Bytes(byte* obj, int length, ulong seed = 0)
        {
            return ComputeBytes(obj, length, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        protected override unsafe UInt64 Key(byte* obj, int length, ulong seed = 0)
        {
            return ComputeKey(obj, length, seed);
        }

        #endregion
    }

    /// <summary>
    /// Class Unique64.
    /// Implements the <see cref="System.Uniques.Uniqueness" />
    /// </summary>
    /// <seealso cref="System.Uniques.Uniqueness" />
    public class Unique64 : Uniqueness
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Unique64" /> class.
        /// </summary>
        public Unique64() : base(HashBits.bit64)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override Byte[] Bytes(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override Byte[] Bytes(IList obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override unsafe Byte[] Bytes(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeBytes((byte*)obj.ToPointer(), length, seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Byte[].</returns>
        public override Byte[] Bytes(IUnique obj)
        {
            return obj.GetBytes();
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override Byte[] Bytes(Object obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override Byte[] Bytes(string obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override Byte[] Bytes(Type obj, ulong seed = 0)
        {
            return obj.UniqueBytes64(seed);
        }

        /// <summary>
        /// Computes the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override unsafe Byte[] ComputeBytes(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher64.ComputeBytes(bytes, length, seed);
        }

        /// <summary>
        /// Computes the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public override Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            return Hasher64.ComputeBytes(bytes, seed);
        }

        /// <summary>
        /// Computes the key.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override unsafe UInt64 ComputeKey(byte* bytes, int length, ulong seed = 0)
        {
            return Hasher64.ComputeKey(bytes, length, seed);
        }

        /// <summary>
        /// Computes the key.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 ComputeKey(byte[] bytes, ulong seed = 0)
        {
            return Hasher64.ComputeKey(bytes, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(Byte[] obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(IList obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override unsafe UInt64 Key(IntPtr obj, int length, ulong seed = 0)
        {
            return ComputeKey((byte*)obj.ToPointer(), length, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(IUnique obj)
        {
            return obj.UniqueKey;
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(IUnique obj, ulong seed)
        {
            return ComputeKey(obj.GetUniqueBytes(), seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key<V>(IUnique<V> obj)
        {
            return obj.CompactKey();
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key<V>(IUnique<V> obj, ulong seed)
        {
            return Key(obj.UniqueValues(), seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(Object obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(string obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public override UInt64 Key(Type obj, ulong seed = 0)
        {
            return obj.UniqueKey64(seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        protected override unsafe Byte[] Bytes(byte* obj, int length, ulong seed = 0)
        {
            return ComputeBytes(obj, length, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        protected override unsafe UInt64 Key(byte* obj, int length, ulong seed = 0)
        {
            return ComputeKey(obj, length, seed);
        }

        #endregion
    }

    /// <summary>
    /// Class Uniqueness.
    /// Implements the <see cref="System.Uniques.IUniqueness" />
    /// </summary>
    /// <seealso cref="System.Uniques.IUniqueness" />
    public abstract class Uniqueness : IUniqueness
    {
        #region Fields

        /// <summary>
        /// The unique
        /// </summary>
        protected Uniqueness unique;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Uniqueness" /> class.
        /// </summary>
        public Uniqueness()
        {
            unique = Unique.Bit64;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Uniqueness" /> class.
        /// </summary>
        /// <param name="hashBits">The hash bits.</param>
        public Uniqueness(HashBits hashBits)
        {
            if(hashBits == HashBits.bit32)
                unique = Unique.Bit32;
            else
                unique = Unique.Bit64;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public virtual Byte[] Bytes(Byte[] obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public virtual Byte[] Bytes(IList obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public virtual Byte[] Bytes(IntPtr obj, int length, ulong seed = 0)
        {
            return unique.Bytes(obj, length, seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Byte[].</returns>
        public virtual Byte[] Bytes(IUnique obj)
        {
            return obj.GetBytes();
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public virtual Byte[] Bytes(Object obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public virtual Byte[] Bytes(string obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public virtual Byte[] Bytes(Type obj, ulong seed = 0)
        {
            return unique.Bytes(obj, seed);
        }

        /// <summary>
        /// Computes the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public virtual unsafe Byte[] ComputeBytes(byte* bytes, int length, ulong seed = 0)
        {
            return unique.ComputeBytes(bytes, length, seed);
        }

        /// <summary>
        /// Computes the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        public virtual unsafe Byte[] ComputeBytes(byte[] bytes, ulong seed = 0)
        {
            return unique.ComputeBytes(bytes, seed);
        }

        /// <summary>
        /// Computes the key.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public virtual unsafe UInt64 ComputeKey(byte* bytes, int length, ulong seed = 0)
        {
            return unique.ComputeKey(bytes, length, seed);
        }

        /// <summary>
        /// Computes the key.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public virtual unsafe UInt64 ComputeKey(byte[] bytes, ulong seed = 0)
        {
            return unique.ComputeKey(bytes, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public virtual UInt64 Key(Byte[] obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public virtual UInt64 Key(IList obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public virtual UInt64 Key(IntPtr obj, int length, ulong seed = 0)
        {
            return unique.Key(obj, length, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>UInt64.</returns>
        public virtual UInt64 Key(IUnique obj)
        {
            return obj.UniqueKey;
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public virtual UInt64 Key(IUnique obj, ulong seed)
        {
            if(obj.UniqueSeed != seed) obj.UniqueSeed = seed;
            return unique.ComputeKey(obj.GetUniqueBytes(), seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>UInt64.</returns>
        public virtual UInt64 Key<V>(IUnique<V> obj)
        {
            return obj.CompactKey();
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public virtual UInt64 Key<V>(IUnique<V> obj, ulong seed)
        {
            
            return unique.Key(obj.UniqueValues(), seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public virtual UInt64 Key(Object obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public virtual UInt64 Key(string obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        public virtual UInt64 Key(Type obj, ulong seed = 0)
        {
            return unique.Key(obj, seed);
        }

        /// <summary>
        /// Byteses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>Byte[].</returns>
        protected virtual unsafe Byte[] Bytes(byte* obj, int length, ulong seed = 0)
        {
            return unique.Bytes(obj, length, seed);
        }

        /// <summary>
        /// Keys the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="length">The length.</param>
        /// <param name="seed">The seed.</param>
        /// <returns>UInt64.</returns>
        protected virtual unsafe UInt64 Key(byte* obj, int length, ulong seed = 0)
        {
            return unique.Key(obj, length, seed);
        }

        #endregion
    }
}
