using Jynx.Abstractions.Entities;

namespace Jynx.Core.Services.Events
{
    public class CreatingCommentEvent
    {
        public CreatingCommentEvent(Comment comment)
        {
            Comment = comment;
        }

        public Comment Comment { get; }

        public bool Canceled { get; set; }
    }
}
