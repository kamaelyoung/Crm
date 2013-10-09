using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    public interface IGridViewService
    {
        GridViewInfo Create(string name, string formId, string creatorAccount, bool isShared, string searchExpressionJson, List<GridViewColumnSetupInfo> columns);

        void Modify(string viewId, List<GridViewColumnSetupInfo> columns);

        void Modify(string viewId, string name, bool isShared, string searchExpressionJson, List<GridViewColumnSetupInfo> columns);

        void Delete(string viewId);

        GridViewInfo GetGridView(string viewId);

        List<GridViewInfo> GetGridViews(string formId, string userAccount);

        List<GridViewInfo> GetMyGridViews(string formId, string userAccount);

        List<GridViewInfo> GetSystemGridViews(string formId);
    }
}
