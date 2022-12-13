using System;

namespace UltimatR
{
    public interface IChannelPool : IDisposable
    {
        IChannelAccessor Acquire(string channelName = null, string connectionName = null);
    }
}