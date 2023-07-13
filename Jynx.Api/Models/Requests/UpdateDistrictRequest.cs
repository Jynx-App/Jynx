using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Requests
{
    public class UpdateDistrictRequest : ICanPatch<District>
    {
        public string Id { get; set; } = "";

        public string Description { get; set; } = "";

        void ICanPatch<District>.Patch(District entity)
        {
            entity.Description = Description;
        }
    }
}
