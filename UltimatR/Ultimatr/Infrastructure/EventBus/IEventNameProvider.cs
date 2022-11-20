using System;

namespace UltimatR
{
    public interface IEventNameProvider
    {
        string GetName(Type eventType);
    }
}