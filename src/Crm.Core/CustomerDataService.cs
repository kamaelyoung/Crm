using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.DataServices;
using Coldew.Core;
using Crm.Data;

namespace Crm.Core
{
    public class CustomerDataService : TModelMetadataDataService<CustomerModel>
    {
        public CustomerDataService(ColdewObject cobject)
            : base(cobject)
        {
            
        }

        public override Metadata Create(string id, string propertysJson)
        {
            List<MetadataProperty> propertys = MetadataPropertyListHelper.GetPropertys(propertysJson, this._cobject);
            Customer metadata = new Customer(id, propertys, this._cobject, this);
            return metadata;
        }
    }
}
