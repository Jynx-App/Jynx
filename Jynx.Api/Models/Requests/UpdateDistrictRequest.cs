using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Requests
{
    public class UpdateDistrictRequest : DistrictRelatedIdRequest, ICanPatch<District>
    {
        public string Description { get; set; } = "";

        void ICanPatch<District>.Patch(District entity)
        {
            entity.Description = Description;
        }
    }
}
