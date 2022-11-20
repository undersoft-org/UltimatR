using System;

namespace UltimatR
{
    public interface IEvent : IEntity
    {
        uint EventVersion   { get; set; }
        string EventType      { get; set; }
        long AggregateId    { get; set; }
        string AggregateType  { get; set; }
        string Data           { get; set; }
        DateTime PublishTime  { get; set; }
        PublishStatus PublishStatus { get; set; }
    }
}