namespace Jynx.Common.Entities
{
    public class ApiApp : BaseEntity
    {
        public string UserId { get; set; } = "";

        public string Name { get; set; } = "";

        public string PublicKey { get; set; } = "";

        public string PrivateKey { get; set; } = "";

        public string CallbackUrl { get; set; } = "";
    }
}
