﻿namespace Jynx.Data.Cosmos
{
    public class CosmosOptions
    {
        public const string DefaultKey = "Cosmos";

        public string DatabaseName { get; set; } = "";

        public string Endpoint { get; set; } = "";

        public string PrimaryKey { get; set; } = "";

        public bool CreateContainersIfMissing { get; set; }
    }
}
