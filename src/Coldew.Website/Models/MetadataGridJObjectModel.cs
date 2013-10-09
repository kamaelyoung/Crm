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
        public MetadataGridJObjectModel(string formId, MetadataInfo info, Dictionary<string, MetadataInfo> myFavorites, MetadataController controller)
        {
            this.Add("id", info.ID);
            foreach (PropertyInfo propertyInfo in info.Propertys)
            {
                if (propertyInfo.Code == FormConstCode.FIELD_NAME_NAME)
                {
                    string favoritedIcon = "";
                    if (myFavorites.ContainsKey(info.ID))
                    {
                        favoritedIcon = "<i class='icon-star'></i>";
                    }
                    else
                    {
                        favoritedIcon = "<i class='icon-star-empty'></i>";
                    }
                    string editUrl = controller.Url.Action("Edit", new { metadataId = info.ID, formId = formId });
                    string editLink = favoritedIcon + string.Format("<a href='{0}' target='_blank'>{1}</a>", editUrl, propertyInfo.ShowValue);
                    this.Add(propertyInfo.Code, editLink);
                }
                else
                {
                    this.Add(propertyInfo.Code, propertyInfo.ShowValue);
                }
            }
        }

        public MetadataGridJObjectModel(string formId, MetadataInfo info, MetadataController controller)
        {
            this.Add("id", info.ID);
            foreach (PropertyInfo propertyInfo in info.Propertys)
            {
                if (propertyInfo.Code == FormConstCode.FIELD_NAME_NAME)
                {
                    string editUrl = controller.Url.Action("Edit", new { metadataId = info.ID, formId = formId });
                    string editLink = string.Format("<a href='{0}' target='_blank'>{1}</a>", editUrl, propertyInfo.ShowValue);
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