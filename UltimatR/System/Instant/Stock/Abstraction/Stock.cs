﻿using System.Extract;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Instant.Stock
{
    [SecurityPermission(SecurityAction.LinkDemand)]
    [SecurityPermission(SecurityAction.InheritanceDemand)]
    public abstract unsafe class Stock : IDisposable
    {
        public bool Exists { get; set; } = false;
        public bool FixSize { get; set; } = false;
        public Type ItemType { get; private set; }

        public string Path { get; set; }
        public string Name { get; private set; }

        public ushort ClusterId { get; set; } = 0;
        public ushort SectorId { get; set; } = 0;

        public long BufferSize { get; set; } = 0;
        public long UsedSize { get; set; } = 0;
        public long FreeSize { get; set; } = 0;

        public int ItemSize { get; set; } = -1;
        public int ItemCount { get; set; } = -1;
        public long ItemCapacity { get; set; } = -1;

        public virtual long SharedMemorySize
        {
            get { return HeaderOffset + Marshal.SizeOf(typeof(StockHeader)) + BufferSize; }
        }

        public virtual long UsedMemorySize
        {
            get { return HeaderOffset + Marshal.SizeOf(typeof(StockHeader)) + UsedSize; }
        }

        public virtual long FreeMemorySize
        {
            get { return HeaderOffset + Marshal.SizeOf(typeof(StockHeader)) + FreeSize; }
        }

        protected virtual long HeaderOffset
        {
            get { return 0; }
        }

        protected virtual long BufferOffset
        {
            get { return HeaderOffset + Marshal.SizeOf(typeof(StockHeader)); }
        }

        public bool IsOwnerOfSharedMemory { get; private set; }

        public bool ShuttingDown
        {
            get
            {
                if (Header == null || Header->Shutdown == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public SectorOptions options;

        protected MemoryMappedFile Mmf;
        protected MemoryMappedViewAccessor View;

        protected byte* ViewPtr = null;
        protected byte* BufferStartPtr = null;
        protected StockHeader* Header = null;

        protected Stock(string file, string name, long bufferSize, bool ownsSharedMemory, bool fixSize = false,
            Type itemType = null)
        {
            #region Argument validation

            if (name == String.Empty || name == null)
                throw new ArgumentException("Cannot be String.Empty or null", "name");
            if (ownsSharedMemory && bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize", bufferSize,
                    "Buffer size must be larger than zero when creating a new shared memory buffer.");
#if DEBUG
            else if (!ownsSharedMemory && bufferSize > 0)
                System.Diagnostics.Debug.Write("Buffer size is ignored when opening an existing shared memory buffer.",
                    "Warning");
#endif

            #endregion

            IsOwnerOfSharedMemory = ownsSharedMemory;
            Name = name;
            Path = file;

            if (IsOwnerOfSharedMemory)
            {
                BufferSize = bufferSize;
                ItemType = itemType;
                ItemSize = (itemType != null) ? Marshal.SizeOf(ItemType) : -1;
                FixSize = fixSize;
            }
        }

        protected Stock(SectorOptions options)
            : this(options.SectorPath, options.SectorName, options.SectorSize * options.BlockSize, options.IsOwner,
                options.Oversized,
                options.Type)
        {
            this.options = options;

        }

        ~Stock()
        {
            Dispose(false);
        }

        #region Open / Close

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2202:Do not dispose objects multiple times")]
        protected bool Open()
        {
            Close();

            try
            {
                IsOwnerOfSharedMemory = true;
                bool exists = false;
                Mmf = MemoryMappedFile.CreateNew(Path, Assemblies.AssemblyCode + "/" + Name, SharedMemorySize,
                    out exists, FixSize);
                if (exists && FixSize)
                {
                    PreReadHeader();
                    CreateView();
                }
                else
                {
                    CreateView();
                    InitHeader();
                }

                Exists = exists;
            }
            catch
            {
                try
                {
                    IsOwnerOfSharedMemory = false;
                    Mmf = MemoryMappedFile.OpenExisting(Assemblies.AssemblyCode + "/" + Name);
                    PreReadHeader();
                    CreateView();
                }
                catch
                {
                    Close();
                    throw;
                }
            }

            try
            {
                if (!DoOpen())
                {
                    Close();
                    return false;
                }
                else
                    return true;
            }
            catch
            {
                Close();
                throw;
            }
        }

        protected virtual bool DoOpen()
        {
            return true;
        }

        public IntPtr GetStockPtr()
        {
            return (IntPtr)BufferStartPtr;
        }

        protected void CreateView()
        {
            View = Mmf.CreateViewAccessor(0, SharedMemorySize, MemoryMappedFileAccess.ReadWrite);
            View.SafeMemoryMappedViewHandle.AcquireIntPtr(ref ViewPtr);
            Header = (StockHeader*)(ViewPtr + HeaderOffset);
            if (!IsOwnerOfSharedMemory)
                BufferStartPtr = ViewPtr + HeaderOffset + Marshal.SizeOf(typeof(StockHeader));
            else
                BufferStartPtr = ViewPtr + BufferOffset;
        }

        public void PreReadHeader()
        {
            using (MemoryMappedViewAccessor headerView =
                   Mmf.CreateViewAccessor(0, HeaderOffset + Marshal.SizeOf(typeof(StockHeader)),
                       MemoryMappedFileAccess.Read))
            {
                byte* headerPtr = null;

                headerView.SafeMemoryMappedViewHandle.AcquireIntPtr(ref headerPtr);
                StockHeader* _header = (StockHeader*)(headerPtr + HeaderOffset);
                StockHeader header = (StockHeader)Marshal.PtrToStructure((IntPtr)_header, typeof(StockHeader));
                int headerSize = Marshal.SizeOf(typeof(StockHeader));
                BufferSize = header.SharedMemorySize - headerSize;
                UsedSize = header.UsedMemorySize - headerSize;
                FreeSize = header.FreeMemorySize - headerSize;
                ItemCapacity = header.ItemCapacity;
                ItemSize = header.ItemSize;
                ItemCount = header.ItemCount;
                ClusterId = header.ClusterId;
                SectorId = header.SectorId;
                headerView.SafeMemoryMappedViewHandle.ReleaseIntPtr();
            }
        }

        public void ReadHeader()
        {
            object _header = new object();
            View.Read(HeaderOffset, _header, typeof(StockHeader));
            StockHeader header = (StockHeader)_header;
            int headerSize = Marshal.SizeOf(typeof(StockHeader));
            BufferSize = header.SharedMemorySize - headerSize;
            UsedSize = header.UsedMemorySize - headerSize;
            FreeSize = header.FreeMemorySize - headerSize;
            ItemCapacity = header.ItemCapacity;
            ItemSize = header.ItemSize;
            ItemCount = header.ItemCount;
            ClusterId = header.ClusterId;
            SectorId = header.SectorId;
        }

        public void InitHeader()
        {
            if (!IsOwnerOfSharedMemory)
                return;

            StockHeader header = new StockHeader();
            header.SharedMemorySize = SharedMemorySize;
            header.UsedMemorySize = Marshal.SizeOf(typeof(StockHeader));
            header.FreeMemorySize = SharedMemorySize - header.UsedMemorySize;
            header.ItemSize = (ItemType != null && ItemSize > 0) ? ItemSize : -1;
            header.ItemCapacity = (ItemType != null && ItemSize > 0) ? (header.FreeMemorySize / ItemSize) : -1;
            header.ItemCount = -1;
            header.Shutdown = 0;
            View.Write(HeaderOffset, header);
        }

        public void WriteHeader()
        {
            StockHeader header = new StockHeader();
            header.SharedMemorySize = SharedMemorySize;
            header.UsedMemorySize = UsedMemorySize;
            header.FreeMemorySize = FreeMemorySize;
            header.ItemCapacity = ItemCapacity;
            header.ItemCount = ItemCount;
            header.ItemSize = ItemSize;
            header.ClusterId = ClusterId;
            header.SectorId = SectorId;
            header.Shutdown = 0;
            View.Write(HeaderOffset, header);
        }

        public virtual void Close()
        {
            if (IsOwnerOfSharedMemory && View != null)
            {
#pragma warning disable 0420
                Interlocked.Exchange(ref Header->Shutdown, 1);
#pragma warning restore 0420
            }

            DoClose();

            if (View != null)
            {
                View.SafeMemoryMappedViewHandle.ReleaseIntPtr();
                View.Dispose();
            }

            if (Mmf != null)
                Mmf.Dispose();

            Header = null;
            ViewPtr = null;
            BufferStartPtr = null;
            View = null;
            Mmf = null;
        }

        protected virtual void DoClose() { }

        #endregion

        #region Writing

        protected virtual void Write(object source, long position = 0, Type type = null, int timeout = 1000)
        {
            View.Write(BufferOffset + position, source);
        }

        protected virtual void Write(object[] source, long position = 0, Type type = null, int timeout = 1000)
        {
            View.WriteArray(BufferOffset + position, source, 0, source.Length);
        }

        protected virtual void Write(object[] source, int index, int count, long position = 0, Type type = null,
            int timeout = 1000)
        {
            View.WriteArray(BufferOffset + position, source, index, count);
        }

        protected virtual void Write(IntPtr source, long length, long position = 0, Type t = null, int timeout = 1000)
        {
            Extractor.CopyBlock(BufferStartPtr, (ulong)position, (byte*)(source.ToPointer()), 0, (ulong)length);
        }

        protected virtual void Write(byte* source, long length, long position = 0, Type t = null, int timeout = 1000)
        {
            Extractor.CopyBlock(BufferStartPtr, (ulong)position, source, 0, (ulong)length);
        }

        protected virtual void Write(Action<IntPtr> writeFunc, long position = 0)
        {
            writeFunc(new IntPtr(BufferStartPtr + position));
        }

        #endregion

        #region Reading

        protected virtual void Read(object data, long position = 0, Type t = null, int timeout = 1000)
        {
            View.Read(BufferOffset + position, data, t);
        }

        protected virtual void Read(object[] destination, long position = 0, Type t = null, int timeout = 1000)
        {
            int count = destination != null ? destination.Length : 0;
            View.ReadArray(BufferOffset + position, destination, 0, count, t);
        }

        protected virtual void Read(object[] destination, int index, int count, long position = 0, Type t = null,
            int timeout = 1000)
        {
            View.ReadArray(BufferOffset + position, destination, index, count, t);
        }

        protected virtual void Read(IntPtr destination, long length, long position = 0, Type t = null,
            int timeout = 1000)
        {
            Extractor.CopyBlock(destination, 0, new IntPtr(BufferStartPtr), (ulong)position, (ulong)length);
        }

        protected virtual void Read(byte* destination, long length, long position = 0, Type t = null,
            int timeout = 1000)
        {
            Extractor.CopyBlock(destination, 0, BufferStartPtr, (ulong)position, (ulong)length);
        }

        protected virtual void Read(Action<IntPtr> readFunc, long position = 0)
        {
            readFunc(new IntPtr(BufferStartPtr + position));
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                this.Close();
            }
        }

        #endregion
    }
}