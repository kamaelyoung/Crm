using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Core.Organization;

namespace Crm.Core
{
    public class GridViewService : IGridViewService
    {
        CrmManager _crmManager;
        public GridViewService(CrmManager crmManager)
        {
            this._crmManager = crmManager;
        }

        public void SetViewColumns(GridViewType type, string userAccount, List<GridViewColumnSetupInfo> columns)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(userAccount);
            this._crmManager.GridViewManager.SetGridView(user, type, columns);
        }

        public GridViewInfo GetGridView(GridViewType type, string userAccount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(userAccount);
            GridView view = this._crmManager.GridViewManager.GetGridView(user, type);
            if (view != null)
            {
                return view.Map();
            }
            return null;
        }
    }
}
