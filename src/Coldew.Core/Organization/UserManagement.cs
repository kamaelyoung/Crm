using System;
using System.Collections.Generic;
using System.Data;
using Coldew.Api.Organization.Exceptions;
using System.Collections.ObjectModel;
using System.Linq;


using Coldew.Data.Organization;
using System.IO;
using NHibernate.Criterion;
using Coldew.Api.Organization;


namespace Coldew.Core.Organization
{
    public class UserManagement 
    {

        static UserManagement()
        {
            
        }

        public UserManagement(OrganizationManagement orgMnger)
        {
            this._orgMdl = orgMnger;
            this._users = new List<User>();
            this._userByIdDictionary = new Dictionary<string, User>();
            this._userByAccountDictionary = new Dictionary<string, User>();
        }

        OrganizationManagement _orgMdl;

        private Dictionary<string, User> _userByIdDictionary;

        private Dictionary<string, User> _userByAccountDictionary;

        private bool _loaded;

        private List<User> _users;

        private List<User> _Users
        {
            get
            {
                this.Load();
                return this._users;
                
            }
        }

        internal event TEventHandler<UserManagement, List<User>> Loading;

        public event TEventHandler<UserManagement, List<User>> Loaded;

        /// <summary>
        /// 创建用户之前
        /// </summary>
        public event TEventHandler<UserManagement, CreateEventArgs<UserCreateInfo, UserInfo, User>> Creating;

        /// <summary>
        /// 创建用户之后
        /// </summary>
        public event TEventHandler<UserManagement, CreateEventArgs<UserCreateInfo, UserInfo, User>> Created;

        /// <summary>
        /// 删除用户之前
        /// </summary>
        public event TEventHandler<UserManagement, DeleteEventArgs<User>> Deleting;

        /// <summary>
        /// 删除用户之后
        /// </summary>
        public event TEventHandler<UserManagement, DeleteEventArgs<User>> Deleted;

        private object _updateLockObject = new object();

        public virtual User Create(User operationUser, UserCreateInfo createInfo)
        {
            lock (this._updateLockObject)
            {
                if (operationUser == null)
                {
                    throw new ArgumentNullException("operationUser");
                }

                if (string.IsNullOrEmpty(createInfo.Account))
                {
                    throw new AccountEmptyException();
                }
                if (string.IsNullOrEmpty(createInfo.Name))
                {
                    throw new UserNameEmptyException();
                }
                if (this._Users.Exists(x => x.Account == createInfo.Account))
                {
                    throw new AccountReapeatException();
                }
                Position position = this._orgMdl.PositionManager.GetPositionById(createInfo.MainPositionId);
                if (position == null && createInfo.Role == UserRole.User)
                {
                    throw new UserNeedMainPositionException();
                }
                if (createInfo.Password == null)
                {
                    throw new ArgumentNullException("userInfo.Password");
                }
                CreateEventArgs<UserCreateInfo, UserInfo, User> args = new CreateEventArgs<UserCreateInfo, UserInfo, User>
                {
                    Operator = operationUser,
                    CreateInfo = createInfo
                };
                if (this.Creating != null)
                {
                    this.Creating(this, args);
                }
                UserModel userModel = new UserModel
                {
                    Account = createInfo.Account,
                    Email = createInfo.Email,
                    Gender = (int)createInfo.Gender,
                    Name = createInfo.Name,
                    Password = Cryptography.MD5Encode(createInfo.Password),
                    Remark = createInfo.Remark,
                    Role = (int)createInfo.Role,
                    Status = (int)UserStatus.Normal
                };


                userModel.ID = NHibernateHelper.CurrentSession.Save(userModel).ToString();

                User user = new User(this._orgMdl, userModel);
                user.Changed += new TEventHandler<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_Changed);

                List<User> users = this._Users.ToList(); 
                users.Add(user);
                this._users = users;
                this.IndexUser();

                if (position != null)
                {
                    this._orgMdl.UserPositionManager.Create(operationUser,
                        new UserPositionInfo { Main = true, PositionId = position.ID, UserId = user.ID });
                }

                if (this.Created != null)
                {
                    args.CreatedObject = user;
                    args.CreatedSnapshotInfo = user.MapUserInfo();
                    this.Created(this, args);
                }
                return user;
            }
        }

