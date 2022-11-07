using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using Microsoft.Win32.SafeHandles;

namespace System.Instant.Stock
{
#if !NET40Plus

    [SecurityPermission(SecurityAction.LinkDemand)]
    public sealed class MemoryMappedView : IDisposable
    {
        SafeMemoryMappedViewHandle _handle;

        public SafeMemoryMappedViewHandle SafeMemoryMappedViewHandle
        {
            get { return this._handle; }
        }

        long _size;
        long _offset;

        public long Size
        {
            get { return _size; }
        }

        public long ViewStartOffset
        {
            get { return _offset; }
        }

        private MemoryMappedView(SafeMemoryMappedViewHandle handle, long offset, long size)
        {
            this._handle = handle;
            this._offset = offset;
            this._size = size;
        }

        ~MemoryMappedView()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposeManagedResources)
        {
            if (this._handle != null && !this._handle.IsClosed)
                this._handle.Dispose();
            this._handle = null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability",
            "CA1404:CallGetLastErrorImmediatelyAfterPInvoke")]
        internal static MemoryMappedView CreateView(SafeMemoryMappedFileHandle safeMemoryMappedFileHandle,
            MemoryMappedFileAccess access, long offset, long size)
        {
            UnsafeNativeMethods.SYSTEM_INFO info = new UnsafeNativeMethods.SYSTEM_INFO();
            UnsafeNativeMethods.GetSystemInfo(ref info);

            long fileMapStart = (offset / info.dwAllocationGranularity) * info.dwAllocationGranularity;

            long mapViewSize = (offset % info.dwAllocationGranularity) + size;

            long viewDelta = offset - fileMapStart;

            SafeMemoryMappedViewHandle safeHandle = UnsafeNativeMethods.MapViewOfFile(safeMemoryMappedFileHandle,
                access.ToMapViewFileAccess(), (ulong)fileMapStart, new UIntPtr((ulong)mapViewSize));
            var lastWin32Error = Marshal.GetLastWin32Error();
            if (safeHandle.IsInvalid)
            {
                if (lastWin32Error == UnsafeNativeMethods.ERROR_FILE_NOT_FOUND)
                    throw new FileNotFoundException();
                throw new System.IO.IOException(UnsafeNativeMethods.GetMessage(lastWin32Error));
            }

            return new MemoryMappedView(safeHandle, viewDelta, size);
        }
    }
#endif
}