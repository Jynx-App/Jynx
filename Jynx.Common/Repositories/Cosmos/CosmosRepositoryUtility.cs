using Jynx.Common.Repositories.Cosmos.Exceptions;

namespace Jynx.Common.Repositories.Cosmos
{
    public static class CosmosRepositoryUtility
    {
        public static string CreateCompoundId(params string[] parts)
            => string.Join("+", parts);

        public static (string id, string pk) GetIdAndPartitionKeyFromCompoundKey(string compoundId)
        {
            var parts = compoundId.Split("+");

            if (parts.Length != 2)
                throw new InvalidCompoundIdException();

            return (parts[1], parts[0]);
        }
    }
}
