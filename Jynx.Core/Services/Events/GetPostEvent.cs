using Jynx.Abstractions.Entities;

namespace Jynx.Core.Services.Events
{
    internal class GetPostEvent
    {
        public GetPostEvent(Post post)
        {
            Post = post;
        }

        public Post Post { get; }

        public bool Canceled { get; set; }
    }
}
