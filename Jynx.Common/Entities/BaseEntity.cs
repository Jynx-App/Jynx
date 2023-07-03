using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jynx.Common.Entities
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class BaseEntity
    {
        public string? Id { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Edited { get; set; }
    }
}
