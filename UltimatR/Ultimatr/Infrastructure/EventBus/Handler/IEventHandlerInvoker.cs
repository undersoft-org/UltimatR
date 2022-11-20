﻿using System;
using System.Threading.Tasks;

namespace UltimatR
{
    public interface IEventHandlerInvoker
    {
        Task InvokeAsync(IEventHandler eventHandler, object eventData, Type eventType);
    }
}