using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core;
using Crm.Api;

namespace Crm.Core
{
    public class CustomerAreaField : Field
    {
        CustomerAreaManager _areaManager;
        public CustomerAreaField(FieldNewInfo info, CustomerArea defaultValue, CustomerAreaManager areaManager)
            :base(info)
        {
            this.DefaultValue = defaultValue;
            this._areaManager = areaManager;
        }

        public override string Type
        {
            get { return CustomerFieldType.CustomerArea; }
        }

        public override string TypeName
        {
            get { return "客户区域"; }
        }

        public CustomerArea DefaultValue { set; get; }

        public override MetadataValue CreateMetadataValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException();
            }
            CustomerArea area = null;
            int areaId;
            if (int.TryParse(value, out areaId))
            {
                area = this._areaManager.GetAreaById(areaId);
            }
            else
            {
                area = this._areaManager.GetAreaByName(value);
            }
            return new CustomerAreaMetadataValue(area, this);
        }

        public override FieldInfo Map()
        {
            CustomerAreaFieldInfo info =new CustomerAreaFieldInfo();
            this.Fill(info);
            return info;
        }
    }
}
