using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Responses
{
    public class GetDistrictResponse
    {
        public GetDistrictResponse(District district)
        {
            Id = district.Id!;
            Description = district.Description;
        }

        public string Id { get; set; } = "";

        public string Description { get; set; } = "";
    }
}
