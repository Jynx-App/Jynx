using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Responses
{
    public class GetApiAppResponse
    {
        public GetApiAppResponse(ApiApp apiApp)
        {
            Id = apiApp.Id!;
            Name = apiApp.Name;
            PublicKey = apiApp.PublicKey;
            PrivateKey = apiApp.PrivateKey;
            CallbackUrl = apiApp.CallbackUrl;
        }

        public string Id { get; set; } = "";

        public string Name { get; set; } = "";

        public string PublicKey { get; set; } = "";

        public string PrivateKey { get; set; } = "";

        public string CallbackUrl { get; set; } = "";
    }
}
