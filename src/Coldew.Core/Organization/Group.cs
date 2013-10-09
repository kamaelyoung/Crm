using System;
using System.Linq;
using System.Linq.Expressions;
using Coldew.Api.Organization.Exceptions;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Coldew.Data.Organization;
using Coldew.Api.Organization;


namespace Coldew.Core.Organization
{
    /// <summary>
    /// 用户组
    /// </summary>
    public class Group
    {
        public Group(GroupModel groupModel, OrganizationManagement orgMnger)
        {

            if (string.IsNullOrWhiteSpace(groupModel.ID))
            {
                throw new ArgumentNullException("groupInfo.ID");
            }

            if (string.IsNullOrWhiteSpace(groupModel.Name))
            {
                throw new ArgumentNullException("groupInfo.Name");
            }
            if (orgMnger == null)
            {
                throw new ArgumentNullException("orgMnger");
            }

            this.ID = groupModel.ID;
            this.Name = groupModel.Name;
            this.CreateTime = groupModel.CreateTime;
            this.CreatorID = groupModel.CreatorId;
            this.Remark = groupModel.Remark;
            this.GroupType = (GroupType)groupModel.GroupType;
            this._departments = new List<Department>();
            this._groupUsers = new List<User>();
            this._positions = new List<Position>();
            this._groups = new List<Group>();
            this._orgMnger = orgMnger;
        }

        private OrganizationManagement _orgMnger;

        private bool _memberLoaded;

        private List<User> _groupUsers;
        private List<User> _GroupUsers
        {
            get
            {
                if (!this._memberLoaded)
                {
                    lock (this)
                    {
                        if (!this._memberLoaded)
                        {
                            this.LoadMembers();
                            this._memberLoaded = true;
                        }
                    }
                }
                return _groupUsers;
            }
        }

        public ReadOnlyCollection<User> GroupUsers
        {
            get
            {
                return this._GroupUsers
                    .Where(x => x.Status != UserStatus.Logoff)
                    .ToList()
                    .AsReadOnly();
            }
        }

        private List<Department> _departments;
        private List<Department> _Departments
        {
            get
            {
                if (!this._memberLoaded)
                {
                    lock (this)
                    {
                        if (!this._memberLoaded)
                        {
                            this.LoadMembers();
                            this._memberLoaded = true;
                        }
                    }
                }
                return _departments;
            }
        }

        public ReadOnlyCollection<Department> Departments
        {
            get
            {
                return this._Departments.AsReadOnly();
            }
        }

        private List<Position> _positions;
        private List<Position> _Positions
        {
            get
            {
                if (!this._memberLoaded)
                {
                    lock (this)
                    {
                        if (!this._memberLoaded)
                        {
                            this.LoadMembers();
                            this._memberLoaded = true;
                        }
                    }
                }
                return _positions;
            }
        }

        public ReadOnlyCollection<Position> Positions
        {
            get
            {
                return this._Positions.AsReadOnly();
            }
        }

        private List<Group> _groups;
        private List<Group> _Groups
        {
            get
            {
                if (!this._memberLoaded)
                {
                    lock (this)
                    {
                        if (!this._memberLoaded)
                        {
                            this.LoadMembers();
                            this._memberLoaded = true;
                        }
                    }
                }
                return _groups;
            }
        }

