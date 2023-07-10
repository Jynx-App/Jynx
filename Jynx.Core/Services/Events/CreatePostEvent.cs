using Jynx.Abstractions.Entities;

namespace Jynx.Core.Services.Events
{
    internal class CreatePostEvent
    {
        public CreatePostEvent(Post post)
        {
            Post = post;
        }

        public Post Post { get; }

        public bool Canceled { get; set; }
    }
}
