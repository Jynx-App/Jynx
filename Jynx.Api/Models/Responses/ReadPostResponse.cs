﻿using Jynx.Common.Entities;

namespace Jynx.Api.Models.Responses
{
    public class ReadPostResponse
    {
        public ReadPostResponse(Post post)
        {
            Id = post.Id ?? "";
            Created = post.Created ?? DateTime.MinValue;
            DistrictId = post.DistrictId;
            UserId = post.UserId;
            Edited = post.Edited;
            EditedById = post.EditedById;
            Title = post.Title;
            Body = post.Body;
            CommentsLocked = post.CommentsLocked;
        }

        public string Id { get; set; } = "";

        public DateTime Created { get; set; }

        public DateTime? Edited { get; set; }

        public string DistrictId { get; set; } = "";

        public string UserId { get; set; } = "";

        public string? EditedById { get; set; }

        public string Title { get; set; } = "";

        public string Body { get; set; } = "";

        public bool CommentsLocked { get; set; }
    }
}
