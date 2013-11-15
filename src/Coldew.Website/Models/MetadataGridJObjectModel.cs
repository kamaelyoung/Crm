using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Coldew.Api;
using System.Web.Mvc;
using Coldew.Website.Controllers;

namespace Coldew.Website.Models
{
    public class MetadataGridJObjectModel : JObject
    {
        public MetadataGridJObjectModel(string objectId, UserMetadataInfo info, MetadataController controller)
        {
            this.Add("id", info.ID);
            this.Add("canModify", info.PermissionValue.HasFlag(MetadataPermissionValue.Modify));
            this.Add("canDelete", info.PermissionValue.HasFlag(MetadataPermissionValue.Delete));
            foreach (PropertyInfo propertyInfo in info.Propertys)
            {
                if (propertyInfo.Code == ColdewObjectCode.FIELD_NAME_NAME)
                {
                    string favoritedIcon = "";
                    if (info.Favorited)
                    {
                        favoritedIcon = "<i class='icon-star'></i>";
                    }
                    else
                    {
                        favoritedIcon = "<i class='icon-star-empty'></i>";
                    }
                    string editUrl = controller.Url.Action("Details", new { metadataId = info.ID, objectId = objectId});
                    string editLink = favoritedIcon + string.Format("<a href='{0}' target='_blank'>{1}</a>", editUrl, propertyInfo.ShowValue);
                    this.Add(propertyInfo.Code, editLink);
                }
                else
                {
                    this.Add(propertyInfo.Code, propertyInfo.ShowValue);
                }
            }
        }

        public MetadataGridJObjectModel(MetadataInfo info)
        {
            this.Add("id", info.ID);
            foreach (PropertyInfo propertyInfo in info.Propertys)
            {
                this.Add(propertyInfo.Code, propertyInfo.ShowValue);
            }
        }
    }
}