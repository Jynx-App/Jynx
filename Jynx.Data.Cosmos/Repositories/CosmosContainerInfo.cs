using Jynx.Abstractions.Entities;

namespace Jynx.Data.Cosmos.Repositories
{
    internal class CosmosContainerInfo
    {
        public string Name { get; set; } = "";

        public string PartitionKey { get; set; } = nameof(BaseEntity.Id);

        public int Throughput { get; set; } = 400;
    }
}
