using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api.Organization;

namespace Crm.Api
{
    [Serializable]
    public class GridViewInfo
    {
        public int ID {  set; get; }

        public bool IsSystem { set; get; }

        public UserInfo Creator {  set; get; }

        public GridViewType Type {  set; get; }

        public List<GridViewColumnInfo> Columns {  set; get; }
    }
}
