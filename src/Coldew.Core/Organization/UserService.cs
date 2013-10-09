using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

using System.Dynamic;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public class UserService : IUserService
    {
        public UserService(ColdewManager crmManger)
        {
            this.OrganizationManager = crmManger.OrgManager;
        }

        public OrganizationManagement OrganizationManager { set; get; }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public UserInfo Create(string operationUserId, UserCreateInfo createInfo)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            User user = this.OrganizationManager.UserManager.Create(opUser, createInfo);
            return user.MapUserInfo();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="userId"></param>
        public void Delete(string operationUserId, string userId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            this.OrganizationManager.UserManager.Delete(opUser, userId);
        }

        public void Lock(string operationUserId, string userId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            user.Lock(opUser);
        }

        public void Unlock(string operationUserId, string userId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            user.Unlock(opUser);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="user"></param>
        public void ChangeInfo(string operationUserId, UserChangeInfo changeInfo)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            User user = this.OrganizationManager.UserManager.GetUserById(changeInfo.ID);
            user.Change(opUser, changeInfo);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        public void ChangePassword(string account, string oldPassword, string newPassword)
        {
            User user = this.OrganizationManager.UserManager.GetUserByAccount(account);
            user.ChangePassword(oldPassword, newPassword);
        }

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserInfo GetUserById(string userId)
        {
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            if (user != null)
            {
                return user.MapUserInfo();
            }
            return null;
        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public UserInfo GetUserByAccount(string userAccount)
        {
            User user = this.OrganizationManager.UserManager.GetUserByAccount(userAccount);
            if (user != null)
            {
                return user.MapUserInfo();
            }
            return null;
        }

        public IList<UserInfo> GetUsersInDepartment(string departmentId)
        {
            Department department = this.OrganizationManager.DepartmentManager.GetDepartmentById(departmentId);
            if (department != null)
            {
                return (from u in department.Users
                        select u.MapUserInfo()).ToList();
            } 
            return null;
        }

        public IList<UserInfo> GetUsersInPosition(string positionId)
        {
            Position position = this.OrganizationManager.PositionManager.GetPositionById(positionId);
            if (position != null)
            {
                return (from u in position.Users
                        select u.MapUserInfo()).ToList();
            }
            return null;
        }

        public IList<string> GetDepartmentIdList(string userId)
        {
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            if (user != null)
            {
                return user.Departments.Select(x => x.ID).ToList();
            }
            return null;
        }

        public IList<DepartmentInfo> GetDepartmentList(string userId)
        {
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            return user.Departments.Select(x => x.MapDepartmentInfo()).ToList();
        }

        public IList<string> GetPositionIdList(string userId)
        {
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            if (user != null)
            {
                return user.Positions.Select(x => x.ID).ToList();
            }
            return null;
        }

        public IList<PositionInfo> GetPositionList(string userId)
        {
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            if (user != null)
            {
                return user.Positions.Select(x => x.MapPositionInfo()).ToList();
            }
            return null;
        }

        public IList<string> GetGroupIdList(string userId)
        {
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            if (user != null)
            {
                return user.Groups.Select(x => x.ID).ToList();
            }
            return null;
        }

        public IList<GroupInfo> GetGroupList(string userId)
        {
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            if (user != null)
            {
                return user.Groups.Select(x => x.MapGroupInfo()).ToList();
            } 
            return null;
        }

        public IList<UserInfo> GetUsersInGroup(string groupId)
        {
            Group group = this.OrganizationManager.GroupManager.GetGroupById(groupId);
            if (group != null)
            {
                return group.Users.Select(x => x.MapUserInfo()).ToList();
            }
            return null;
        }

        public IList<UserInfo> GetAllUser()
        {
            return this.OrganizationManager.UserManager.Users
                .Select(x => x.MapUserInfo())
                .ToList();
        }

        public IList<UserInfo> GetAllNormalUser()
        {
            return this.OrganizationManager.UserManager.Users
                .Where(x => x.Status == UserStatus.Normal && x.Role == UserRole.User)
                .Select(x => x.MapUserInfo())
                .ToList();
        }

        public IList<UserInfo> SearchUser(UserFilterInfo filterInfo)
        {
            IList<UserInfo> userInfos = null;
            switch (filterInfo.OrganizationType)
            {
                case OrganizationType.Company:
                case OrganizationType.Department:

                    DepartmentUserSearcher deparUserFilter = new DepartmentUserSearcher(this.OrganizationManager);
                    deparUserFilter.Account = filterInfo.Account;
                    deparUserFilter.AccountOrName = filterInfo.AccountOrName;
                    deparUserFilter.DepartmentId = filterInfo.OrganizationId;
                    deparUserFilter.Name = filterInfo.Name;
                    deparUserFilter.Recursive = filterInfo.Recursive;

                    userInfos = deparUserFilter.Search().Select(x => x.MapUserInfo()).ToList();
                    break;
                case OrganizationType.GeneralManagerPosition:
                case OrganizationType.ManagerPosition:
                case OrganizationType.Position:
                case OrganizationType.VirtualPosition:

                    PositionUserSearcher positionUserFilter = new PositionUserSearcher(this.OrganizationManager);
                    positionUserFilter.Account = filterInfo.Account;
                    positionUserFilter.PositionId = filterInfo.OrganizationId;
                    positionUserFilter.Name = filterInfo.Name;
                    positionUserFilter.Recursive = filterInfo.Recursive;

                    userInfos = positionUserFilter.Search().Select(x => x.MapUserInfo()).ToList();
                    break;
            }
            if (userInfos == null)
            {
                userInfos = new List<UserInfo>();
            }
            return userInfos;
        }

        public IList<UserInfo> SearchUser(string accountOrName)
        {
            List<User> users = this.OrganizationManager.UserManager.Search(accountOrName);
            users.RemoveAll(delegate(User x)
            {
                if (x.Account.ToLower() == "system")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
            if (users != null)
            {
                return users.Select(x => x.MapUserInfo()).ToList();
            }
            return null;
        }

        public void Logoff(string operationUserId, string userId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            user.Logoff(opUser);
            this.OrganizationManager.UserPositionManager.Delete(opUser, userId);
            this.OrganizationManager.UserPositionManager.Create(opUser,
                new UserPositionInfo { PositionId = this.OrganizationManager.PositionManager.TopPosition.ID, UserId = userId, Main = true });
        }

        public void Activate(string operationUserId, string userId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            user.Activate(opUser);
        }

        public IList<UserInfo> GetLogoffedUsers()
        {
            return this.OrganizationManager.UserManager
                .Users
                .Where(x => x.Status == UserStatus.Logoff)
                .Select(x => x.MapUserInfo())
                .ToList();
        }

        public void ImportPassword(string operationUserId, string userId, string password)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            if (user != null)
            {
                user.ImportPassword(opUser, password);
            }
        }

        public void ChangeSignInInfo(string operationUserId, UserSignInChangeInfo changeInfo)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            User user = this.OrganizationManager.UserManager.GetUserById(changeInfo.ID);
            user.ChangeSignInInfo(opUser, changeInfo);
        }

        public UserInfo GetSystem()
        {
            return this.OrganizationManager.System.MapUserInfo();
        }

        public int GetUsersCount()
        {
            return this.OrganizationManager.UserManager.Users.Where(x => x.Status != UserStatus.Logoff && x.Role == UserRole.User).Count();
        }

        public List<UserInfo> GetAllUnderling(string userId)
        {
            List<UserInfo> underlings = new List<UserInfo>();
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            if (user != null)
            {
                IEnumerable<User> users = this.OrganizationManager.UserManager.Users.Where(x => x.Role == UserRole.User && x.Status == UserStatus.Normal);
                if (user.Role == UserRole.Administrator)
                {
                    return users.Select(x => x.MapUserInfo()).ToList();
                }
                foreach (User user1 in users)
                {
                    if (user1 == user)
                    {
                        continue;
                    }
                    if (user1.IsMySuperior(user, true))
                    {
                        underlings.Add(user1.MapUserInfo());
                    }
                }
            }
            return underlings;
        }

        public void ResetPassword(string operationUserId, string userId, string newPassword)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            User user = this.OrganizationManager.UserManager.GetUserById(userId);
            if (user != null)
            {
                user.ResetPassword(opUser, newPassword);
            }
        }
    }
}