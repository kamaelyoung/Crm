namespace Crm.Api.Organization
{
    /// <summary>
    /// 成员类型
    /// </summary>
    public enum MemberType
    {
        /// <summary>
        /// 用户
        /// </summary>
        User,

        /// <summary>
        /// Owner
        /// </summary>
        Owners,

        /// <summary>
        /// 创建人
        /// </summary>
        Creators,

        /// <summary>
        /// 用户组
        /// </summary>
        Group,

        /// <summary>
        /// 职位
        /// </summary>
        Position,

        /// <summary>
        /// 部门
        /// </summary>
        Department,

        /// <summary>
        /// 所有人（Everyone）
        /// </summary>
        Everyone,

        /// <summary>
        /// 访客（未登录用户）
        /// </summary>
        Guests,

        Contact,
    }
}
