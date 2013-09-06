using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api.Organization;

namespace Crm.Website.Models
{
    public class PositionTreeModel
    {
        public PositionTreeModel(PositionInfo position)
        {
            this.id = position.ID;
            this.text = position.Name;
            this.iconClass = "icon-position";
        }

        public string id;

        public string text;

        public string iconClass;
    }
}