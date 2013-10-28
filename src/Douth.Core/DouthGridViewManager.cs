using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Core.Organization;
using Coldew.Data;
using Newtonsoft.Json;
using Coldew.Api;

namespace Douth.Core
{
    public class DouthGridViewManager : GridViewManager
    {
        public DouthGridViewManager(OrganizationManagement orgManager, ColdewObject coldewObject)
            : base(orgManager, coldewObject)
        {

        }

        protected override GridView Create(GridViewModel model)
        {
            if (model.Code == DouthObjectConstCode.GRID_VIEW_CODE_EXPIRED_CONTRACT)
            {
                return this.Create(model, new ContractExpiredSearcher());
            }
            else if (model.Code == DouthObjectConstCode.GRID_VIEW_CODE_EXPIRING_CONTRACT)
            {
                return this.Create(model, new ContractExpiringSearcher());
            }
            return base.Create(model);
        }

        private GridView Create(GridViewModel model, MetadataSearcher searcher)
        {
            User creator = this._orgManager.UserManager.GetUserByAccount(model.CreatorAccount);
            List<GridViewColumnModel> columnModels = JsonConvert.DeserializeObject<List<GridViewColumnModel>>(model.ColumnsJson);
            List<GridViewColumn> columns = columnModels.Select(x => new GridViewColumn(this._coldewObject.GetFieldById(x.FieldId), x.Width)).ToList();
            GridView view = new GridView(model.ID, model.Code, model.Name, (GridViewType)model.Type, creator, model.IsShared, model.IsSystem,
                    model.Index, columns, searcher, this._coldewObject);
            return view;
        }
    }
}
