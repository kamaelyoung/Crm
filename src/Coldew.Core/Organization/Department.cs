using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using Coldew.Api.Organization.Exceptions;

using Coldew.Data.Organization;
using Coldew.Api.Organization;


namespace Coldew.Core.Organization
{
    public class Department : Member
    {
        OrganizationManagement _orgMnger;

        public Department(OrganizationManagement orgMnger, string id, string name, Position managerPosition, string remark)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("departmentInfo.ID");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            if (managerPosition == null)
            {
                throw new ArgumentNullException("managerPosition");
            }
            this.ManagerPosition = managerPosition;
            this._orgMnger = orgMnger;
            this.ID = id;
            this.Name = name;
            this.Remark = remark;
        }

        /// <summary>
        /// 上级部门
        /// </summary>
        public virtual Department Parent 
        {
            get
            {
                if (ManagerPosition.Parent != null)
                {
                    return ManagerPosition.Parent.Department;
                }
                return null;
            }
        }

        /// <summary>
        /// 上级职位
        /// </summary>
        public virtual Position PositionParent
        {
            get
            {
                return ManagerPosition.Parent;
            }
        }

        /// <summary>
        /// 部门主管职位
        /// </summary>
        public virtual Position ManagerPosition { set; get; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public virtual string Name { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; private set; }

        /// <summary>
        /// 修改信息之前
        /// </summary>
        public virtual event TEventHandler<Department, ChangeEventArgs<DepartmentChangeInfo, DepartmentInfo, Department>> Changing;

        /// <summary>
        /// 修改信息之后
        /// </summary>
        public virtual event TEventHandler<Department, ChangeEventArgs<DepartmentChangeInfo, DepartmentInfo, Department>> Changed;

        public virtual void Change(User operationUser, DepartmentChangeInfo changeInfo)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (string.IsNullOrWhiteSpace(changeInfo.Name))
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(changeInfo.Name))
            {
                throw new ArgumentNullException("name");
            }
            if (changeInfo.Name != this.Name && this.Parent != null)
            {
                if (this.Parent.Children.Any(x => x.Name == changeInfo.Name))
                {
                    throw new DepartmentNameReapeatException();
                }
            }
            ChangeEventArgs<DepartmentChangeInfo, DepartmentInfo, Department> args = new ChangeEventArgs<DepartmentChangeInfo, DepartmentInfo, Department> 
            { 
                ChangeInfo = changeInfo,
                ChangeObject = this,
                ChangingSnapshotInfo = this.MapDepartmentInfo(),
                Operator = operationUser,
            };

            if (this.Changing != null)
            {
                this.Changing(this, args);
            }
            DepartmentModel model = NHibernateHelper.CurrentSession.Get<DepartmentModel>(this.ID);
            model.Name = changeInfo.Name;
            model.Remark = changeInfo.Remark;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Name = changeInfo.Name;
            this.Remark = changeInfo.Remark;

            if (this.Changed != null)
            {
                args.ChangedSnapshotInfo = this.MapDepartmentInfo();
                this.Changed(this, args);
            }
        }

        public ReadOnlyCollection<Department> Children
        {
            get
            {
                

                List<Department> children = this._orgMnger.DepartmentManager.Departments
                    .Where(x => x.Parent == this)
                    .ToList();
                return children.AsReadOnly();
            }
        }

        public ReadOnlyCollection<User> LogoffedUsers
        {
            get
            {
                return Positions.SelectMany(x => x.LogoffedUsers).Distinct().ToList().AsReadOnly();
            }
        }

        public ReadOnlyCollection<User> Users
        {
            get
            {
                return Positions.SelectMany(x => x.Users).Distinct().ToList().AsReadOnly();
            }
        }

        public ReadOnlyCollection<User> ChildrenUsers
        {
            get
            {
                return this.Children
                    .SelectMany(x => x.Users.Union(x.ChildrenUsers))
                    .Distinct()
                    .ToList()
                    .AsReadOnly();
            }
        }

        public ReadOnlyCollection<User> ChildrenLogedOffUsers
        {
            get
            {
                return this.Children
                    .SelectMany(x => x.ChildrenLogedOffUsers.Union(x.ChildrenLogedOffUsers))
                    .Distinct()
                    .ToList()
                    .AsReadOnly();
            }
        }

        public ReadOnlyCollection<User> AllUsers
        {
            get
            {
                return this.Users.Union(this.ChildrenUsers).ToList().AsReadOnly();
            }
        }

        public ReadOnlyCollection<User> AllLogoffedUsers
        {
            get
            {
                return this.LogoffedUsers.Union(this.ChildrenLogedOffUsers).ToList().AsReadOnly();
            }
        }

        public ReadOnlyCollection<Position> Positions
        {
            get
            {
                

                return this._orgMnger.PositionManager.Positions.
                    Where(x => x.Department == this)
                    .ToList()
                    .AsReadOnly();
            }
        }

        public bool InDepartment(User user, bool recursive)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (this.Users.Contains(user))
            {
                return true;
            }
            if(recursive)
            {
                foreach (Department child in this.Children)
                {
                    bool inChild = child.InDepartment(user, recursive);
                    if (inChild)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public DepartmentInfo MapDepartmentInfo()
        {
            return new DepartmentInfo 
            { 
                ID = this.ID,
                ParentId = this.Parent == null? null : this.Parent.ID,
                ManagerPositionId = this.ManagerPosition.ID,
                Name = this.Name,
                Remark = this.Remark
            };
        }

        public override MemberType Type
        {
            get { return MemberType.Department; }
        }

        public override List<Member> GetParents()
        {
            if(this.Parent == null)
            {
                return new List<Member>();
            }

            return new List<Member> { this.Parent };
        }

        public override List<Member> GetChildren()
        {
            List<Member> members = new List<Member>();
            members.AddRange(this.Children);
            return members;
        }

        public override List<User> GetUsers(bool recursive)
        {
            List<User> users = this.Users.ToList();
            if (recursive)
            {
                users.AddRange(this.Children.SelectMany(x => x.Users).ToList());
            }
            return users.Distinct().ToList();
        }

        public override bool Contains(User user)
        {
            return this.Users.Contains(user);
        }
    }
}
