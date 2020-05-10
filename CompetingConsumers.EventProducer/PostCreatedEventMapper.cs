using System;
using CompetingConsumers.Common;
using Newtonsoft.Json;

namespace CompetingConsumers.EventProducer
{
    public class PostCreatedEventMapper : MapperBase<PostCreatedEvent, DomainEvent>
    {
        public override DomainEvent Map(PostCreatedEvent source)
        {
            return source is null
                ? null
                : new DomainEvent(
                    Guid.NewGuid(),
                    JsonConvert.SerializeObject(source.Payload),
                    $"post-{source.Payload.PostId}",
                    "created",
                    JsonConvert.SerializeObject(source.Metadata));
        }
    }
}