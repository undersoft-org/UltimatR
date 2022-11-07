using System;
using System.Instant.Stock;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
#if !NET40Plus

    public sealed class SafeMemoryMappedFileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeMemoryMappedFileHandle()
            : base(true) { }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeMemoryMappedFileHandle(IntPtr handle, bool ownsHandle)
            : base(ownsHandle)
        {
            base.SetHandle(handle);
        }

        protected override bool ReleaseHandle()
        {
            try
            {
                return UnsafeNativeMethods.CloseHandle(this.handle);
            }
            finally
            {
                this.handle = IntPtr.Zero;
            }
        }
    }
#endif
}