﻿using CQRS.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Events
{
    public class BaseEvents : Message
    {
        public BaseEvents(string type)
        {
            this.Type = type;
        }

        public int Version { get; set; }
        public string Type { get; set; }

    }
}
