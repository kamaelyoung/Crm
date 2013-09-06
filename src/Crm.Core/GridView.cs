using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Crm.Data;
using Crm.Api;
using Crm.Core.Extend;
using log4net.Util;

namespace Crm.Core
{
    public class GridView
    {
        ReaderWriterLock _lock;
        FormManager _formManager;
        public GridView(int id, User creator, bool isSystem, GridViewType type, FormManager formManager)
        {
            this.ID = id;
            this.Creator = creator;
            this.IsSystem = isSystem;
            this.Type = type;
            this._formManager = formManager;
            this.Columns = new List<GridViewColumn>();
            this._lock = new ReaderWriterLock();
        }

        public int ID { private set; get; }

        public User Creator { private set; get; }

        public bool IsSystem { private set; get; }

        public GridViewType Type { private set; get; }

        private List<GridViewColumn> Columns { set; get; }

        public void SetColumns(List<GridViewColumnSetupInfo> setupColumns)
        {
            this._lock.AcquireWriterLock();
            try
            {
                List<GridViewColumn> columns = this.Columns.ToList();
                foreach (GridViewColumn column in columns)
                {
                    column.Delete();
                }

                foreach (GridViewColumnSetupInfo columnInfo in setupColumns)
                {
                    Field field = this._formManager.GetFieldById(columnInfo.FieldId);
                    this.CreateColumn(field, columnInfo.Width);
                }
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public GridViewColumn CreateColumn(Field field, int width)
        {
            this._lock.AcquireWriterLock();
            try
            {
                GridViewColumnModel model = new GridViewColumnModel
                {
                    FieldId = field.ID,
                    Width = width,
                    ViewId = this.ID
                };
                model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();
                return this.CreateColumn(model);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private GridViewColumn CreateColumn(GridViewColumnModel model)
        {
            Field field = this._formManager.GetFieldById(model.FieldId);
            GridViewColumn column = new GridViewColumn(model.ID, field, model.Width);
            column.Deleted += new TEventHanlder<GridViewColumn>(Column_Deleted);
            this.Columns.Add(column);
            return column;
        }

        void Column_Deleted(GridViewColumn column)
        {
            this._lock.AcquireWriterLock();
            try
            {
                this.Columns.Remove(column);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        internal void Load()
        {
            IList<GridViewColumnModel> models = NHibernateHelper.CurrentSession.QueryOver<GridViewColumnModel>().Where(x => x.ViewId == this.ID).List();
            foreach (GridViewColumnModel model in models)
            {
                this.CreateColumn(model);
            }
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
                    Type = this.Type
                };
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }
    }
}
