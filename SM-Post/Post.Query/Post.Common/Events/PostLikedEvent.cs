using CQRS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Common.Events
{
    public class PostLikedEvent : BaseEvents
    {
        public PostLikedEvent() : base(nameof(PostLikedEvent))
        {

        }

        public string Author { get; set; }
        public string Message { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
