using Jynx.Common.Entities;

namespace Jynx.Api.Models.Requests
{
    public class UpdateUserRequest : ICanPatch<User>
    {
        public string Id { get; set; } = "";

        void ICanPatch<User>.Patch(User entity)
        {
            // More user related stuff will go here later
        }
    }
}
