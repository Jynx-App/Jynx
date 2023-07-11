using Jynx.Abstractions.Entities;

namespace Jynx.Core.Services.Events
{
    public class PinPostEvent
    {
        public PinPostEvent(Post post, bool pin, int numberOfCurrentlyPinnedPosts)
        {
            Post = post;
            Pin = pin;
            NumberOfCurrentlyPinnedPosts = numberOfCurrentlyPinnedPosts;
        }

        public Post Post { get; }

        public bool Pin { get; }
        public int NumberOfCurrentlyPinnedPosts { get; }
        public bool Canceled { get; set; }
    }
}
