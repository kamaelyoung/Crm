using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Coldew.Data;
using Coldew.Api;
using log4net.Util;
using Coldew.Core;
using Newtonsoft.Json;
using Coldew.Api.Exceptions;
using Coldew.Core.Search;

namespace Coldew.Core
{
    public class GridViewManager
    {
        protected Dictionary<string, GridView> _gridViewDicById;
        protected OrganizationManagement _orgManager;
        protected ColdewObject _coldewObject;
        protected ReaderWriterLock _lock;

        public GridViewManager(OrganizationManagement orgManager, ColdewObject coldewObject)
        {
            this._orgManager = orgManager;
            this._coldewObject = coldewObject;
            this._gridViewDicById = new Dictionary<string, GridView>();
            this._lock = new ReaderWriterLock();
        }

        public int MaxIndex()
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._gridViewDicById.Values.Max(x => x.Index);
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public GridView Create(GridViewType type, string code, string name, User user, bool isShared, bool isSystem, int index, 
            string searchExpressionJson, List<GridViewColumnSetupInfo> setupColumns)
        {
            var columnModels = setupColumns.Select(x => new GridViewColumnModel { FieldCode = x.FieldCode, Width = x.Width});
            string columnJson = JsonConvert.SerializeObject(columnModels);
            GridViewModel model = new GridViewModel
            {
                CreatorAccount = user.Account,
                IsSystem = isSystem,
                ObjectId = this._coldewObject.ID,
                Name = name,
                Type = (int)type,
                ColumnsJson = columnJson,
                IsShared = isShared,
                SearchExpression = searchExpressionJson,
                Code = code,
                Index = index
            };
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            GridView view = this.Create(model);
            this.BindEvent(view);
            this.Index(view);
            return view;
        }

        private void Index(GridView view)
        {
            this._gridViewDicById.Add(view.ID, view);
        }

        private void BindEvent(GridView view)
        {
            view.Deleted += new TEventHandler<GridView>(View_Deleted);
        }

        void View_Deleted(GridView args)
        {
            this._lock.AcquireReaderLock();
            try
            {
                this._gridViewDicById.Remove(args.ID);
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public GridView GetGridView(string viewId)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (this._gridViewDicById.ContainsKey(viewId))
                {
                    return this._gridViewDicById[viewId];
                }
                return null;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<GridView> GetGridViews(User user)
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._gridViewDicById.Values.Where(x => x.HasPermission(user)).OrderBy(x => x.Index).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<GridView> GetSystemGridViews()
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._gridViewDicById.Values.OrderBy(x => x.Index).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        protected virtual GridView Create(GridViewModel model)
        {
            User creator = this._orgManager.UserManager.GetUserByAccount(model.CreatorAccount);
            List<GridViewColumnModel> columnModels = JsonConvert.DeserializeObject<List<GridViewColumnModel>>(model.ColumnsJson);
            List<GridViewColumn> columns = columnModels.Select(x => new GridViewColumn(this._coldewObject.GetFieldByCode(x.FieldCode), x.Width)).ToList();
            GridView view = new GridView(model.ID, model.Code, model.Name, (GridViewType)model.Type, creator, model.IsShared, model.IsSystem,
                    model.Index, columns, MetadataExpressionSearcher.Parse(model.SearchExpression, this._coldewObject), this._coldewObject);
            return view;
        }

        internal void Load()
        {
            IList<GridViewModel> models = NHibernateHelper.CurrentSession.QueryOver<GridViewModel>().Where(x => x.ObjectId == this._coldewObject.ID).List();
            foreach (GridViewModel model in models)
            {
                GridView view = this.Create(model);
                this.BindEvent(view);
                this.Index(view);
            }
        }
    }
}
