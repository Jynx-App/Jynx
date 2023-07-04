using System.ComponentModel.DataAnnotations;

namespace Jynx.Api.Models.Requests
{
    public class IdRequest
    {
        [StringLength(100)]
        public string Id { get; set; } = "";
    }
}
