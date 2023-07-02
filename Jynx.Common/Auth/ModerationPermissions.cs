namespace Jynx.Common.Auth
{
    public enum ModerationPermissions
    {
        None,

        ApprovePosts,
        EditPosts,
        DeletePosts,

        ApproveComments,
        EditComments,
        DeleteComments,

        ApproveUsers,
        BanUsers,

        AddModerators,
        RemoveModerators,

        AddUserGroups,
        EditUserGroups,
        RemoveUserGroups,

        AddToUserGroups,
        RemoveFromuserGroups,
    }
}
