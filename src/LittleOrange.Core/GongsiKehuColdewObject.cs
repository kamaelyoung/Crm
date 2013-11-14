using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Core.UI;
using Coldew.Core.DataServices;
using LittleOrange.Data;

namespace LittleOrange.Core
{
    public class GongsiKehuColdewObject : ColdewObject
    {
        public GongsiKehuColdewObject(string id, string code, string name, ColdewManager coldewManager)
            :base(id, code, name, Coldew.Api.ColdewObjectType.Standard, true, coldewManager)
        {

        }

        protected override MetadataDataService CreateDataService()
        {
            return new TModelMetadataDataService<GongsiKehuModel>(this);
        }
    }
}
