using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Data;
using Coldew.Core;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class GridViewColumn
    {
        public GridViewColumn(Field field, int width)
        {
            this.Field = field;
            this.Width = width;
            this.Field.Deleted += new TEventHandler<Field, User>(Field_Deleted);
        }

        void Field_Deleted(Field sender, User args)
        {
            this.Delete();
        }

        public Field Field { private set; get; }

        public int Width { private set; get; }

        public event TEventHandler<GridViewColumn> Deleted;

        public void Delete()
        {
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
                FieldId = this.Field.ID,
                Name = this.Field.Name,
                Width = this.Width
            };
        }
    }
}
