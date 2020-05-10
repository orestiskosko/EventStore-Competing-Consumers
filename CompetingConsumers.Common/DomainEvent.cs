using System;

namespace CompetingConsumers.Common
{
    public class DomainEvent
    {
        public DomainEvent(
            Guid id,
            string payload,
            string streamName,
            string eventType,
            string eventMetadata)
        {
            Id = id;
            Payload = payload;
            StreamName = streamName;
            EventType = eventType;
            EventMetadata = eventMetadata;
        }

        public Guid Id { get; }
        public string Payload { get; }
        public string StreamName { get; }
        public string EventType { get; }
        public string EventMetadata { get; }
    }
}