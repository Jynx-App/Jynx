using Jynx.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace Jynx.Api.Models.Requests
{
    public class CreatePostRequest : IValidatableObject
    {
        [StringLength(36)]
        public string DistrictId { get; set; } = "";

        [StringLength(100)]
        public string Title { get; set; } = "";

        [StringLength(40000)]
        public string? Body { get; set; }

        [StringLength(2048)]
        public string? Url { get; set; }

        public Post ToEntity()
            => new()
            {
                DistrictId = DistrictId,
                Title = Title,
                Body = Body,
                Url = Url
            };

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Body) && string.IsNullOrWhiteSpace(Url))
                yield return new ValidationResult("Must provide Body or Url");

            if (!string.IsNullOrWhiteSpace(Body) && !string.IsNullOrWhiteSpace(Url))
                yield return new ValidationResult("Must only provide Body or Url");
        }
    }
}
