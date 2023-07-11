using Jynx.Abstractions.Entities;

namespace Jynx.Core.Services.Events
{
    public class PinPostEvent
    {
        public PinPostEvent(Post post, bool pin)
        {
            Post = post;
            Pin = pin;
        }

        public Post Post { get; }

        public bool Pin { get; }

        public bool Canceled { get; set; }
    }
}
