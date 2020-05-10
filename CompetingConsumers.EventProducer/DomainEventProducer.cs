using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CompetingConsumers.Common;
using EventStore.ClientAPI;

namespace CompetingConsumers.EventProducer
{
    public interface IDomainEventProducer
    {
        Task Produce(DomainEvent domainEvent, CancellationToken token = default);
    }

    public class DomainEventProducer : IDomainEventProducer
    {
        private readonly IEventStoreConnection _eventStoreConnection;

        public DomainEventProducer(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

        public async Task Produce(DomainEvent domainEvent, CancellationToken token = default)
        {
            var eventData = new EventData(
                domainEvent.Id,
                domainEvent.EventType,
                true,
                Encoding.UTF8.GetBytes(domainEvent.Payload),
                Encoding.UTF8.GetBytes(domainEvent.EventMetadata));

            await _eventStoreConnection.AppendToStreamAsync(domainEvent.StreamName, ExpectedVersion.Any, eventData);
        }
    }
}