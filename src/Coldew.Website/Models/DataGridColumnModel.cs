using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api;

namespace Coldew.Website.Models
{
    public class DataGridColumnModel
    {
        public DataGridColumnModel()
        {

        }

        public DataGridColumnModel(GridViewColumnInfo column)
        {
            this.title = column.Name;
            this.width = column.Width;
            this.field = column.Code;
        }

        public string title;
        public int width;
        public string field;
    }
}