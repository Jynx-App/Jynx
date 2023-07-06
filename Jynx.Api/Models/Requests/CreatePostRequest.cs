using Jynx.Abstractions.Entities;
using System.ComponentModel.DataAnnotations;

namespace Jynx.Api.Models.Requests
{
    public class CreatePostRequest
    {
        public string DistrictId { get; set; } = "";

        public string Title { get; set; } = "";

        public string? Body { get; set; }

        public string? Url { get; set; }

        public Post ToEntity()
            => new()
            {
                DistrictId = DistrictId,
                Title = Title,
                Body = Body,
                Url = Url
            };
    }
}
