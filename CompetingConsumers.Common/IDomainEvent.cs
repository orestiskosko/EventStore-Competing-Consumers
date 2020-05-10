namespace CompetingConsumers.Common
{
    public interface IDomainEvent<TPayload>
        where TPayload : class
    {
        public string Id { get; }
        public TPayload Payload { get; }
        public DomainEventMetadata Metadata { get; }
    }
}