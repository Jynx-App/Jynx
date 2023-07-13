using Jynx.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jynx.Core.Services.Events
{
    public class CreatedPostEvent
    {
        public CreatedPostEvent(Post post)
        {
            Post = post;
        }

        public Post Post { get; }
    }
}
