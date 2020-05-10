namespace CompetingConsumers.Common
{
    public class DomainEventMetadata
    {
        public DomainEventMetadata(string sender)
        {
            Sender = sender;
        }

        public string Sender { get; }
    }
}