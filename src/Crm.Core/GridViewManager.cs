using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Crm.Data;
using Crm.Api;
using log4net.Util;
using Crm.Core.Extend;

namespace Crm.Core
{
    public class GridViewManager
    {
        private Dictionary<User, List<GridView>> _userViews;
        private Dictionary<GridViewType, GridView> _systemViews;
        OrganizationManagement _orgManager;
        FormManager _formManager;
        ReaderWriterLock _lock;

        public GridViewManager(OrganizationManagement orgManager, FormManager formManager)
        {
            this._orgManager = orgManager;
            this._formManager = formManager;
            this._userViews = new Dictionary<User,List<GridView>>();
            this._systemViews = new Dictionary<GridViewType, GridView>();
            this._lock = new ReaderWriterLock();
            this.Load();
        }

        public GridView CreateSystemGridView(User user, GridViewType type)
        {
            this._lock.AcquireWriterLock();
            try
            {
                GridView view = this.CreateGridView(user, type, true);
                this._systemViews.Add(type, view);
                return view;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private GridView CreateGridView(User user, GridViewType type, bool isSystem)
        {
            GridViewModel model = new GridViewModel
            {
                CreatorAccount = user.Account,
                IsSystem = isSystem,
                Type = (int)type
            };
            model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();

            return new GridView(model.ID, user, isSystem, type, this._formManager);
        }

        public GridView SetGridView(User user, GridViewType type, List<GridViewColumnSetupInfo> columns)
        {
            this._lock.AcquireWriterLock();
            try
            {
                GridView view = null;
                if (this._userViews.ContainsKey(user) && this._userViews[user].Any(x => x.Type == type))
                {
                    view = this._userViews[user].Find(x => x.Type == type);
                }
                if (view == null)
                {
                    view = this.CreateGridView(user, type, false);
                    if (!this._userViews.ContainsKey(user))
                    {
                        this._userViews.Add(user, new List<GridView>());
                    }
                    this._userViews[user].Add(view);
                }
                view.SetColumns(columns);
                return view;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public GridView GetGridView(User user, GridViewType type)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (!this._userViews.ContainsKey(user) || !this._userViews[user].Any(x => x.Type == type))
                {
                    return this._systemViews[type];
                }
                return this._userViews[user].Find(x => x.Type == type);
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private void Load()
        {
            IList<GridViewModel> models = NHibernateHelper.CurrentSession.QueryOver<GridViewModel>().List();
            foreach (GridViewModel model in models)
            {
                User creator = this._orgManager.UserManager.GetUserByAccount(model.CreatorAccount);
                GridView view = new GridView(model.ID, creator, model.IsSystem, (GridViewType)model.Type, this._formManager);
                if (model.IsSystem)
                {
                    this._systemViews.Add(view.Type, view);
                }
                else
                {
                    if (!this._userViews.ContainsKey(creator))
                    {
                        this._userViews.Add(creator, new List<GridView>());
                    }
                    this._userViews[creator].Add(view);
                }
                view.Load();
            }
        }
    }
}
