using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventsStoreCollection;

        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value
                .ConnectionString);
            var mongoDataBase  = mongoClient.GetDatabase(config.Value
              .DatabaseName);
            _eventsStoreCollection = mongoDataBase.GetCollection<EventModel>(config.Value.Collection);
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggreageId)
        {
           return await _eventsStoreCollection.Find(x=>x.AggregateIdentifier == aggreageId).ToListAsync().ConfigureAwait(false);
        }

        public async Task SaveAsnyc(EventModel @event)
        {
             await _eventsStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
        }

        public async Task<List<EventModel>> FindAllAsync()
        {
            return await _eventsStoreCollection.Find(_ => true).ToListAsync().ConfigureAwait(false);
        }
    }
}
