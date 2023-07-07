using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Requests
{
    public class CreateDistrictRequest
    {
        public string Id { get; set; } = "";

        public string Description { get; set; } = "";

        public District ToEntity()
            => new()
            {
                Id = Id,
                Description = Description,
            };
    }
}
