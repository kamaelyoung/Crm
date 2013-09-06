using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data.Organization;
using Crm.Api.Organization;

namespace Crm.Core.Organization
{
    public class OperationLogManagement
    {
        public OperationLogManagement(OrganizationManagement orgMnger)
        {
            this._orgMnger = orgMnger;

            orgMnger.UserManager.Loaded +=
                new TEventHanlder<UserManagement, List<User>>(this.UserService_OnLoaded);
            orgMnger.UserManager.Created += new TEventHanlder<UserManagement, CreateEventArgs<UserCreateInfo, UserInfo, User>>(UserManager_Created);

            orgMnger.PositionManager.Loaded += new TEventHanlder<PositionManagement, List<Position>>(this.PositionManager_Loaded);
            orgMnger.PositionManager.Created += new TEventHanlder<PositionManagement, CreateEventArgs<PositionCreateInfo, PositionInfo, Position>>(PositionManager_Created);
            orgMnger.PositionManager.Deleted += new TEventHanlder<PositionManagement, DeleteEventArgs<Position>>(PositionManager_Deleted);

            orgMnger.DepartmentManager.Loaded += new TEventHanlder<DepartmentManagement, List<Department>>(this.DepartmentManager_OnLoaded);
            orgMnger.DepartmentManager.Created += new TEventHanlder<DepartmentManagement, CreateEventArgs<DepartmentCreateInfo, DepartmentInfo, Department>>(DepartmentManager_Created);
            orgMnger.DepartmentManager.Deleted += new TEventHanlder<DepartmentManagement, DeleteEventArgs<Department>>(DepartmentManager_Deleted);

            orgMnger.GroupManager.Loaded += new TEventHanlder<GroupManagement, List<Group>>(this.GroupManager_Loaded);
            orgMnger.GroupManager.Created += new TEventHanlder<GroupManagement, CreateEventArgs<GroupCreateInfo, GroupInfo, Group>>(GroupManager_Created);
            orgMnger.GroupManager.Deleted += new TEventHanlder<GroupManagement, DeleteEventArgs<Group>>(GroupManager_Deleted);

            orgMnger.UserPositionManager.Created += new TEventHanlder<UserPositionManagement, UserPositionOperationArgs>(UserPositionManager_Created);
            orgMnger.UserPositionManager.Deleted += new TEventHanlder<UserPositionManagement, UserPositionOperationArgs>(UserPositionManager_Deleted);

        }

        OrganizationManagement _orgMnger;

        #region User

