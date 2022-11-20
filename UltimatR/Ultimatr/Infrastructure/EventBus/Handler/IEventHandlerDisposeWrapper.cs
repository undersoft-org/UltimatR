using System;

namespace UltimatR
{
    public interface IEventHandlerDisposeWrapper : IDisposable
    {
        IEventHandler EventHandler { get; }
    }
}
