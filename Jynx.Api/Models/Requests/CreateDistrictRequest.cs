using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Requests
{
    public class CreateDistrictRequest : IDistrictRelated
    {
        public string Id { get; set; } = "";

        public string Description { get; set; } = "";
        string IDistrictRelated.DistrictId => Id;

        public District ToEntity()
            => new()
            {
                Id = Id,
                Description = Description,
            };
    }
}
