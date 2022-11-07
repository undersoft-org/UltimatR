﻿namespace System.Instant.Stock
{
    using System;
    using Extract;
    using Security.Permissions;

    [SecurityPermission(SecurityAction.LinkDemand)]
    public unsafe class FileStock : SharedStock, IStock
    {
        public Type type;

        public object this[int index]
        {
            get
            {
                object item = null;
                Read(item, index, type);
                return item;
            }
            set { Write(value, index, type); }
        }

        public object this[int index, int offset, Type t]
        {
            get
            {
                object item = null;
                Read(item, index, t, 1000);
                return item;
            }
            set { Write(value, index, t, 1000); }
        }

        public void Rewrite(int index, object structure)
        {
            Read(structure, index, type);
        }

        public FileStock(string file, string name, int bufferSize, Type _type) :
            base(file, name, bufferSize, true, true)
        {
            type = _type;
            Open();
        }

        public FileStock(string file, string name, Type _type) :
            base(file, name, 0, false, true)
        {
            type = _type;
            Open();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Write(object data, long position = 0, Type t = null, int timeout = 1000)
        {
            base.Write(data, position, t, timeout);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Write(object[] data, long position = 0, Type t = null, int timeout = 1000)
        {
            base.Write(data, position, t, timeout);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Write(object[] buffer, int index, int count, long position = 0, Type t = null,
            int timeout = 1000)
        {
            base.Write(buffer, index, count, position, t, timeout);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Write(IntPtr ptr, long length, long position = 0, Type t = null, int timeout = 1000)
        {
            AcquireWriteLock();
            base.Write(ptr, length, position);
            ReleaseWriteLock();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Write(Action<IntPtr> writeFunc, long position = 0)
        {
            base.Write(writeFunc, position);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Write(byte* ptr, long length, long position = 0, Type t = null, int timeout = 1000)
        {
            base.Write(ptr, length, position);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Read(object data, long position = 0, Type t = null, int timeout = 1000)
        {
            base.Read(data, position, t);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Read(object[] data, long position = 0, Type t = null, int timeout = 1000)
        {
            base.Read(data, position, t, timeout);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Read(object[] buffer, int index, int count, long position = 0, Type t = null,
            int timeout = 1000)
        {
            AcquireReadLock();
            base.Read(buffer, index, count, position, t, timeout);
            ReleaseReadLock();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Read(IntPtr destination, long length, long position = 0, Type t = null, int timeout = 1000)
        {
            base.Read(destination, length, position, t, timeout);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Read(byte* destination, long length, long position = 0, Type t = null, int timeout = 1000)
        {
            base.Read(destination, length, position, t, timeout);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public new void Read(Action<IntPtr> readFunc, long position = 0)
        {
            base.Read(readFunc, position);
        }

        public void CopyTo(IStock destination, uint length, int startIndex = 0)
        {
            Extractor.CopyBlock(destination.GetStockPtr() + startIndex, this.GetStockPtr(), length);
        }
    }
}