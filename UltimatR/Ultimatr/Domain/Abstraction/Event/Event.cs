using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UltimatR
{
    public class Event : Entity, IEvent
    {
        public virtual uint     EventVersion { get; set; }
        public virtual string   EventType { get; set; }
        public virtual long     AggregateId { get; set; }
        public virtual string   AggregateType { get; set; }
        public virtual string   Data { get; set; }
        public virtual DateTime PublishTime { get; set; }
        public virtual PublishStatus PublishStatus { get; set; }
    }

}