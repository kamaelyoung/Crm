using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    public interface IGridViewService
    {
        void SetViewColumns(GridViewType type, string userAccount, List<GridViewColumnSetupInfo> columns);

        GridViewInfo GetGridView(GridViewType type, string userAccount);
    }
}
