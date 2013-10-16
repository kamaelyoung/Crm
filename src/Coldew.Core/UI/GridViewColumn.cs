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
        }

        public Field Field { private set; get; }

        public int Width { private set; get; }

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
