using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Jynx.Common.AspNetCore.Mvc.ModelBinding
{
    public static class ModelStateDictionaryExtensions
    {
        public static IEnumerable<string> GetErrors(this ModelStateDictionary modelStateDictionary)
        {
            foreach (var kvp in modelStateDictionary)
            {
                var fieldName = kvp.Key;

                var errorMessage = fieldName == "$" ? "JSON is invalid" : string.Join(",", kvp.Value.Errors.Select(e => e.ErrorMessage));

                yield return errorMessage;
            }
        }
    }
}
