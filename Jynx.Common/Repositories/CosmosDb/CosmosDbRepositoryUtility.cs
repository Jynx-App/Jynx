using Jynx.Common.Repositories.CosmosDb.Exceptions;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal static class CosmosDbRepositoryUtility
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
