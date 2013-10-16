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

namespace Crm.Core
{
    public class ContractManager : MetadataManager
    {
        public ContractManager(ColdewObject form, OrganizationManagement orgManger)
            : base(form, orgManger)
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

        protected override Metadata CreateAndSaveDB(List<MetadataProperty> propertys)
        {
            ContractModel model = new ContractModel();
            model.PropertysJson = MetadataPropertyListHelper.ToPropertyModelJson(propertys);
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            Contract metadata = new Contract(model.ID, propertys, this.ColdewObject);
            return metadata;
        }

        protected override List<Metadata> LoadFromDB()
        {
            List<Metadata> metadatas = new List<Metadata>();

            IList<ContractModel> models = NHibernateHelper.CurrentSession.QueryOver<ContractModel>().List();
            foreach (ContractModel model in models)
            {
                Contract metadata = new Contract(model.ID, MetadataPropertyListHelper.GetPropertys(model.PropertysJson, this.ColdewObject), this.ColdewObject);

                metadatas.Add(metadata);
            }
            return metadatas;
        }
    }
}
