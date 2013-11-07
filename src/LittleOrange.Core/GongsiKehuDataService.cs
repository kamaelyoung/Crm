using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.DataServices;
using LittleOrange.Data;
using Coldew.Core;

namespace LittleOrange.Core
{
    public class GongsiKehuDataService : TModelMetadataDataService<GongsiKehuModel>
    {
        public GongsiKehuDataService(ColdewObject cobject)
            : base(cobject)
        {
            
        }

        public override Coldew.Core.Metadata Create(string id, string propertysJson)
        {
            List<MetadataProperty> propertys = MetadataPropertyListHelper.GetPropertys(propertysJson, this._cobject);
            GongsiKehu metadata = new GongsiKehu(id, propertys, this._cobject, this);
            return metadata;
        }
    }
}
