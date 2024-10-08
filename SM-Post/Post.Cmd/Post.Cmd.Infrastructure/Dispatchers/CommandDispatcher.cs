﻿using CQRS.Core.Commands;
using CQRS.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new Dictionary<Type, Func<BaseCommand, Task>>();
        
        public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
        {
            if(_handlers.ContainsKey(typeof(T)))
            {
                throw new IndexOutOfRangeException("You can not register with the same name");
            }

            _handlers.Add(typeof(T), x => handler((T)x));
        }

        public async Task SendAsync(BaseCommand command)
        {
            if(_handlers.TryGetValue(command.GetType(), out Func<BaseCommand,Task> handler))
            {
                await handler(command);
            }
            else
            {
                throw new ArgumentNullException(nameof(_handlers) , "No command handler is registered.");
            }
        }
    }
}