        void UserService_OnLoaded(UserManagement sender, List<User> args)
        {
            args.ForEach(x =>
            {
                x.Changed += new TEventHanlder<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_Modifyed);
                x.Logoffed += new TEventHanlder<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_Logoffed);
                x.Locked += new TEventHanlder<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_Locked);
                x.Activated += new TEventHanlder<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_Activated);
                x.PasswordReseted += new TEventHanlder<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_PasswordReseted);
            });
        }

        void UserManager_Created(UserManagement sender, CreateEventArgs<UserCreateInfo, UserInfo, User> args)
        {
            User user = sender.GetUserById(args.CreatedObject.ID);
            user.Changed += new TEventHanlder<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_Modifyed);
            user.Logoffed += new TEventHanlder<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_Logoffed);
            user.Locked += new TEventHanlder<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_Locked);
            user.Activated += new TEventHanlder<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_Activated);
            user.PasswordReseted += new TEventHanlder<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_PasswordReseted);

            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("创建了用户{0}",
                    args.CreatedObject.Name),
                OperationType = OperationType.AddUser,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        private void SaveOperation(OperationLogInfo opInfo)
        {
            OperationLogModel operationModel = new OperationLogModel
                {
                    OperationContent = opInfo.OperationContent,
                    OperationTime = opInfo.OperationTime,
                    OperationType = (int)opInfo.OperationType,
                    OperatorId = opInfo.OperatorId,
                    OperatorName = opInfo.OperatorName
                };

            NHibernateHelper.CurrentSession.Save(operationModel);
        }

        void User_Modifyed(User sender, ChangeEventArgs<UserChangeInfo, UserInfo, User> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("修改了用户{0}信息",
                    args.ChangingSnapshotInfo.Name),
                OperationType = OperationType.ModifyUser,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void User_Logoffed(User sender, ChangeEventArgs<UserChangeInfo, UserInfo, User> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("注销了用户{0}",
                    args.ChangingSnapshotInfo.Name),
                OperationType = OperationType.LogoutUser,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void User_Locked(User sender, ChangeEventArgs<UserChangeInfo, UserInfo, User> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("锁定了用户{0}",
                    args.ChangingSnapshotInfo.Name),
                OperationType = OperationType.LockUser,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void User_Activated(User sender, ChangeEventArgs<UserChangeInfo, UserInfo, User> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("激活了用户{0}",
                    args.ChangingSnapshotInfo.Name),
                OperationType = OperationType.ActivateUser,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void User_PasswordReseted(User sender, ChangeEventArgs<UserChangeInfo, UserInfo, User> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("重置了用户{0}的密码",
                    args.ChangingSnapshotInfo.Name),
                OperationType = OperationType.ResetPassword,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }
        #endregion

        #region Position

        void PositionManager_Loaded(PositionManagement sender, List<Position> args)
        {
            args.ForEach(x =>
            {
                x.Changed += new TEventHanlder<Position, ChangeEventArgs<PositionChangeInfo, PositionInfo, Position>>(Position_Modifyed);
            });
        }

        void PositionManager_Created(PositionManagement sender, CreateEventArgs<PositionCreateInfo, PositionInfo, Position> args)
        {
            Position position = args.CreatedObject;
            position.Changed += new TEventHanlder<Position, ChangeEventArgs<PositionChangeInfo, PositionInfo, Position>>(Position_Modifyed);

            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("创建了职位{0}",
                    args.CreatedObject.Name),
                OperationType = OperationType.AddPosition,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void PositionManager_Deleted(PositionManagement sender, DeleteEventArgs<Position> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("删除了职位{0}",
                    args.DeleteObject.Name),
                OperationType = OperationType.DeletePosition,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };

            this.SaveOperation(opInfo);
        }

        void Position_Modifyed(Position sender, ChangeEventArgs<PositionChangeInfo, PositionInfo, Position> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("修改了职位{0}信息",
                    sender.Name),
                OperationType = OperationType.ModifyPosition,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void Position_RemovedLeaderPosition(Position sender, ChangeEventArgs<PositionChangeInfo, PositionInfo, Position> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("移除了职位{0}的其他上级{1}",
                    sender.Name, args.ChangeObject.Name),
                OperationType = OperationType.RemoveLeaderPosition,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void Position_AddedLeaderPosition(Position sender, ChangeEventArgs<PositionChangeInfo, PositionInfo, Position> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("将职位{0}设置为了职位{1}的上级",
                    args.ChangeObject.Name, sender.Name),
                OperationType = OperationType.AddLeaderPosition,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }
        #endregion

        #region Department

        void DepartmentManager_OnLoaded(DepartmentManagement sender, List<Department> args)
        {
            args.ForEach(x =>
            {
                x.Changed += new TEventHanlder<Department, ChangeEventArgs<DepartmentChangeInfo, DepartmentInfo, Department>>(Department_Modifyed);
            });
        }

        void DepartmentManager_Deleted(DepartmentManagement sender, DeleteEventArgs<Department> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("删除了部门{0}",
                    args.DeleteObject.Name),
                OperationType = OperationType.DeleteDeparment,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void Department_Modifyed(Department sender, ChangeEventArgs<DepartmentChangeInfo, DepartmentInfo, Department> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("修改了部门{0}信息",
                    args.ChangingSnapshotInfo.Name),
                OperationType = OperationType.ModifyDeparment,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void DepartmentManager_Created(DepartmentManagement sender, CreateEventArgs<DepartmentCreateInfo, DepartmentInfo, Department> args)
        {
            Department department = args.CreatedObject;
            department.Changed += new TEventHanlder<Department, ChangeEventArgs<DepartmentChangeInfo, DepartmentInfo, Department>>(Department_Modifyed);
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("创建了部门{0}",
                    args.CreatedObject.Name),
                OperationType = OperationType.AddDeparment,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }
        #endregion

        #region Group

        void GroupManager_Loaded(GroupManagement sender, List<Group> args)
        {
            args.ForEach(x =>
            {
                x.AddedMember += new TEventHanlder<Group, ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>>(Group_AddedMember);
                x.RemovedMember += new TEventHanlder<Group, ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>>(Group_RemovedMember);
                x.Changed += new TEventHanlder<Group, ChangeEventArgs<GroupChangeInfo, GroupInfo, Group>>(Group_Modifyed);
            });
        }

        void Group_RemovedMember(Group sender, ChangeEventArgs<GroupMemberInfo, GroupInfo, Group> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("从用户组{0}移除成员{1}", sender.Name, args.ChangeInfo.MemberName),
                OperationType = OperationType.RemoveMemberFromGroup,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void Group_AddedMember(Group sender, ChangeEventArgs<GroupMemberInfo, GroupInfo, Group> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("添加成员{0}到用户组{1}", args.ChangeInfo.MemberName, sender.Name),
                OperationType = OperationType.AddMemberToGroup,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void Group_Modifyed(Group sender, ChangeEventArgs<GroupChangeInfo, GroupInfo, Group> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("修改了用户组{0}", sender.Name),
                OperationType = OperationType.ModifyGroup,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void GroupManager_Deleted(GroupManagement sender, DeleteEventArgs<Group> args)
        {
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("删除了用户组{0}", args.DeleteObject.Name),
                OperationType = OperationType.DeleteGroup,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        void GroupManager_Created(GroupManagement sender, CreateEventArgs<GroupCreateInfo, GroupInfo, Group> args)
        {
            Group group = args.CreatedObject;
            group.AddedMember += new TEventHanlder<Group, ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>>(Group_AddedMember);
            group.RemovedMember += new TEventHanlder<Group, ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>>(Group_RemovedMember);
            group.Changed += new TEventHanlder<Group, ChangeEventArgs<GroupChangeInfo, GroupInfo, Group>>(Group_Modifyed);
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("创建了用户组{0}", args.CreatedObject.Name),
                OperationType = OperationType.AddGroup,
                OperatorId = args.Operator.ID,
                OperatorName = args.Operator.Name
            };
            this.SaveOperation(opInfo);
        }

        #endregion

        #region UserPosition

        void UserPositionManager_Deleted(UserPositionManagement sender, UserPositionOperationArgs args)
        {
            User user = this._orgMnger.UserManager.GetUserById(args.UserPositionInfo.UserId);
            Position position = this._orgMnger.PositionManager.GetPositionById(args.UserPositionInfo.PositionId);
            if (user == null || position == null)
            {
                return;
            }
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("删除了用户{0}的{1}职位",
                    user.Name, position.Name),
                OperationType = OperationType.RemoveUserFromPosition,
                OperatorId = args.OperationUser.ID,
                OperatorName = args.OperationUser.Name
            };
            this.SaveOperation(opInfo);
        }

        void UserPositionManager_Created(UserPositionManagement sender, UserPositionOperationArgs args)
        {
            User user = this._orgMnger.UserManager.GetUserById(args.UserPositionInfo.UserId);
            Position position = this._orgMnger.PositionManager.GetPositionById(args.UserPositionInfo.PositionId);
            if (user == null || position == null)
            {
                return;
            }
            OperationLogInfo opInfo = new OperationLogInfo
            {
                OperationTime = DateTime.Now,
                OperationContent = string.Format("添加用户{0}到{1}职位",
                    user.Name, position.Name),
                OperationType = OperationType.AddUserToPosition,
                OperatorId = args.OperationUser.ID,
                OperatorName = args.OperationUser.Name
            };
            this.SaveOperation(opInfo);
        }

        #endregion
    }
}
