using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Coldew.Api.Organization.Exceptions;
using System.Linq;

using System.Text.RegularExpressions;
using Coldew.Data.Organization;

using NHibernate.Criterion;
using Coldew.Api.Organization;


namespace Coldew.Core.Organization
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        OrganizationManagement _orgMnger;

        public User(OrganizationManagement orgMnger, UserModel model)
        {
            if (orgMnger == null)
            {
                throw new ArgumentNullException("orgMnger");
            }
            if (string.IsNullOrEmpty(model.ID))
            {
                throw new ArgumentNullException("userInfo.ID");
            }
            if (string.IsNullOrEmpty(model.ID))
            {
                throw new ArgumentNullException("userInfo.ID");
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentNullException("userInfo.Name");
            }
            if (string.IsNullOrWhiteSpace(model.Account))
            {
                throw new ArgumentNullException("userInfo.Account");
            }
            this._orgMnger = orgMnger;
            this.ID = model.ID;
            this.Name = model.Name;
            this.Account = model.Account;
            this.Password = model.Password;
            this.Email = model.Email;
            this.Gender = (UserGender)model.Gender;
            this.Role = (UserRole)model.Role;
            this.Status = (UserStatus)model.Status;
            this.LastLoginTime = model.LastLoginTime;
            this.LastLoginIp = model.LastLoginIp;
            this.Remark = model.Remark;
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; private set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string Account { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; private set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; private set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public virtual string Name { get; private set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual UserGender Gender { get; private set; }

        public virtual UserRole Role { get; private set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public virtual UserStatus Status { get; private set; }

        private Position _mainPosition;
        public virtual Position MainPosition
        {
            set
            {
                _mainPosition = value;
            }
            get
            {
                if (_mainPosition == null)
                {
                    var userPositions = this._orgMnger.UserPositionManager.GetUserPositionsByUserId(this.ID);
                    UserPosition userPositoin = userPositions.Where(x => x.Main).FirstOrDefault();
                    if (userPositoin != null)
                    {
                        _mainPosition = userPositoin.Position;
                    }
                }
                return _mainPosition;
            }
        }

        public virtual ReadOnlyCollection<Position> Positions
        {
            get
            {
                

                return this._orgMnger.UserPositionManager
                    .GetUserPositionsByUserId(this.ID)
                    .Select(x => x.Position)
                    .ToList()
                    .AsReadOnly();
            }
        }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public virtual DateTime? LastLoginTime { get; private set; }

        /// <summary>
        /// 最后一次登录IP
        /// </summary>
        public virtual string LastLoginIp { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; private set; }

        /// <summary>
        /// 修改用户信息之后
        /// </summary>
        public virtual event TEventHandler<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>> Changing;
        public virtual event TEventHandler<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>> Changed;
        public virtual event TEventHandler<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>> Logoffed;
        public virtual event TEventHandler<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>> Locked;
        public virtual event TEventHandler<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>> Activated;
        public virtual event TEventHandler<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>> PasswordReseted;

        public virtual void Change(User operationUser, UserChangeInfo changeInfo)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            if (string.IsNullOrEmpty(changeInfo.Name))
            {
                throw new UserNameEmptyException();
            }
            
            ChangeEventArgs<UserChangeInfo, UserInfo, User> args = new ChangeEventArgs<UserChangeInfo, UserInfo, User>
                {
                    ChangeInfo = changeInfo,
                    ChangingSnapshotInfo = this.MapUserInfo(),
                    Operator = operationUser,
                    ChangeObject = this
                };
            if (this.Changing != null)
            {
                this.Changing(this, args);
            }
            
            UserModel model = NHibernateHelper.CurrentSession.Get<UserModel>(this.ID);
            model.Name = changeInfo.Name;
            model.Email = changeInfo.Email;
            model.Gender = (int)changeInfo.Gender;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Name = changeInfo.Name;
            this.Email = changeInfo.Email;

            if (this.Changed != null)
            {
                args.ChangedSnapshotInfo = this.MapUserInfo();
                this.Changed(operationUser, args);
            }
        }

        public virtual void ChangeSignInInfo(User operationUser, UserSignInChangeInfo changeInfo)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            UserModel model = NHibernateHelper.CurrentSession.Get<UserModel>(this.ID);
            model.LastLoginTime = changeInfo.LastLoginTime;
            model.LastLoginIp = changeInfo.LastLoginIp;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.LastLoginTime = changeInfo.LastLoginTime;
            this.LastLoginIp = changeInfo.LastLoginIp;
        }

        public virtual void ChangePassword(string oldPassword, string newPassword)
        {
            string encodedOldPassword = Cryptography.MD5Encode(oldPassword);
            if (this.Password != encodedOldPassword)
            {
                throw new OldPasswordWrongException();
            }
            string encodedNewPassword = Cryptography.MD5Encode(newPassword);
            this.ImportPassword(this, encodedNewPassword);
        }

        public virtual void ResetPassword(User opUser, string newPassword)
        {
            string encodedNewPassword = Cryptography.MD5Encode(newPassword);
            this.ImportPassword(this, encodedNewPassword);
        }

        public virtual void ImportPassword(User operationUser, string password)
        {
            ChangeEventArgs<UserChangeInfo, UserInfo, User> args = new ChangeEventArgs<UserChangeInfo, UserInfo, User>
            {
                ChangingSnapshotInfo = this.MapUserInfo(),
                Operator = operationUser,
                ChangeObject = this
            };
            UserModel model = NHibernateHelper.CurrentSession.Get<UserModel>(this.ID);
            model.Password = password;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Password = password;

            if (this.PasswordReseted != null)
            {
                args.ChangedSnapshotInfo = this.MapUserInfo();
                this.PasswordReseted(operationUser, args);
            }
        }

        public virtual void Lock(User operationUser)
        {
            UserStatus status = UserStatus.Lock;
            ChangeEventArgs<UserChangeInfo, UserInfo, User> args = new ChangeEventArgs<UserChangeInfo, UserInfo, User>
            {
                ChangingSnapshotInfo = this.MapUserInfo(),
                Operator = operationUser,
                ChangeObject = this
            };
            UserModel model = NHibernateHelper.CurrentSession.Get<UserModel>(this.ID);
            model.Status = (int)status;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Status = status;

            if (this.Locked != null)
            {
                args.ChangedSnapshotInfo = this.MapUserInfo();
                this.Locked(operationUser, args);
            }
        }

        public virtual void Unlock(User operationUser)
        {
            UserStatus status = UserStatus.Normal;
            ChangeEventArgs<UserChangeInfo, UserInfo, User> args = new ChangeEventArgs<UserChangeInfo, UserInfo, User>
            {
                ChangingSnapshotInfo = this.MapUserInfo(),
                Operator = operationUser,
                ChangeObject = this
            };

            UserModel model = NHibernateHelper.CurrentSession.Get<UserModel>(this.ID);
            model.Status = (int)status;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Status = status;

            if (this.Activated != null)
            {
                args.ChangedSnapshotInfo = this.MapUserInfo();
                this.Activated(operationUser, args);
            }
        }

        public virtual void Logoff(User operationUser)
        {
            UserStatus status = UserStatus.Logoff;
            ChangeEventArgs<UserChangeInfo, UserInfo, User> args = new ChangeEventArgs<UserChangeInfo, UserInfo, User>
            {
                ChangingSnapshotInfo = this.MapUserInfo(),
                Operator = operationUser,
                ChangeObject = this
            };

            UserModel model = NHibernateHelper.CurrentSession.Get<UserModel>(this.ID);
            model.Status = (int)status;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Status = status;

            if (this.Logoffed != null)
            {
                args.ChangedSnapshotInfo = this.MapUserInfo();
                this.Logoffed(operationUser, args);
            }
        }

        public virtual void Activate(User operationUser)
        {

            UserStatus status = UserStatus.Normal;
            ChangeEventArgs<UserChangeInfo, UserInfo, User> args = new ChangeEventArgs<UserChangeInfo, UserInfo, User>
            {
                ChangingSnapshotInfo = this.MapUserInfo(),
                Operator = operationUser,
                ChangeObject = this
            };

            UserModel model = NHibernateHelper.CurrentSession.Get<UserModel>(this.ID);

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Status = status;

            if (this.Activated != null)
            {
                args.ChangedSnapshotInfo = this.MapUserInfo();
                this.Activated(operationUser, args);
            }
        }

        public virtual Department MainDepartment
        {
            get
            {
                if (MainPosition == null)
                {
                    return null;
                }
                return this.MainPosition.Department;
            }
        }
        
        public virtual ReadOnlyCollection<Department> Departments
        {
            get
            {
                List<Department> departments = new List<Department>();
                foreach (Position p in Positions)
                {
                    if (p.Department != null)
                    {
                        departments.Add(p.Department);
                    }
                }
                return departments.AsReadOnly();
            }
        }

        public virtual ReadOnlyCollection<Position> LesserPosition
        {
            get
            {
                return Positions.Where(x => x != MainPosition).ToList().AsReadOnly();
            }
        }

        public virtual ReadOnlyCollection<Group> Groups
        {
            get
            {
                

                var userGroups = from g in this._orgMnger.GroupManager.Groups
                                    where g.GroupUsers.Contains(this)
                                    select g;

                return userGroups.ToList().AsReadOnly();
            }
        }

        public virtual ReadOnlyCollection<User> Superiors
        {
            get
            {
                return this.Positions.SelectMany(
                    p => 
                    {
                        if (p.Parent != null)
                        {
                            return p.Parent.Users;
                        }
                        return new List<User>().AsReadOnly();
                        
                    }).Distinct().ToList().AsReadOnly();
            }
        }

        public virtual bool IsMySuperior(User user, bool recursive)
        {
            if (user == this)
            {
                return false;
            }

            //我的上级职位中有了user的职位
            foreach (Position p in user.Positions)
            {
                if (this.Positions.Any(x => x.IsMySuperior(p, recursive)))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual ReadOnlyCollection<User> MyDepartmentManagers
        {
            get
            {
                var managers = (from manager in Superiors
                               where manager.Departments.FirstOrDefault(x => this.Departments.Contains(x)) != null
                               select manager).ToList();
                return managers.AsReadOnly();
            }
        }

        public virtual bool IsMyDepartmentManagers(User user)
        {
            if (Superiors == null)
            {
                return false;
            }

            //我的上级中找到这个用户
            bool contains = this.MyDepartmentManagers.Contains(user);
            if (contains)
            {
                return true;
            }
            return false;
        }

        public UserInfo MapUserInfo()
        {
            UserInfo userInfo = new UserInfo();
            userInfo.ID = this.ID;
            userInfo.Account = this.Account;
            userInfo.Email = this.Email;
            userInfo.Gender = this.Gender;
            userInfo.Role = this.Role;
            userInfo.LastLoginIp = this.LastLoginIp;
            userInfo.LastLoginTime = this.LastLoginTime;
            if (this.MainPosition != null)
            {
                userInfo.MainPositionId = this.MainPosition.ID;
                userInfo.MainPositionName = this.MainPosition.Name;
            }
            userInfo.Name = this.Name;
            userInfo.Password = this.Password;
            userInfo.Remark = this.Remark;
            userInfo.Status = this.Status;
            if(this.MainDepartment != null)
            {
                userInfo.MainDepartmentId = this.MainDepartment.ID;
                userInfo.MainDepartmentName = this.MainDepartment.Name;
            }
            return userInfo;
        }
    }
}
