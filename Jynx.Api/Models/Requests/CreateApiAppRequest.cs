using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Requests
{
    public class CreateApiAppRequest
    {
        public string Name { get; set; } = "";

        public string CallbackUrl { get; set; } = "";

        public ApiApp ToEntity()
            => new()
            {
                Name = Name,
                CallbackUrl = CallbackUrl,
            };
    }
}
