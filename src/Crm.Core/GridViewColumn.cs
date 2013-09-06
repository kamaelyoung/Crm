using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Data;
using Crm.Core.Extend;
using Crm.Core.Organization;

namespace Crm.Core
{
    public class GridViewColumn
    {
        public GridViewColumn(int id, Field field, int width)
        {
            this.ID = id;
            this.Field = field;
            this.Width = width;
            this.Field.Deleted += new TEventHanlder<Field, User>(Field_Deleted);
        }

        void Field_Deleted(Field sender, User args)
        {
            this.Delete();
        }

        public int ID { private set; get; }

        public Field Field { private set; get; }

        public int Width { private set; get; }

        public event TEventHanlder<GridViewColumn> Deleted;

        public void Delete()
        {
            GridViewColumnModel model = NHibernateHelper.CurrentSession.Get<GridViewColumnModel>(this.ID);
            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();

            if (this.Deleted != null)
            {
                this.Deleted(this);
            }
        }

        public GridViewColumnInfo Map()
        {
            return new GridViewColumnInfo
            {
                Code = this.Field.Code,
                ID = this.ID,
                FieldId = this.Field.ID,
                Name = this.Field.Name,
                Width = this.Width
            };
        }
    }
}
