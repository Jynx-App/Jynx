using Jynx.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace Jynx.Api.Models.Requests
{
    public class CreateDistrictRequest
    {
        [StringLength(32, MinimumLength = 3)]
        [RegularExpression("^[a-z_]+$")]
        public string Id { get; set; } = "";

        [StringLength(200)]
        public string Description { get; set; } = "";

        public District ToEntity()
            => new()
            {
                Id = Id,
                Description = Description,
            };
    }
}
