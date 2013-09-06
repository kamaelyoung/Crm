using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Crm.Api.Organization.Exceptions;

using Crm.Data.Organization;
using Crm.Api.Organization;



namespace Crm.Core.Organization
{
    public class Position 
    {
        public Position(PositionModel model, OrganizationManagement orgMnger)
        {
            if (string.IsNullOrWhiteSpace(model.ID))
            {
                throw new ArgumentNullException("positionInfo.ID");
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentNullException("positionInfo.Name");
            }

            this.ID = model.ID;
            this.Name = model.Name;
            this.ParentId = model.ParentId;
            this.Remark = model.Remark;

            this._orgMnger = orgMnger;
        }

        OrganizationManagement _orgMnger;

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; private set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public virtual string Name { get; private set; }

        /// <summary>
        /// 上级职位ID
        /// </summary>
        private string ParentId { get; set; }

        private Position _parent;
        /// <summary>
        /// 上级职位
        /// </summary>
        public virtual Position Parent
        {
            get
            {
                if (_parent == null)
                {
                    

                    _parent = this._orgMnger.PositionManager.Positions.FirstOrDefault(x => x.ID == ParentId);
                }
                return _parent;
            }
            private set
            {
                _parent = value;
            }
        }


        /// <summary>
        /// 职位类型
        /// </summary>
        public virtual OrganizationType PositionType { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; private set; }

        /// <summary>
        /// 修改信息之前
        /// </summary>
        public virtual event TEventHanlder<Position, ChangeEventArgs<PositionChangeInfo, PositionInfo, Position>> Changing;

        /// <summary>
        /// 修改信息之后
        /// </summary>
        public virtual event TEventHanlder<Position, ChangeEventArgs<PositionChangeInfo, PositionInfo, Position>> Changed;

        public virtual Department Department
        {
            get
            {
                

                Department department = this._orgMnger.DepartmentManager.Departments
                        .FirstOrDefault(x => x.ManagerPosition == this);
                if (department == null && Parent != null)
                {
                    return Parent.Department;
                }
                return department;
            }
        }

        public virtual ReadOnlyCollection<Department> ManagerialDepartments
        {
            get
            {
                return SelfManagerialDepartments
                    .Distinct()
                    .ToList()
                    .AsReadOnly();
            }
        }

        public virtual ReadOnlyCollection<Department> SelfManagerialDepartments
        {
            get
            {
                

                return this.SelfChildren
                    .Where(p => p.PositionType == OrganizationType.ManagerPosition)
                    .Select(p => p.Department)
                    .ToList()
                    .AsReadOnly();
            }
        }

        public virtual bool IsMySuperior(Position position, bool recursive)
        {
            if (this.Parent != null)
            {
                if (this.Parent == position)
                {
                    return true;
                }
                if (recursive)
                {
                    bool isMyParentSupperior = this.Parent.IsMySuperior(position, recursive);
                    if (isMyParentSupperior)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual ReadOnlyCollection<Position> NormalChildren
        {
            get
            {
                return SelfChildren
                    .Where(x => x.PositionType == OrganizationType.Position)
                    .ToList()
                    .AsReadOnly();
            }
        }

        public virtual ReadOnlyCollection<Position> Children
        {
            get
            {
                return SelfChildren.ToList().AsReadOnly();
            }
        }

        public virtual ReadOnlyCollection<Position> SelfChildren
        {
            get
            {
                

                return this._orgMnger.PositionManager.Positions.
                        Where(x => x.Parent == this)
                        .ToList()
                        .AsReadOnly();
            }
        }

        public virtual ReadOnlyCollection<Department> DepartmentChildren
        {
            get
            {
                return this._orgMnger
                    .DepartmentManager
                    .Departments
                    .Where(x => x.PositionParent == this)
                    .ToList()
                    .AsReadOnly();
            }
        }

        public ReadOnlyCollection<User> Users
        {
            get
            {

                return this._orgMnger.UserPositionManager
                    .GetUserPositionsByPositionId(this.ID)
                    .Where(x => x.User.Status != UserStatus.Logoff)
                    .Select(x => x.User)
                    .ToList()
                    .AsReadOnly();
            }
        }

        public ReadOnlyCollection<User> LogoffedUsers
        {
            get
            {

                return this._orgMnger.UserPositionManager
                    .GetUserPositionsByPositionId(this.ID)
                    .Where(x => x.User.Status == UserStatus.Logoff)
                    .Select(x => x.User)
                    .ToList()
                    .AsReadOnly();
            }
        }

        private object _updateLockObject = new object();

        public virtual bool InPosition(User user, bool recursive)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            } 
            
            bool contains = this.Users.Contains(user);
            if (!contains && recursive)
            {
                contains = this.Children.Any(x => x.InPosition(user, recursive));
            }
            return contains;
        }

        public virtual void Change(User operationUser, PositionChangeInfo changeInfo)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (string.IsNullOrWhiteSpace(changeInfo.Name))
            {
                throw new ArgumentNullException("name");
            }
            if (changeInfo.ParentId == this.ID)
            {
                throw new PositionParentCannotSelfException();
            }

            if (this.Parent != null && changeInfo.Name != this.Name)
            {
                Position position = this.Parent.SelfChildren.FirstOrDefault(x => x.Name == changeInfo.Name);
                if (position != null)
                {
                    throw new PositionNameReapeatException();
                }
            }

            ChangeEventArgs<PositionChangeInfo, PositionInfo, Position> args = new ChangeEventArgs<PositionChangeInfo, PositionInfo, Position>
            {
                ChangeInfo = changeInfo,
                ChangeObject = this,
                Operator = operationUser,
                ChangingSnapshotInfo = this.MapPositionInfo()
            };

            if (this.Changing != null)
            {
                this.Changing(this, args);
            }

            PositionModel model = NHibernateHelper.CurrentSession.Get<PositionModel>(this.ID);
            model.Name = changeInfo.Name;
            model.Remark = changeInfo.Remark;
            model.ParentId = changeInfo.ParentId;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Name = changeInfo.Name;
            this.Remark = changeInfo.Remark;
            this.ParentId = changeInfo.ParentId;
            this.Parent = null;

            if (this.Changed != null)
            {
                args.ChangedSnapshotInfo = this.MapPositionInfo();
                this.Changed(this, args);
            }
        }

        public virtual bool SelfChildrenHasUser()
        {
            return this.SelfChildren.Any(x => x.Users.Count > 0 || x.SelfChildrenHasUser());
        }

        public virtual bool SelfChildrenHasLogoffedUser()
        {
            return this.SelfChildren.Any(x => x.LogoffedUsers.Count > 0 || x.SelfChildrenHasLogoffedUser());
        }

        public PositionInfo MapPositionInfo()
        {
            return new PositionInfo 
            { 
                ID = this.ID,
                Name = this.Name,
                ParentId = this.ParentId,
                PositionType = this.PositionType,
                Remark = this.Remark,
                DepartmentId = Department == null ? null : Department.ID,
                HaveChildren=this.SelfChildren.Count>0?true:false
            };
        }
    }
}
