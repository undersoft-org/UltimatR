using System.Collections.Generic;

namespace UltimatR
{
    public interface IEventHandlerFactory
    {
        /// <summary>
        /// Gets an event handler.
        /// </summary>
        /// <returns>The event handler</returns>
        IEventHandlerDisposeWrapper GetHandler();

        bool IsInFactories(List<IEventHandlerFactory> handlerFactories);
    }
}
