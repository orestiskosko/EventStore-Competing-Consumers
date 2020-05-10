using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;

namespace CompetingConsumers.EventConsumer
{
    public class PersistentSubscriptionOptions
    {
        public string StreamName { get; set; }
        public string GroupName { get; set; }
        public int MaxRetries { get; set; }
    }

    public class PersistentSubscription<THandler>
        where THandler : IPersistentSubscriptionHandler
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly THandler _handler;
        private readonly ILogger<PersistentSubscription<THandler>> _logger;

        private EventStorePersistentSubscriptionBase _subscription;

        public PersistentSubscription(
            THandler handler,
            IEventStoreConnection eventStoreConnection,
            ILogger<PersistentSubscription<THandler>> logger)
        {
            _handler = handler;
            _eventStoreConnection = eventStoreConnection;
            _logger = logger;
        }

        public async Task StartAsync(PersistentSubscriptionOptions options, CancellationToken token = default)
        {
            await Connect(options.StreamName, options.GroupName, token);
        }

        public void Stop(int timeoutMillis = 5000)
        {
            if (_subscription != null) _subscription.Stop(TimeSpan.FromMilliseconds(timeoutMillis));
        }

        private async Task Connect(string streamName, string groupName, CancellationToken token = default)
        {
            _subscription = await _eventStoreConnection.ConnectToPersistentSubscriptionAsync(
                streamName,
                groupName,
                (sub, resolvedEvent) => OnEventAppeared(sub, resolvedEvent, token),
                OnSubscriptionDropped);
        }

        private async Task OnEventAppeared(
            EventStorePersistentSubscriptionBase subscription,
            ResolvedEvent resolvedEvent,
            CancellationToken token = default)
        {
            try
            {
                await _handler.HandleAsync(resolvedEvent, token);
            }
            catch (TaskCanceledException)
            {
                Stop();
            }
        }

        private void OnSubscriptionDropped(EventStorePersistentSubscriptionBase subscription, SubscriptionDropReason reason, Exception ex)
        {
            _logger.LogWarning("Persistent subscription dropped with reason {0}", reason);
        }
    }
}