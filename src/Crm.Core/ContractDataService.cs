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
    public class ContractDataService : TModelMetadataDataService<ContractModel>
    {
        public ContractDataService(ColdewObject form)
            : base(form)
        {

        }

        //public List<Contract> GetNeedEmailNotifyContracts()
        //{
        //    this._lock.AcquireReaderLock(0);
        //    try
        //    {
        //        var contacts = this._contractList.Where(x => (x.Expiring || x.Expired ) && !x.EmailNotified);
        //        return contacts.ToList();
        //    }
        //    finally
        //    {
        //        this._lock.ReleaseReaderLock();
        //    }
        //}

        public override Metadata Create(string id, string propertysJson)
        {
            return new Contract(id, MetadataPropertyListHelper.GetPropertys(propertysJson, this._cobject), this._cobject, this);
        }
    }
}
