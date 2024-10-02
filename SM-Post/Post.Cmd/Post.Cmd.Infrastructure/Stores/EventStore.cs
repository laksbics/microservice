using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producer;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepostory;
        private readonly IEventProducer _eventProducer;
        public EventStore(IEventStoreRepository eventStoreRepostory, IEventProducer eventProducer)
        {
            _eventStoreRepostory = eventStoreRepostory;
            _eventProducer = eventProducer;
        }

        public async Task<List<Guid>> GetAggregateIdsAsync()
        {
            var eventStream = await _eventStoreRepostory.FindAllAsync();

            if (eventStream == null || !eventStream.Any())
                throw new ArgumentNullException(nameof(eventStream), "Could not retrieve event stream from the event store!");

            return eventStream.Select(x => x.AggregateIdentifier).Distinct().ToList();
        }

        public async Task<List<BaseEvents>> GetEventAsync(Guid aggreageId)
        {
            var eventStream = await _eventStoreRepostory.FindByAggregateId(aggreageId);

            if(eventStream == null || !eventStream.Any())
            {
                throw new AggregateNotFoundException("Incorrect Post Id provider");
            }

            return eventStream.OrderBy(x=>x.Version).Select(x=>x.EventData).ToList();
        }

        public  async Task  SaveEventAsync(Guid AggreageId, IEnumerable<BaseEvents> events, int expectedVersion)
        {
            var eventStream = await _eventStoreRepostory.FindByAggregateId(AggreageId);

            // eventStream[^1] == eventStream.length -1
            if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
            {
                throw new ConcurrencyException("");
            }

            var version = expectedVersion;

            foreach(var @event in events){
                version++;
                @event.Version = version;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    TimeStamp = DateTime.Now,
                    AggregateIdentifier = AggreageId,
                    AggregateType = nameof(PostAggregate),
                    Version = version,
                    EventType = eventType,
                    EventData = @event
                };

                await _eventStoreRepostory.SaveAsnyc(eventModel);
                var topic = "SocialMediaPostEvents"; // __consumer_offsets"; // Environment.GetEnvironmentVariable("KAFKA_TOPIC");

                try
                {
                    await _eventProducer.ProduceAsync(topic, @event);
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
