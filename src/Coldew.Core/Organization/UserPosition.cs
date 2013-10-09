using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data.Organization;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public class UserPosition
    {
        public UserPosition(OrganizationManagement orgMnger, UserPositionModel model)
        {
            this._orgMnger = orgMnger;
            this._userId = model.UserId;
            this._positionId = model.PositionId;
            this.Main = model.Main;
            this._id = model.ID;
            if (this.Main)
            {
                this.User.MainPosition = this.Position;
            }
        }
        OrganizationManagement _orgMnger;

        /// <summary>
        /// 修改信息之前
        /// </summary>
        public virtual event TEventHandler<UserPosition, UserPositionOperationArgs> Modifying;

        /// <summary>
        /// 修改信息之后
        /// </summary>
        public virtual event TEventHandler<UserPosition, UserPositionOperationArgs> Modifyed;

        private int _id;

        private string _userId;

        public User User
        {
            get
            {
                return this._orgMnger.UserManager.GetUserById(this._userId);
            }
        }

        private string _positionId;

        public Position Position
        {
            get
            {
                return this._orgMnger.PositionManager.GetPositionById(this._positionId);
            }
        }

        public bool Main { set; get; }

        public virtual void Moidfy(User operationUser, bool main)
        {
            UserPositionInfo userPositionInfo = this.MapUserPositionInfo();
            userPositionInfo.Main = main;
            UserPositionOperationArgs args = new UserPositionOperationArgs(operationUser, userPositionInfo);

            if (this.Modifying != null)
            {
                this.Modifying(this, args);
            }
            UserPositionModel model = NHibernateHelper.CurrentSession.Get<UserPositionModel>(this._id);
            model.Main = main;
            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();
            this.Main = main;
            if (main)
            {
                this.User.MainPosition = this.Position;
            }

            if (this.Modifyed != null)
            {
                this.Modifyed(this, args);
            }
        }

        public UserPositionInfo MapUserPositionInfo()
        {
            return new UserPositionInfo 
            { 
                Main = this.Main,
                PositionId = this._positionId,
                UserId = this._userId,
                UserName = this.User.Name,
                PositionName = this.Position.Name
            };
        }
    }
}
