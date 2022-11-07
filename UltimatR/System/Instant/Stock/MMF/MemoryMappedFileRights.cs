namespace System.Instant.Stock
{
#if !NET40Plus

    [Flags]
    public enum MemoryMappedFileRights : uint
    {
        Write = 0x02,

        Read = 0x04,

        ReadWrite = MemoryMappedFileRights.Write | MemoryMappedFileRights.Read,
    }
#endif
}