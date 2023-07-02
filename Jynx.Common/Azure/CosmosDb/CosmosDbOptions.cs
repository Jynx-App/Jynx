namespace Jynx.Common.Azure.CosmosDb
{
    public class CosmosDbOptions
    {
        public const string DefaultKey = "CosmosDb";

        public string DatabaseName { get; set; } = "";

        public string Endpoint { get; set; } = "";

        public string PrimaryKey { get; set; } = "";
    }
}
