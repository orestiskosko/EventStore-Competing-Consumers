using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;

namespace CompetingConsumers.EventConsumer
{
    public interface IPersistentSubscriptionHandler
    {
        Task HandleAsync(ResolvedEvent resolvedEvent, CancellationToken token = default);
    }

    public class PostEventHandler : IPersistentSubscriptionHandler
    {
        private readonly ILogger<PostEventHandler> _logger;

        public PostEventHandler(ILogger<PostEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(ResolvedEvent resolvedEvent, CancellationToken token = default)
        {
            var stringEventData = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
            var stringOriginalEventData = Encoding.UTF8.GetString(resolvedEvent.OriginalEvent.Data);
            var rnd = new Random();
            var delay = (int) Math.Floor(50 + rnd.NextDouble() * 600);
            await Task.Delay(delay, CancellationToken.None);
            _logger.LogInformation(
                "Event {0} was handled...",
                resolvedEvent.OriginalEventNumber);
        }
    }
}