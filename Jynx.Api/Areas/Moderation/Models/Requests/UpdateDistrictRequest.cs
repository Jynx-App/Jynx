using Jynx.Abstractions.Entities;

namespace Jynx.Api.Areas.Moderation.Models.Requests
{
    public class UpdateDistrictRequest : ICanPatch<District>, IDistrictRelated
    {
        public string Id { get; set; } = "";

        public string Description { get; set; } = "";

        string IDistrictRelated.DistrictId => Id;

        void ICanPatch<District>.Patch(District entity)
        {
            entity.Description = Description;
        }
    }
}
