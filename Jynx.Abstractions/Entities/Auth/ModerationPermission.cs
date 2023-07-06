namespace Jynx.Abstractions.Entities.Auth
{
    public enum ModerationPermission
    {
        EditDistrict,
        ViewMail,

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
        RemoveFromUserGroups,
    }
}
