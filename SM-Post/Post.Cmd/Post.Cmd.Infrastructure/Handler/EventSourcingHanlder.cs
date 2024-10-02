using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producer;
using Post.Cmd.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Handler
{
    public class EventSourcingHanlder : IEventSourceHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore;
        private readonly IEventProducer _eventProducer;

        public EventSourcingHanlder(IEventStore eventStore, IEventProducer eventProducer)
        {
            _eventStore = eventStore;
            _eventProducer = eventProducer;
        }

        public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = new PostAggregate();
            var events = await _eventStore.GetEventAsync(aggregateId);

            if (events == null || !events.Any()) return aggregate;
            aggregate.ReplyEvents(events);
            aggregate.Version = events.Select(x => x.Version).Max();

            return aggregate;
        }

        public async Task RepublishEventsAsync()
        {
            var aggregateIds = await _eventStore.GetAggregateIdsAsync();

            if (aggregateIds == null || !aggregateIds.Any()) return;

            foreach (var aggregateId in aggregateIds)
            {
                var aggregate = await GetByIdAsync(aggregateId);

                if (aggregate == null || !aggregate.Active) continue;

                var events = await _eventStore.GetEventAsync(aggregateId);

                foreach (var @event in events)
                {
                    var topic =  "SocialMediaPostEvents"; // Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                    await _eventProducer.ProduceAsync(topic, @event);
                }
            }
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventAsync(aggregate.Id, aggregate.GetUnCommitedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}
