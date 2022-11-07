using System;
using System.Instant.Stock;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
#if !NET40Plus

    [SecurityPermission(SecurityAction.LinkDemand)]
    public sealed class SafeMemoryMappedViewHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeMemoryMappedViewHandle()
            : base(true) { }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeMemoryMappedViewHandle(IntPtr handle, bool ownsHandle)
            : base(ownsHandle)
        {
            base.SetHandle(handle);
        }

        protected override bool ReleaseHandle()
        {
            try
            {
                return UnsafeNativeMethods.UnmapViewOfFile(this.handle);
            }
            finally
            {
                this.handle = IntPtr.Zero;
            }
        }

        public unsafe void AcquireIntPtr(ref byte* pointer)
        {
            bool flag = false;
            base.DangerousAddRef(ref flag);
            pointer = (byte*)this.handle.ToPointer();
        }

        public void ReleaseIntPtr()
        {
            base.DangerousRelease();
        }
    }
#endif
}