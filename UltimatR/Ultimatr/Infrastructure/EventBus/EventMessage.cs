using System;

namespace UltimatR
{
    public class EventMessage
    {
        public Guid MessageId { get; }

        public object EventData { get; }

        public Type EventType { get; }

        public EventMessage(Guid messageId, object eventData, Type eventType)
        {
            MessageId = messageId;
            EventData = eventData;
            EventType = eventType;
        }
    }
}