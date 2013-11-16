using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.UI
{
    public class GridViewCreateInfo
    {
        public GridViewCreateInfo(GridViewType type, string code, string name, bool isShared, bool isSystem,
            string searchExpressionJson, List<GridViewColumnSetupInfo> setupColumns, string orderBy, string createdUserAccount)
        {
            this.CreatedUserAccount = createdUserAccount;
            this.Code = code;
            this.Name = name;
            this.Type = type;
            this.IsShared = isShared;
            this.IsSystem = isSystem;
            this.SearchExpression = searchExpressionJson;
            this.SetupColumns = setupColumns;
            this.OrderBy = orderBy;
        }

        public virtual string CreatedUserAccount { set; get; }

        public virtual string Code { set; get; }

        public virtual string Name { set; get; }

        public virtual GridViewType Type { set; get; }

        public virtual bool IsShared { set; get; }

        public virtual bool IsSystem { set; get; }

        public virtual int Index { set; get; }

        public virtual List<GridViewColumnSetupInfo> SetupColumns { set; get; }

        public virtual string SearchExpression { set; get; }

        public virtual string OrderBy { set; get; }
    }
}