        public ReadOnlyCollection<Group> Groups
        {
            get
            {
                return this._Groups.AsReadOnly();
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; private set; }

        /// <summary>
        /// 组名称
        /// </summary>
        public virtual string Name { get; private set; }

        /// <summary>
        /// 是否个人用户组
        /// </summary>
        public virtual Boolean IsPersonal
        {
            get
            {
                return this.GroupType == GroupType.Personal;
            }
        }

        public virtual Boolean IsSystem
        {
            get
            {
                return this.GroupType == GroupType.System;
            }
        }

        public virtual DateTime CreateTime { get; private set; }

        public virtual GroupType GroupType { get; protected set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        private string CreatorID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual User Creator
        {
            get
            {
                
                return this._orgMnger.UserManager.GetUserById(CreatorID);
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; private set; }

        /// <summary>
        /// 修改信息之前
        /// </summary>
        public virtual event TEventHandler<Group, ChangeEventArgs<GroupChangeInfo, GroupInfo, Group>> Changing;

        /// <summary>
        /// 修改信息之后
        /// </summary>
        public virtual event TEventHandler<Group, ChangeEventArgs<GroupChangeInfo, GroupInfo, Group>> Changed;

        public virtual event TEventHandler<Group, ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>> AddedMember;

        public virtual event TEventHandler<Group, ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>> RemovedMember;

        public virtual ReadOnlyCollection<User> Users
        {
            get
            {
                List<User> users = new List<User>();
                var departmentUsers = this._Departments.SelectMany(x => x.Users);
                users.AddRange(departmentUsers);
                var positionUsers = this._Positions.SelectMany(x => x.Users);
                users.AddRange(positionUsers);
                var groupUsers = this._Groups.SelectMany(x => x.Users);
                users.AddRange(groupUsers);
                users.AddRange(this._GroupUsers.Where(x => x.Status != UserStatus.Logoff));

                return users.Distinct().ToList().AsReadOnly();
            }
        }

        private object _updateLockObject = new object();

        public virtual void AddUser(User operationUser, User user)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            if (this._GroupUsers.Contains(user))
            {
                throw new ContainsUserException(user.Name);
            }
            GroupMemberInfo memberInfo = new GroupMemberInfo { GroupId = this.ID, MemberId = user.ID, MemberName = user.Name, MemberType = MemberType.User };

            lock (_updateLockObject)
            {
                this.CreateGroupMemberModel(memberInfo);
                List<User> users = this._GroupUsers.ToList();
                users.Add(user);
                this._groupUsers = users;

                if (this.AddedMember != null)
                {
                    ChangeEventArgs<GroupMemberInfo, GroupInfo, Group> args = new ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>
                    {
                        ChangedSnapshotInfo = this.MapGroupInfo(),
                        ChangeObject = this,
                        ChangeInfo = memberInfo,
                        Operator = operationUser
                    };
                    this.AddedMember(this, args);
                }
            }
        }

        public virtual void RemoveUser(User operationUser, User user)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            GroupMemberInfo memberInfo = new GroupMemberInfo { GroupId = this.ID, MemberId = user.ID, MemberName = user.Name, MemberType = MemberType.User };
            if (this._GroupUsers.Contains(user))
            {
                lock (_updateLockObject)
                {
                    this.DeleteGroupMemberModel(user.ID, MemberType.User);
                    List<User> users = this._GroupUsers.ToList();
                    users.Remove(user);
                    this._groupUsers = users;

                    if (RemovedMember != null)
                    {
                        ChangeEventArgs<GroupMemberInfo, GroupInfo, Group> args = new ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>
                        {
                            ChangedSnapshotInfo = this.MapGroupInfo(),
                            ChangeObject = this,
                            ChangeInfo = memberInfo,
                            Operator = operationUser
                        };
                        this.RemovedMember(this, args);
                    }
                }
            }
        }

        public virtual void AddDepartment(User operationUser, Department department)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (department == null)
            {
                throw new ArgumentNullException("department");
            }
            if (this._Departments.Contains(department))
            {
                throw new ContainsDepartmentException(department.Name);
            }

            GroupMemberInfo memberInfo = new GroupMemberInfo { GroupId = this.ID, MemberId = department.ID, MemberName = department.Name, MemberType = MemberType.Department };

            GroupMemberModel model = new GroupMemberModel
            {
                GroupId = memberInfo.GroupId,
                MemberId = memberInfo.MemberId,
                MemberType = (int)memberInfo.MemberType
            };
            NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();

            lock (_updateLockObject)
            {
                List<Department> departments = this._Departments.ToList();
                departments.Add(department);
                this._departments = departments;

                if (this.AddedMember != null)
                {
                    ChangeEventArgs<GroupMemberInfo, GroupInfo, Group> args = new ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>
                    {
                        ChangedSnapshotInfo = this.MapGroupInfo(),
                        ChangeObject = this,
                        ChangeInfo = memberInfo,
                        Operator = operationUser
                    };
                    this.AddedMember(this, args);
                }
            }
        }

        public virtual void RemoveDepartment(User operationUser, Department department)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (department == null)
            {
                throw new ArgumentNullException("department");
            }
            GroupMemberInfo memberInfo = new GroupMemberInfo { GroupId = this.ID, MemberId = department.ID, MemberName = department.Name, MemberType = MemberType.Department };
           
            if (this._Departments.Contains(department))
            {
                lock (_updateLockObject)
                {
                    this.DeleteGroupMemberModel(department.ID, MemberType.Department);
                    List<Department> departments = this._Departments.ToList();
                    departments.Remove(department);
                    this._departments = departments;

                    if (RemovedMember != null)
                    {
                        ChangeEventArgs<GroupMemberInfo, GroupInfo, Group> args = new ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>
                        {
                            ChangedSnapshotInfo = this.MapGroupInfo(),
                            ChangeObject = this,
                            ChangeInfo = memberInfo,
                            Operator = operationUser
                        };
                        this.RemovedMember(this, args);
                    }
                }
            }
        }

