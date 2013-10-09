using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Coldew.Api;

namespace Coldew.Website.Models
{
    public class GridViewEditModel
    {
        public GridViewEditModel(GridViewInfo viewInfo)
        {
            this.name = viewInfo.Name;
            this.search = viewInfo.SearchExpression;
        }

        public string name;
        public string search;
    }
}