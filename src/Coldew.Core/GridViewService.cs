using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class GridViewService : IGridViewService
    {
        ColdewManager _coldewManager;
        public GridViewService(ColdewManager coldewManager)
        {
            this._coldewManager = coldewManager;
        }

        public GridViewInfo Create(string name, string formId, string creatorAccount, bool isShared, string searchExpressionJson, List<GridViewColumnSetupInfo> columns)
        {
            User creator = this._coldewManager.OrgManager.UserManager.GetUserByAccount(creatorAccount);
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            int index = this._coldewManager.GridViewManager.MaxIndex();
            GridView view = this._coldewManager.GridViewManager.Create(GridViewType.Customized, name, creator, isShared, false, index, searchExpressionJson, form, columns);
            return view.Map();
        }

        public void Modify(string viewId, string name, bool isShared, string searchExpressionJson, List<GridViewColumnSetupInfo> columns)
        {
            GridView view = this._coldewManager.GridViewManager.GetGridView(viewId);
            view.Modify(name, isShared, searchExpressionJson, columns);
        }

        public void Modify(string viewId, List<GridViewColumnSetupInfo> columns)
        {
            GridView view = this._coldewManager.GridViewManager.GetGridView(viewId);
            view.Modify(columns);
        }

        public void Delete(string viewId)
        {
            GridView view = this._coldewManager.GridViewManager.GetGridView(viewId);
            view.Delete();
        }

        public GridViewInfo GetGridView(string viewIdOrCode)
        {
            GridView view = this._coldewManager.GridViewManager.GetGridView(viewIdOrCode);
            if (view != null)
            {
                return view.Map();
            }
            return null;
        }

        public List<GridViewInfo> GetGridViews(string formId, string userAccount)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(userAccount);
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            List<GridView> views = this._coldewManager.GridViewManager.GetGridViews(user, form);
            return views.Select(x => x.Map()).ToList();
        }

        public List<GridViewInfo> GetMyGridViews(string formId, string userAccount)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(userAccount);
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            List<GridView> views = this._coldewManager.GridViewManager.GetGridViews(user, form);
            return views.Where(x => x.Creator == user && !x.IsSystem).Select(x => x.Map()).ToList();
        }

        public List<GridViewInfo> GetSystemGridViews(string formId)
        {
            Form form = this._coldewManager.FormManager.GetFormById(formId);
            List<GridView> views = this._coldewManager.GridViewManager.GetSystemGridViews(form);
            return views.Select(x => x.Map()).ToList();
        }
    }
}
