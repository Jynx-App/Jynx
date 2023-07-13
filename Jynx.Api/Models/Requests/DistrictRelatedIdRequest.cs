namespace Jynx.Api.Models.Requests
{
    public class DistrictRelatedIdRequest : IdRequest, IDistrictRelated
    {
        public string DistrictId => Id.Split('.').FirstOrDefault() ?? Id;
    }
}
