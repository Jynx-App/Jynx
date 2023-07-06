namespace Jynx.Abstractions.Entities
{
    public class ApiAppUser : BaseEntity
    {
        public string ApiAppId { get; set; } = "";

        public string UserId { get; set; } = "";
    }
}
