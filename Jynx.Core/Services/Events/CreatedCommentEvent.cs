using Jynx.Abstractions.Entities;

namespace Jynx.Core.Services.Events
{
    public class CreatedCommentEvent
    {
        public CreatedCommentEvent(Comment comment)
        {
            Comment = comment;
        }

        public Comment Comment { get; }
    }
}
