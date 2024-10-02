using CQRS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Infrastructure
{
    public interface IEventStore
    {
        Task SaveEventAsync(Guid AggreageId, IEnumerable<BaseEvents> events, int expectedVersion);
        Task<List<BaseEvents>> GetEventAsync(Guid AggreageId);
        Task<List<Guid>> GetAggregateIdsAsync();
    }
}