        private void IndexUser()
        {
            this._userByIdDictionary = this._Users.ToDictionary(x => x.ID);
            this._userByAccountDictionary = this._Users.ToDictionary(x => x.Account.ToLower());
        }

        void User_Changed(User sender, ChangeEventArgs<UserChangeInfo, UserInfo, User> args)
        {
            
        }

        

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="userId"></param>
        public virtual void Delete(User operationUser, string userId)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            User user = this.GetUserById(userId);

            if (user != null)
            {
                if (user.Role != UserRole.User)
                {
                    throw new OrganizationException("无法删除系统用户 " + user.Name);
                }
                lock (_updateLockObject)
                {
                    DeleteEventArgs<User> args = new DeleteEventArgs<User>
                        {
                            Operator = operationUser,
                            DeleteObject = user
                        };
                    if (Deleting != null)
                    {
                        this.Deleting(this, args);
                    }
                    
                    UserModel model = NHibernateHelper.CurrentSession.Get<UserModel>(userId);
                    NHibernateHelper.CurrentSession.Delete(model);
                    NHibernateHelper.CurrentSession.Flush();

                    this._orgMdl.UserPositionManager.Delete(operationUser, userId);

                    List<User> users = this._Users.ToList();
                    users.Remove(user);
                    this._users = users;

                    this.IndexUser();

                    if (this.Deleted != null)
                    {
                        this.Deleted(this, args);
                    }
                }
            }
        }

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserById(string userId)
        {
            Load();
            try
            {
                return this._userByIdDictionary[userId];
            }
            catch (KeyNotFoundException)
            {

            }
            return null;
        }

        public User GetUserByAccount(string account)
        {
            Load();
            try
            {
                return this._userByAccountDictionary[account.ToLower()];
            }
            catch (KeyNotFoundException)
            {

            }
            return null;
        }

		/// <summary>
		/// 根据帐号和名称获取用户信息 lvxing
		/// 如果都为空所有人员返回所有用户成员
		/// 如果两都不为空返回满足两者条件的用户成员
		/// 如果帐号不为空姓名为空按帐号查询用户成员否则相反
		/// </summary>
		/// <param name="account">帐号</param>
		/// <param name="name">姓名</param>
		/// <returns>用户信息列表</returns>
        public List<User> Search(string account, string name)
        {
            if (account == null)
            {
                account = "";
            }
            if (name == null)
            {
                name = "";
            }
            return this._Users.Where(x => x.Status != UserStatus.Logoff
                && x.Role == UserRole.User
                && x.Account.IndexOf(account, StringComparison.InvariantCultureIgnoreCase) > -1
                && x.Name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) > -1)
                .ToList();
		}

        public List<User> Search(string accountOrName)
        {
            if (accountOrName == null)
            {
                accountOrName = "";
            }
            return this._Users.Where(x => x.Status != UserStatus.Logoff &&
                (x.Account.IndexOf(accountOrName, StringComparison.InvariantCultureIgnoreCase) > -1
                || x.Name.IndexOf(accountOrName, StringComparison.InvariantCultureIgnoreCase) > -1))
                .ToList();
        }

        public virtual bool CheckPassword(string account, string password)
        {
            string crypPassword = Cryptography.MD5Encode(password);
            return this._Users.Exists(x => x.Account.Equals(account, StringComparison.InvariantCultureIgnoreCase) && crypPassword == x.Password);
        }

        public ReadOnlyCollection<User> Users
        {
            get
            {
                return this._Users.AsReadOnly();
            }
        }

        protected virtual void Load()
        {
            if (!_loaded)
            {
                lock (this)
                {
                    if (!_loaded)
                    {
                        if (this.Loading != null)
                        {
                            this.Loading(this, this._users);
                        }
                        List<UserModel> models = NHibernateHelper.CurrentSession.QueryOver<UserModel>().List().ToList();
                        if (models != null)
                        {
                            models.ForEach(x =>
                            {
                                User user = new User(this._orgMdl, x);
                                user.Changed += new TEventHandler<User, ChangeEventArgs<UserChangeInfo, UserInfo, User>>(User_Changed);
                                this._users.Add(user);

                            });
                        }
                        _loaded = true;
                        this.IndexUser();

                        if (this.Loaded != null)
                        {
                            this.Loaded(this, this._users);
                        }
                    }
                }
            }
        }
    }
}