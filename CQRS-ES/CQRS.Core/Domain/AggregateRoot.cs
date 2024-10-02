﻿using CQRS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _id;
        private readonly List<BaseEvents> _changes = new();
        public Guid Id { get { return _id; } }
        
        public int Version { get; set; } = -1;
        public IEnumerable<BaseEvents> GetUnCommitedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        private void ApplyChange(BaseEvents @event, bool isNew)
        {
            var method = this.GetType().GetMethod("Apply", new Type[] {@event.GetType()});

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method), $"The Apply method was not found in the aggregate for {@event.GetType().Name}");
            }

            method.Invoke(this, new object[] {@event});

            if(isNew)
            {
                _changes.Add( @event );
            }
        }

        protected void RaiseEvents(BaseEvents @event)
        {
            ApplyChange( @event, true );
        }

        public void ReplyEvents(IEnumerable<BaseEvents> events)
        {
            foreach (var @event in events)
            {
                ApplyChange(@event, false);
            }
        }
         
    }
}
