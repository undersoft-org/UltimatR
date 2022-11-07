namespace System.Instant.Stock
{
#if !NET40Plus

    public enum MemoryMappedFileAccess : uint
    {
        Read = 2,

        ReadWrite = 4,

        CopyOnWrite = 8,

        ReadExecute = 32,

        ReadWriteExecute = 64
    }

    internal static class MemoryMappedFileAccessExtensions
    {
        internal static UnsafeNativeMethods.FileMapAccess ToMapViewFileAccess(this MemoryMappedFileAccess access)
        {
            switch (access)
            {
                case MemoryMappedFileAccess.Read:
                    return UnsafeNativeMethods.FileMapAccess.FileMapRead;
                case MemoryMappedFileAccess.ReadWrite:
                    return UnsafeNativeMethods.FileMapAccess.FileMapRead | UnsafeNativeMethods.FileMapAccess.FileMapWrite;
                case MemoryMappedFileAccess.ReadExecute:
                    return UnsafeNativeMethods.FileMapAccess.FileMapRead | UnsafeNativeMethods.FileMapAccess.FileMapExecute;
                case MemoryMappedFileAccess.ReadWriteExecute:
                    return UnsafeNativeMethods.FileMapAccess.FileMapRead | UnsafeNativeMethods.FileMapAccess.FileMapWrite |
                           UnsafeNativeMethods.FileMapAccess.FileMapExecute;
                default:
                    return UnsafeNativeMethods.FileMapAccess.FileMapAllAccess;
            }
        }
    }
#endif
}