﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Events
{
    public class EventModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid AggregateIdentifier { get; set; }
        public string AggregateType { get; set; }
        public int Version { get; set; }
        public string EventType { get; set; }
        public BaseEvents EventData { get; set; }

        
    }
}
