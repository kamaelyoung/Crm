using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api.Organization;

namespace Crm.Core.Organization
{
    public class GroupService : IGroupService
    {
        public GroupService(OrganizationManagement organizationManager)
        {
            this.OrganizationManager = organizationManager;
        }

        public OrganizationManagement OrganizationManager { set; get; }

        public GroupInfo Create(string operationUserId, GroupCreateInfo createInfo)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Group group = this.OrganizationManager.GroupManager.Create(opUser, createInfo);
            return group.MapGroupInfo();
        }

        public void DeleteGroupById(string operationUserId, string groupId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            this.OrganizationManager.GroupManager.Delete(opUser, groupId);
        }

        public void ChangeGroupInfo(string operationUserId, GroupChangeInfo changeInfo)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Group group = this.OrganizationManager.GroupManager.GetGroupById(changeInfo.ID);
            group.Change(opUser, changeInfo);
        }

        public void AddUserToGroup(string operationUserId, string groupId, string userId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Group group = this.OrganizationManager.GroupManager.GetGroupById(groupId);
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            group.AddUser(opUser, user);
        }

        public void RemoveUserFromGroup(string operationUserId, string groupId, string userId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Group group = this.OrganizationManager.GroupManager.GetGroupById(groupId);
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            group.RemoveUser(opUser, user);
        }

        public void AddPositionToGroup(string operationUserId, string groupId, string positionId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Group group = this.OrganizationManager.GroupManager.GetGroupById(groupId);
            Position position = this.OrganizationManager.PositionManager.GetPositionById(positionId);
            group.AddPosition(opUser, position);
        }

        public void RemovePositionFromGroup(string operationUserId, string groupId, string positionId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Group group = this.OrganizationManager.GroupManager.GetGroupById(groupId);
            Position position = this.OrganizationManager.PositionManager.GetPositionById(positionId);
            group.RemovePoisition(opUser, position);
        }

        public void AddDepartmentToGroup(string operationUserId, string groupId, string departmentId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Group group = this.OrganizationManager.GroupManager.GetGroupById(groupId);
            Department department = this.OrganizationManager.DepartmentManager.GetDepartmentById(departmentId);
            group.AddDepartment(opUser, department);
        }

        public void RemoveDepartmentFromGroup(string operationUserId, string groupId, string departmentId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Group group = this.OrganizationManager.GroupManager.GetGroupById(groupId);
            Department department = this.OrganizationManager.DepartmentManager.GetDepartmentById(departmentId);
            group.RemoveDepartment(opUser, department);
        }

        public void AddGroupToGroup(string operationUserId, string toGroupId, string groupId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Group toGroup = this.OrganizationManager.GroupManager.GetGroupById(toGroupId);
            Group group = this.OrganizationManager.GroupManager.GetGroupById(groupId);
            toGroup.AddGroup(opUser, group);
        }

        public void RemoveGroupFromGroup(string operationUserId, string fromGroupId, string groupId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            Group fromGroup = this.OrganizationManager.GroupManager.GetGroupById(fromGroupId);
            Group group = this.OrganizationManager.GroupManager.GetGroupById(groupId);
            fromGroup.RemoveGroup(opUser, group);
        }

        public GroupInfo GetGroupById(string groupId)
        {
            Group group = this.OrganizationManager.GroupManager.GetGroupById(groupId);
            if (group != null)
            {
                return group.MapGroupInfo();
            }
            return null;
        }

        public IList<GroupInfo> GetGroups()
        {
            return this.OrganizationManager.GroupManager.Groups
                .Where(x => !x.IsPersonal)
                .Select(x => x.MapGroupInfo())
                .ToList();
        }

        public IList<GroupInfo> GetPersonalGroups(string userId)
        {
            return this.OrganizationManager.GroupManager.Groups.
                Where(x => x.IsPersonal && x.Creator.ID == userId)
                .Select(x => x.MapGroupInfo())
                .ToList();
        }

        public IList<GroupMemberInfo> GetGroupMembersByGroupId( string groupId)
        {
            Group group = this.OrganizationManager.GroupManager.GetGroupById(groupId);

            return group.GroupUsers.Select(x => new GroupMemberInfo { GroupId = groupId, MemberId = x.ID, MemberName = x.Name, MemberType = MemberType.User })
                .Concat(group.Departments.Select(x => new GroupMemberInfo { GroupId = groupId, MemberId = x.ID, MemberName = x.Name, MemberType = MemberType.Department }))
                .Concat(group.Positions.Select(x => new GroupMemberInfo { GroupId = groupId, MemberId = x.ID, MemberName = x.Name, MemberType = MemberType.Position }))
                .Concat(group.Groups.Select(x => new GroupMemberInfo { GroupId = groupId, MemberId = x.ID, MemberName = x.Name, MemberType = MemberType.Group }))
                .ToList();
        }

        public void AddMemberToGroup(string operationUserId, GroupMemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberType.User)
            {
                this.AddUserToGroup(operationUserId, memberInfo.GroupId, memberInfo.MemberId);
            }
            else if (memberInfo.MemberType == MemberType.Position)
            {
                this.AddPositionToGroup(operationUserId, memberInfo.GroupId, memberInfo.MemberId);
            }
            else if (memberInfo.MemberType == MemberType.Department)
            {
                this.AddDepartmentToGroup(operationUserId, memberInfo.GroupId, memberInfo.MemberId);
            }
            else if (memberInfo.MemberType == MemberType.Group)
            {
                this.AddGroupToGroup(operationUserId, memberInfo.GroupId, memberInfo.MemberId);
            }
        }

        public void RemoveMemberFromGroup(string operationUserId, GroupMemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberType.User)
            {
                this.RemoveUserFromGroup(operationUserId, memberInfo.GroupId, memberInfo.MemberId);
            }
            else if (memberInfo.MemberType == MemberType.Position)
            {
                this.RemovePositionFromGroup(operationUserId, memberInfo.GroupId, memberInfo.MemberId);
            }
            else if (memberInfo.MemberType == MemberType.Department)
            {
                this.RemoveDepartmentFromGroup(operationUserId, memberInfo.GroupId, memberInfo.MemberId);
            }
            else if (memberInfo.MemberType == MemberType.Group)
            {
                this.RemoveGroupFromGroup(operationUserId, memberInfo.GroupId, memberInfo.MemberId);
            }
        }


        public IList<GroupInfo> GetAllPersonalGroups()
        {
            return this.OrganizationManager.GroupManager.Groups.
                   Where(x => x.IsPersonal)
                   .Select(x => x.MapGroupInfo())
                   .ToList();
        }

        public IList<GroupInfo> SearchByName(string name)
        {
            var groupList = this.OrganizationManager.GroupManager.SearchByName(name);
            var result = new List<GroupInfo>();
            if (groupList != null && groupList.Count > 0)
                result.AddRange(from groupInfo in groupList where groupInfo != null select groupInfo.MapGroupInfo());
            return result;
        }
    }
}