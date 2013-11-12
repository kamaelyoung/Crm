using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data;
using Crm.Api;
using System.Text.RegularExpressions;
using Crm.Api.Exceptions;
using Newtonsoft.Json;
using Coldew.Core;
using Coldew.Core.Organization;
using Coldew.Core.DataServices;

namespace Crm.Core
{
    public class ActivityDataService : TModelMetadataDataService<ActivityModel>
    {
        public ActivityDataService(ColdewObject cobject)
            :base(cobject)
        {
            
        }

        public override Metadata Create(string id, string propertysJson)
        {
            return new Activity(id, MetadataPropertyListHelper.GetPropertys(propertysJson, this._cobject), this._cobject);
        }
    }
}
