using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api.Organization;

namespace Coldew.Website.Models
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