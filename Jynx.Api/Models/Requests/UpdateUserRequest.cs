using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Requests
{
    public class UpdateUserRequest : ICanPatch<User>
    {
        void ICanPatch<User>.Patch(User entity)
        {
            // More user related stuff will go here later
        }
    }
}
