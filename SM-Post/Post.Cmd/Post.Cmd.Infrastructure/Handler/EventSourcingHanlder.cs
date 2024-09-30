using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
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
        public async Task<PostAggregate> GetByIdAsnyuc(Guid aggregateId)
        {
             var aggregate = new PostAggregate();
             var events = await _eventStore.GetEventAsync(aggregateId);
              
             if(events == null || !events.Any())
             {
                return aggregate;
             }

            aggregate.ReplyEvents(events);
            var latestVersion = events.Select(e => e.Version).Max();
            aggregate.Version = latestVersion;
            return aggregate;
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
             await _eventStore.SaveEventAsync(aggregate.Id, aggregate.GetUnCommitedChanges(), aggregate.Version);
             aggregate.MarkChanges();
        }
    }
}
