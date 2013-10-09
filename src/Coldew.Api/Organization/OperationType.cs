namespace Coldew.Api.Organization
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        AddGroup = 1,

        DeleteGroup,

        ModifyGroup,

        AddUser,

        LogoutUser,

        ModifyUser,

        AddDeparment,

        DeleteDeparment,

        ModifyDeparment,

        AddPosition,

        DeletePosition,

        ModifyPosition,

        AddLeaderPosition,

        RemoveLeaderPosition,

        AddUserToPosition,

        RemoveUserFromPosition,

        AddMemberToGroup,

        RemoveMemberFromGroup,

        ModifyPasswordStrategy,

        LockUser,

        ActivateUser,

        ResetPassword,

        ModifyLoginStrategy,
    }
}