        public virtual void AddPosition(User operationUser, Position position)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (position == null)
            {
                throw new ArgumentNullException("position");
            }
            if (this._Positions.Contains(position))
            {
                throw new ContainsPositionException(position.Name);
            }

            GroupMemberInfo memberInfo = new GroupMemberInfo { GroupId = this.ID, MemberId = position.ID, MemberName = position.Name, MemberType = MemberType.Position };

            this.CreateGroupMemberModel(memberInfo);

            lock (_updateLockObject)
            {
                List<Position> positions = this._Positions.ToList();
                positions.Add(position);
                this._positions = positions;

                if (this.AddedMember != null)
                {
                    ChangeEventArgs<GroupMemberInfo, GroupInfo, Group> args = new ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>
                    {
                        ChangedSnapshotInfo = this.MapGroupInfo(),
                        ChangeObject = this,
                        ChangeInfo = memberInfo,
                        Operator = operationUser
                    };
                    this.AddedMember(this, args);
                }
            }
        }

        public virtual void RemovePoisition(User operationUser, Position position)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (position == null)
            {
                throw new ArgumentNullException("position");
            }
            GroupMemberInfo memberInfo = new GroupMemberInfo { GroupId = this.ID, MemberId = position.ID, MemberName = position.Name, MemberType = MemberType.Position };
            
            if (this._Positions.Contains(position))
            {
                this.DeleteGroupMemberModel(position.ID, MemberType.Position);
                lock (_updateLockObject)
                {
                    List<Position> positions = this._Positions.ToList();
                    positions.Remove(position);
                    this._positions = positions;

                    if (RemovedMember != null)
                    {
                        ChangeEventArgs<GroupMemberInfo, GroupInfo, Group> args = new ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>
                        {
                            ChangedSnapshotInfo = this.MapGroupInfo(),
                            ChangeObject = this,
                            ChangeInfo = memberInfo,
                            Operator = operationUser
                        };
                        this.RemovedMember(this, args);
                    }
                }
            }
        }

        public virtual void AddGroup(User operationUser, Group group)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            if (group == this)
            {
                throw new ContainsSelfGroupException();
            }
            if (this._Groups.Contains(group))
            {
                throw new ContainsGroupException(group.Name);
            }
            if (group.InGroup(this))
            {
                throw new ContainsCircleGroupException(this.Name, group.Name);
            }

            GroupMemberInfo memberInfo = new GroupMemberInfo { GroupId = this.ID, MemberId = group.ID, MemberName = group.Name, MemberType = MemberType.Group };

            this.CreateGroupMemberModel(memberInfo);
            lock (_updateLockObject)
            {
                List<Group> groups = this._Groups.ToList();
                groups.Add(group);
                this._groups = groups;

                if (this.AddedMember != null)
                {
                    ChangeEventArgs<GroupMemberInfo, GroupInfo, Group> args = new ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>
                    {
                        ChangedSnapshotInfo = this.MapGroupInfo(),
                        ChangeObject = this,
                        ChangeInfo = memberInfo,
                        Operator = operationUser
                    };
                    this.AddedMember(this, args);
                }
            }
        }

        public virtual void RemoveGroup(User operationUser, Group group)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            GroupMemberInfo memberInfo = new GroupMemberInfo { GroupId = this.ID, MemberId = group.ID, MemberName = group.Name, MemberType = MemberType.Group };
            if (this._Groups.Contains(group))
            {
                this.DeleteGroupMemberModel(group.ID, MemberType.Group);
                lock (_updateLockObject)
                {
                    List<Group> groups = this._Groups.ToList();
                    groups.Remove(group);
                    this._groups = groups;

                    if (RemovedMember != null)
                    {
                        ChangeEventArgs<GroupMemberInfo, GroupInfo, Group> args = new ChangeEventArgs<GroupMemberInfo, GroupInfo, Group>
                        {
                            ChangedSnapshotInfo = this.MapGroupInfo(),
                            ChangeObject = this,
                            ChangeInfo = memberInfo,
                            Operator = operationUser
                        };
                        this.RemovedMember(this, args);
                    }
                }
            }
        }

        public virtual void ClearMembers(User operationUser)
        {
            List<User> users = this._GroupUsers.ToList();
            users.ForEach(x => this.RemoveUser(operationUser, x));

            List<Department> departments = this._Departments.ToList();
            departments.ForEach(x => this.RemoveDepartment(operationUser, x));

            List<Position> positions = this._Positions.ToList();
            positions.ForEach(x => this.RemovePoisition(operationUser, x));

            List<Group> groups = this._Groups.ToList();
            groups.ForEach(x => this.RemoveGroup(operationUser, x));
        }

        public virtual void Change(User operationUser, GroupChangeInfo changeInfo)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (changeInfo == null)
            {
                throw new ArgumentNullException("groupInfo");
            }
            if (this.IsSystem)
            {
                throw new SystemGroupCannotModifyException();
            }
            if (string.IsNullOrWhiteSpace(changeInfo.Name))
            {
                throw new ArgumentNullException("groupInfo.Name");
            }

            if (changeInfo.Name != this.Name)
            {
                if (this._orgMnger.GroupManager.Groups.Any(x => x.GroupType != GroupType.Personal && x.Name == changeInfo.Name))
                {
                    throw new GroupNameReapeatException();
                }
            }
            ChangeEventArgs<GroupChangeInfo, GroupInfo, Group> args = new ChangeEventArgs<GroupChangeInfo, GroupInfo, Group>
            {
                ChangeInfo = changeInfo,
                ChangeObject = this,
                Operator = operationUser,
                ChangingSnapshotInfo = this.MapGroupInfo()
            };

            if (this.Changing != null)
            {
                this.Changing(this, args);
            }
            GroupModel model = NHibernateHelper.CurrentSession.Get<GroupModel>(this.ID);
            model.Name = changeInfo.Name;
            model.Remark = changeInfo.Remark;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Name = changeInfo.Name;
            this.Remark = changeInfo.Remark;

            if (this.Changed != null)
            {
                args.ChangedSnapshotInfo = this.MapGroupInfo();
                this.Changed(this, args);
            }
        }

        public virtual bool InGroup(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (Users.Contains(user))
            {
                return true;
            }
            return false;
        }

        public virtual bool InGroup(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }

            if (this._Groups.Exists(x => x == group || x.InGroup(group)))
            {
                return true;
            }
            return false;
        }

        protected virtual void LoadMembers()
        {
            List<GroupMemberModel> models = NHibernateHelper.CurrentSession.QueryOver<GroupMemberModel>().Where(x => x.GroupId == this.ID).List().ToList();
            if (models != null)
            {
                models.ForEach(x =>
                {
                    switch ((MemberType)x.MemberType)
                    {
                        case MemberType.User:
                            User user = this._orgMnger.UserManager.GetUserById(x.MemberId);
                            if (user != null)
                            {
                                this._groupUsers.Add(user);
                            }
                            break;
                        case MemberType.Department:
                            Department department = this._orgMnger.DepartmentManager.GetDepartmentById(x.MemberId);
                            if (department != null)
                            {
                                this._departments.Add(department);
                            }
                            break;
                        case MemberType.Position:
                            Position position = this._orgMnger.PositionManager.GetPositionById(x.MemberId);
                            if (position != null)
                            {
                                this._positions.Add(position);
                            }
                            break;
                        case MemberType.Group:
                            Group group = this._orgMnger.GroupManager.GetGroupById(x.MemberId);
                            if (group != null)
                            {
                                this._groups.Add(group);
                            }
                            break;
                    }
                });
            }
        }


        public virtual GroupInfo MapGroupInfo()
        {
            return new GroupInfo
            {
                CreatorId = this.CreatorID,
                CreatorName = this.Creator.Name,
                ID = this.ID,
                IsPersonal = this.IsPersonal,
                GroupType = this.GroupType,
                Name = this.Name,
                Remark = this.Remark,
                IsSystem = this.IsSystem,
                CreateTime = this.CreateTime
            };
        }

        private void CreateGroupMemberModel(GroupMemberInfo memberInfo)
        {
            GroupMemberModel model = new GroupMemberModel
            {
                GroupId = memberInfo.GroupId,
                MemberId = memberInfo.MemberId,
                MemberType = (int)memberInfo.MemberType
            };
            NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        private void DeleteGroupMemberModel(string memberId, MemberType type)
        {
            GroupMemberModel model = NHibernateHelper.CurrentSession.QueryOver<GroupMemberModel>()
                .Where(x => x.GroupId == this.ID && x.MemberId == memberId && x.MemberType == (int)type)
                .SingleOrDefault();
            if (model == null)
            {
                return;
            }
            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();
        }
    }
}
