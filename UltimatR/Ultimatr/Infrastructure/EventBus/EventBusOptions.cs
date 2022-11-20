
using System.Collections.Generic;

namespace UltimatR
{
    public class EventBusOptions
    {
        public IList<IEventHandler> Handlers { get; }

        public EventBusOptions()
        {
            Handlers = new List<IEventHandler>();
        }
    }
}
