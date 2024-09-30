﻿using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig consumerConfig;
        private readonly IEventHandler _eventHandler;

        public EventConsumer(IOptions<ConsumerConfig> config, IEventHandler eventHandler)
        {
            consumerConfig = config.Value;
            _eventHandler = eventHandler;
        }

        public void Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(consumerConfig).SetKeyDeserializer(Deserializers.Utf8).SetValueDeserializer(Deserializers.Utf8).Build(); ;
            consumer.Subscribe(topic);

            while (true)
            {
                var consumerResult = consumer.Consume();
                if(consumerResult?.Message == null) continue;

                var options = new JsonSerializerOptions {  Converters = {new EventJsonConverter()} };
                var @event = JsonSerializer.Deserialize<BaseEvents>(consumerResult.Message.Value, options);
                var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] {@event.GetType()});
                if (handlerMethod == null)
                {
                    throw new ArgumentNullException(nameof(handlerMethod), $"Could not find event handler Method");
                 
                }
                handlerMethod.Invoke(consumerResult.Message, new object[] {@event});
                consumer.Commit(consumerResult);
            }
        }
    }
}
