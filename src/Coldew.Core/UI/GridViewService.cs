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

        public GridViewInfo Create(string name, string objectId, string creatorAccount, bool isShared, string searchExpressionJson, List<GridViewColumnSetupInfo> columns)
        {
            User creator = this._coldewManager.OrgManager.UserManager.GetUserByAccount(creatorAccount);
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            int index = form.GridViewManager.MaxIndex();
            GridView view = form.GridViewManager.Create(GridViewType.Customized, "", name, creator, isShared, false, index, searchExpressionJson, columns);
            return view.Map();
        }

        private GridView GetViewById(string viewId)
        {
            List<ColdewObject> objects = this._coldewManager.ObjectManager.GetForms();
            GridView view = null;
            foreach (ColdewObject cobject in objects)
            {
                view = cobject.GridViewManager.GetGridView(viewId);
                if (view != null)
                {
                    return view;
                }
            }
            return null;
        }

        public void Modify(string viewId, string name, bool isShared, string searchExpressionJson, List<GridViewColumnSetupInfo> columns)
        {
            GridView view = this.GetViewById(viewId);
            view.Modify(name, isShared, searchExpressionJson, columns);
        }

        public void Modify(string viewId, List<GridViewColumnSetupInfo> columns)
        {
            GridView view = this.GetViewById(viewId);
            view.Modify(columns);
        }

        public void Delete(string viewId)
        {
            GridView view = this.GetViewById(viewId);
            view.Delete();
        }

        public GridViewInfo GetGridView(string viewId)
        {
            GridView view = this.GetViewById(viewId);
            if (view != null)
            {
                return view.Map();
            }
            return null;
        }

        public List<GridViewInfo> GetGridViews(string objectId, string userAccount)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(userAccount);
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            List<GridView> views = form.GridViewManager.GetGridViews(user);
            return views.Select(x => x.Map()).ToList();
        }

        public List<GridViewInfo> GetMyGridViews(string objectId, string userAccount)
        {
            User user = this._coldewManager.OrgManager.UserManager.GetUserByAccount(userAccount);
            ColdewObject form = this._coldewManager.ObjectManager.GetFormById(objectId);
            List<GridView> views = form.GridViewManager.GetGridViews(user);
            return views.Where(x => x.Creator == user && !x.IsSystem).Select(x => x.Map()).ToList();
        }
    }
}
