using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Coldew.Api.Organization.Exceptions;
using Coldew.Data.Organization;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public class UserPositionManagement
    {
        private bool _loaded;

        OrganizationManagement _orgMnger;

        List<UserPosition> _userPositions;
        List<UserPosition> _UserPositions
        {
            get
            {
                return _userPositions;
            }
        }

        public UserPositionManagement(OrganizationManagement orgMnger)
        {
            this._orgMnger = orgMnger;
            this._userPositions = new List<UserPosition>();
        }

        public event TEventHandler<UserPositionManagement, List<UserPosition>> Loaded;

        /// <summary>
        /// 创建用户职位之前
        /// </summary>
        public event TEventHandler<UserPositionManagement, UserPositionOperationArgs> Creating;

        /// <summary>
        /// 创建用户职位之后
        /// </summary>
        public event TEventHandler<UserPositionManagement, UserPositionOperationArgs> Created;

        /// <summary>
        /// 删除用户职位之前
        /// </summary>
        public event TEventHandler<UserPositionManagement, UserPositionOperationArgs> Deleting;

        /// <summary>
        /// 删除用户职位之后
        /// </summary>
        public event TEventHandler<UserPositionManagement, UserPositionOperationArgs> Deleted;

        private object _updateLockObject = new object();

        /// <summary>
        /// 创建职位
        /// </summary>
        /// <param name="position">职位</param>
        /// <returns>职位</returns>
        public UserPosition Create(User operationUser, UserPositionInfo userPositionInfo)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }

            Position position = this._orgMnger.PositionManager.GetPositionById(userPositionInfo.PositionId);
            if (position == null)
            {
                throw new ArgumentException(string.Format("找不到id为{0}的职位", userPositionInfo.PositionId));
            }
            User user = this._orgMnger.UserManager.GetUserById(userPositionInfo.UserId);
            if (user == null)
            {
                throw new ArgumentException(string.Format("找不到id为{0}的用户", userPositionInfo.UserId));
            }

            if (this._UserPositions.Exists(x => x.User == user && x.Position == position))
            {
                throw new PositionUserExistsException(user.Name);
            }
            lock (_updateLockObject)
            {
                UserPositionOperationArgs args = new UserPositionOperationArgs(operationUser, userPositionInfo);
                if (this.Creating != null)
                {
                    this.Creating(this, args);
                }
                UserPositionModel model = new UserPositionModel
                    {
                        Main = userPositionInfo.Main,
                        PositionId = userPositionInfo.PositionId,
                        UserId = userPositionInfo.UserId
                    };
                model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();

                UserPosition userPosition = new UserPosition(this._orgMnger, model);

                List<UserPosition> userPositions = this._UserPositions.ToList();
                userPositions.Add(userPosition);
                this._userPositions = userPositions;

                if (this.Created != null)
                {
                    this.Created(this, args);
                }
                return userPosition;
            }
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="positionId">职位ID</param>
        public void Delete(User operationUser, string userId, string positionId)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }

            UserPosition userPosition = this.GetUserPosition(userId, positionId);
            if (userPosition != null)
            {
                lock (_updateLockObject)
                {
                    UserPositionOperationArgs args = new UserPositionOperationArgs(operationUser, userPosition.MapUserPositionInfo());
                    if (Deleting != null)
                    {
                        this.Deleting(this, args);
                    }
                    UserPositionModel model = NHibernateHelper.CurrentSession.QueryOver<UserPositionModel>().Where(x => x.UserId == userId && x.PositionId == positionId).SingleOrDefault();
                    NHibernateHelper.CurrentSession.Delete(model);
                    NHibernateHelper.CurrentSession.Flush();
                    List<UserPosition> userPositions = this._UserPositions.ToList();
                    userPositions.Remove(userPosition);
                    this._userPositions = userPositions;
                    if (this.Deleted != null)
                    {
                        this.Deleted(this, args);
                    }
                }
            }
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="positionId">职位ID</param>
        public void Delete(User operationUser, string userId)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }

            ReadOnlyCollection<UserPosition> userPositions = this.GetUserPositionsByUserId(userId);
            foreach (UserPosition userPosition in userPositions)
            {
                Delete(operationUser, userPosition.User.ID, userPosition.Position.ID);
            }
        }

        /// <summary>
        /// 获取职位信息
        /// </summary>
        /// <param name="positionId">职位ID</param>
        /// <returns>职位信息</returns>
        public UserPosition GetUserPosition(User operationUser, string userId, string positionId)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }

            return this.GetUserPosition(userId, positionId);
        }

        internal UserPosition GetUserPosition(string userId, string positionId)
        {
            
            return this._UserPositions.Find(x => x.User.ID == userId && x.Position.ID == positionId);
        }

        public void ChangeUserPosition(User operationUser, string userId, string positionId, string changePositionId)
        {
            if (operationUser == null)
            {
                throw new ArgumentNullException("operationUser");
            }
            User user = this._orgMnger.UserManager.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentException(string.Format("userId, 找不到Id为{0}的用户", userId));
            }
            UserPosition changeToUserPosition = this.GetUserPosition(userId, changePositionId);
            if (changeToUserPosition != null)
            {
                throw new PositionUserExistsException(user.Name);
            }
            UserPosition up = this.GetUserPosition(userId, positionId);
            UserPositionInfo info = up.MapUserPositionInfo();
            info.PositionId = changePositionId;
            this.Delete(operationUser, userId, positionId);
            this.Create(operationUser, info);
        }

        public ReadOnlyCollection<UserPosition> GetUserPositionsByUserId(string userId)
        {
            return this._UserPositions.Where(x => x.User.ID == userId).ToList().AsReadOnly();
        }

        public ReadOnlyCollection<UserPosition> GetUserPositionsByPositionId(string positionId)
        {
            return this._UserPositions.Where(x => x.Position.ID == positionId).ToList().AsReadOnly();
        }

        internal virtual void Load()
        {
            if (!this._loaded)
            {
                lock (this)
                {
                    if (!this._loaded)
                    {
                        List<UserPositionModel> models = NHibernateHelper.CurrentSession.QueryOver<UserPositionModel>().List().ToList();
                        if (models != null)
                        {
                            models.ForEach(x =>
                            {
                                UserPosition userPosition = new UserPosition(this._orgMnger, x);
                                this._userPositions.Add(userPosition);
                            });
                        }
                        this._loaded = true;
                        if (this.Loaded != null)
                        {
                            this.Loaded(this, this._userPositions);
                        }
                    }
                }
            }
        }
    }
}
