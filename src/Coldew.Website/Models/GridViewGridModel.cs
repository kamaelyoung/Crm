using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api;
using System.Web.Mvc;

namespace Coldew.Website.Models
{
    public class GridViewGridModel
    {
        public GridViewGridModel(GridViewInfo viewInfo, Controller controller, string formId)
        {
            this.id = viewInfo.ID;
            this.editLink = string.Format("<a href='{0}'>{1}</a>", controller.Url.Action("EditGridView", new { viewId = viewInfo.ID, formId = formId }), viewInfo.Name);
            this.name = viewInfo.Name;
            this.isShared = viewInfo.IsShared ? "是" : "否";
        }

        public string id ;

        public string editLink ;

        public string name ;

        public string isShared;
    }
}