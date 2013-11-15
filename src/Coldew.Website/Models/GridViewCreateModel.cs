using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Coldew.Website.Models
{
    public class GridViewCreateModel
    {
        public string name;
        public bool isShared;
        public string orderFieldCode;
        public string orderDirection;
        public string OrderBy { get { return this.orderFieldCode + " " + this.orderDirection; } }
        public List<GridViweColumnSetupModel> columns;
        public JObject search;
    }
}