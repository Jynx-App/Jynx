using Jynx.Abstractions.Entities;

namespace Jynx.Core.Services.Events
{
    internal class CreatingPostEvent
    {
        public CreatingPostEvent(Post post)
        {
            Post = post;
        }

        public Post Post { get; }

        public bool Canceled { get; set; }
    }
}
