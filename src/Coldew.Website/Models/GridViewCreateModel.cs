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
        public List<GridViweColumnSetupModel> columns;
        public JObject search;
    }
}