﻿using Jynx.Abstractions.Entities;

namespace Jynx.Core.Services.Events
{
    public class PinCommentEvent
    {
        public PinCommentEvent(Comment comment, bool pin, int numberOfCurrentlyPinnedComments)
        {
            Comment = comment;
            Pin = pin;
            NumberOfCurrentlyPinnedComments = numberOfCurrentlyPinnedComments;
        }

        public Comment Comment { get; }

        public bool Pin { get; }
        public int NumberOfCurrentlyPinnedComments { get; }
        public bool Canceled { get; set; }
    }
}
