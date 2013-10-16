using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Coldew.Data;
using Coldew.Api;
using Coldew.Core;
using log4net.Util;
using Newtonsoft.Json;
using Coldew.Core.Search;

namespace Coldew.Core
{
    public class GridView
    {
        ReaderWriterLock _lock;
        public GridView(string id, string code, string name, GridViewType type, User creator, bool isShared, bool isSystem, int index, List<GridViewColumn> columns, 
            MetadataSearcher Searcher, ColdewObject cobject)
        {
            this.ID = id;
            this.Code = code;
            this.Name = name;
            this.Type = type;
            this.Creator = creator;
            this.IsShared = isShared;
            this.IsSystem = isSystem;
            this.Index = index;
            this.Searcher = Searcher;
            this.ColdewObject = cobject;
            this.Columns = columns;
            if (this.Columns == null)
            {
                this.Columns = new List<GridViewColumn>();
            }
            this._lock = new ReaderWriterLock();
            this.ColdewObject.FieldDeleted += new TEventHandler<Core.ColdewObject, Field>(ColdewObject_FieldDeleted);
        }

        void ColdewObject_FieldDeleted(ColdewObject sender, Field field)
        {
            this.RemoveFieldColumn(field);
        }

        public string ID { private set; get; }

        public GridViewType Type { private set; get; }

        public string Code { private set; get; }

        public string Name { private set; get; }

        public User Creator { private set; get; }

        public bool IsShared { private set; get; }

        public bool IsSystem { private set; get; }

        public int Index { private set; get; }

        public MetadataSearcher Searcher { private set; get; }

        private List<GridViewColumn> Columns { set; get; }

        public ColdewObject ColdewObject { private set; get; }

        public void Modify(string name, bool isShared, string searchExpressionJson, List<GridViewColumnSetupInfo> columnsInfo)
        {
            this._lock.AcquireWriterLock();
            try
            {
                MetadataSearcher searcher = MetadataExpressionSearcher.Parse(searchExpressionJson, this.ColdewObject);
                List<GridViewColumn> columns = columnsInfo.Select(x => new GridViewColumn(this.ColdewObject.GetFieldById(x.FieldId), x.Width)).ToList();

                GridViewModel model = NHibernateHelper.CurrentSession.Get<GridViewModel>(this.ID);
                var columnModels = columns.Select(x => new GridViewColumnModel { FieldId = x.Field.ID, Width = x.Width });
                model.ColumnsJson = JsonConvert.SerializeObject(columnModels);
                model.SearchExpression = searchExpressionJson;
                model.Name = name;
                model.IsShared = isShared;
                NHibernateHelper.CurrentSession.Update(model);
                NHibernateHelper.CurrentSession.Flush();

                this.Columns = columns;
                this.Searcher = searcher;
                this.IsShared = isShared;
                this.Name = name;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public void Modify(List<GridViewColumnSetupInfo> columnsInfo)
        {
            this._lock.AcquireWriterLock();
            try
            {
                List<GridViewColumn> columns = columnsInfo.Select(x => new GridViewColumn(this.ColdewObject.GetFieldById(x.FieldId), x.Width)).ToList();

                this.UpdateColumnsDb(columns);

                this.Columns = columns;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public event TEventHandler<GridView> Deleting;
        public event TEventHandler<GridView> Deleted;

        public void Delete()
        {
            if (this.Deleting != null)
            {
                this.Deleting(this);
            }

            GridViewModel model = NHibernateHelper.CurrentSession.Get<GridViewModel>(this.ID);
            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();

            if (this.Deleted != null)
            {
                this.Deleted(this);
            }
        }

        private void UpdateColumnsDb(List<GridViewColumn> columns)
        {
            GridViewModel model = NHibernateHelper.CurrentSession.Get<GridViewModel>(this.ID);
            var columnModels = columns.Select(x => new GridViewColumnModel { FieldId = x.Field.ID, Width = x.Width });
            model.ColumnsJson = JsonConvert.SerializeObject(columnModels);
            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        private void RemoveFieldColumn(Field field)
        {
            this._lock.AcquireWriterLock();
            try
            {
                List<GridViewColumn> columns = this.Columns.ToList();
                GridViewColumn column = columns.Find(x => x.Field == field);
                if (column != null)
                {
                    columns.Remove(column);
                    this.UpdateColumnsDb(columns);
                    this.Columns = columns;
                }
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public bool HasPermission(User user)
        {
            if (this.IsShared)
            {
                return true;
            }
            if (this.Creator == user)
            {
                return true;
            }
            return false;
        }

        public GridViewInfo Map()
        {
            this._lock.AcquireReaderLock();
            try
            {
                return new GridViewInfo()
                {
                    Columns = this.Columns.Select(x => x.Map()).ToList(),
                    Creator = this.Creator.MapUserInfo(),
                    ID = this.ID,
                    IsSystem = this.IsSystem,
                    IsShared = this.IsShared,
                    Type  = this.Type,
                    Name = this.Name,
                    SearchExpression = this.Searcher == null ? "" : this.Searcher.ToString()
                };
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }
    }
}
