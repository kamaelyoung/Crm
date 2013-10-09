using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Organization;

namespace Coldew.Api
{
    [Serializable]
    public class GridViewInfo
    {
        public string ID { set; get; }

        public GridViewType Type { set; get; }

        public string Name { set; get; }

        public bool IsSystem { set; get; }

        public bool IsShared{ set; get; }

        public UserInfo Creator {  set; get; }

        public List<GridViewColumnInfo> Columns {  set; get; }

        public string SearchExpression { set; get; }
    }
}
