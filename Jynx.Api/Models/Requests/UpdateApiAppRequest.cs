using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Requests
{
    public class UpdateApiAppRequest : ICanPatch<ApiApp>
    {
        public string Id { get; set; } = "";

        public string Name { get; set; } = "";

        public string CallbackUrl { get; set; } = "";

        void ICanPatch<ApiApp>.Patch(ApiApp entity)
        {
            entity.Id = Id;
            entity.Name = Name;
            entity.CallbackUrl = CallbackUrl;
        }
    }
}
