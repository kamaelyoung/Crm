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

namespace Coldew.Core
{
    public class GridViewManager
    {
        private Dictionary<string, GridView> _gridViewDicById;
        OrganizationManagement _orgManager;
        FormManager _formManager;
        ReaderWriterLock _lock;

        public GridViewManager(OrganizationManagement orgManager, FormManager formManager)
        {
            this._orgManager = orgManager;
            this._formManager = formManager;
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

        public GridView Create(GridViewType type, string name, User user, bool isShared, bool isSystem, int index, string searchExpressionJson, Form form, List<GridViewColumnSetupInfo> setupColumns)
        {
            var columnModels = setupColumns.Select(x => new GridViewColumnModel { FieldId = x.FieldId, Width = x.Width });
            string columnJson = JsonConvert.SerializeObject(columnModels);
            GridViewModel model = new GridViewModel
            {
                CreatorAccount = user.Account,
                IsSystem = isSystem,
                FormId = form.ID,
                Name = name,
                Type = (int)type,
                ColumnsJson = columnJson,
                IsShared = isShared,
                SearchExpression = searchExpressionJson
            };
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();
            List<GridViewColumn> columns = setupColumns.Select(x => new GridViewColumn(form.GetFieldById(x.FieldId), x.Width)).ToList();
            MetadataSearcher searcher = MetadataSearcher.Parse(searchExpressionJson, form);
            GridView view = new GridView(model.ID, name, type, user, isShared, isSystem, index, columns, searcher,form);
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

        public List<GridView> GetGridViews(User user, Form form)
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._gridViewDicById.Values.Where(x => x.Form == form && x.HasPermission(user)).OrderBy(x => x.Index).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<GridView> GetSystemGridViews(Form form)
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._gridViewDicById.Values.Where(x => x.Form == form).OrderBy(x => x.Index).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        internal void Load()
        {
            IList<GridViewModel> models = NHibernateHelper.CurrentSession.QueryOver<GridViewModel>().List();
            foreach (GridViewModel model in models)
            {
                User creator = this._orgManager.UserManager.GetUserByAccount(model.CreatorAccount);
                Form form = this._formManager.GetFormById(model.FormId);
                List<GridViewColumnModel> columnModels = JsonConvert.DeserializeObject<List<GridViewColumnModel>>(model.ColumnsJson);
                List<GridViewColumn> columns = columnModels.Select(x => new GridViewColumn(form.GetFieldById(x.FieldId), x.Width)).ToList();
                GridView view = new GridView(model.ID, model.Name, (GridViewType)model.Type, creator, model.IsShared, model.IsSystem,
                    model.Index, columns, MetadataSearcher.Parse(model.SearchExpression, form), form);
                this.BindEvent(view);
                this.Index(view);
            }
        }
    }
}
