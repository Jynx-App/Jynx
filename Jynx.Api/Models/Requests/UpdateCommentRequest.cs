﻿using Jynx.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace Jynx.Api.Models.Requests
{
    public class UpdateCommentRequest : ICanPatch<Comment>
    {
        public string Id { get; set; } = "";

        public string Body { get; set; } = "";

        void ICanPatch<Comment>.Patch(Comment entity)
        {
            entity.Body = Body;
        }
    }
}
