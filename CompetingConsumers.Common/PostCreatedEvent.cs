namespace CompetingConsumers.Common
{
    public class PostCreatedEvent : IDomainEvent<PostCreatedEventPayload>
    {
        public PostCreatedEvent(string id, PostCreatedEventPayload payload, DomainEventMetadata metadata)
        {
            Id = id;
            Payload = payload;
            Metadata = metadata;
        }

        public string Id { get; }
        public PostCreatedEventPayload Payload { get; }
        public DomainEventMetadata Metadata { get; }
    }

    public class PostCreatedEventPayload
    {
        public string PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}