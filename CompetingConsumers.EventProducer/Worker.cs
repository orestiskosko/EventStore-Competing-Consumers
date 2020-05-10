using System;
using System.Threading;
using System.Threading.Tasks;
using CompetingConsumers.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CompetingConsumers.EventProducer
{
    public class Worker : BackgroundService
    {
        private readonly IMapper<PostCreatedEvent, DomainEvent> _eventMapper;
        private readonly IDomainEventProducer _eventProducer;
        private readonly ILogger<Worker> _logger;

        public Worker(
            ILogger<Worker> logger,
            IDomainEventProducer eventProducer,
            IMapper<PostCreatedEvent, DomainEvent> eventMapper)
        {
            _logger = logger;
            _eventProducer = eventProducer;
            _eventMapper = eventMapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PostCreatedEvent postEvent;
            PostCreatedEventPayload payload;

            while (!stoppingToken.IsCancellationRequested)
            {
                payload = new PostCreatedEventPayload
                {
                    PostId = Guid.NewGuid().ToString(),
                    UserId = Guid.NewGuid().ToString(),
                    Content = ""
                };

                postEvent = new PostCreatedEvent(
                    Guid.NewGuid().ToString(),
                    payload,
                    new DomainEventMetadata("ProducerWorker"));

                var domainEvent = _eventMapper.Map(postEvent);

                await _eventProducer.Produce(domainEvent, stoppingToken);
                _logger.LogInformation("New post created with id {0}...", postEvent.Id);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}