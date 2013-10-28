using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Coldew.Core;
using Coldew.Core.Organization;
using Coldew.Data;

namespace Douth.Core
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
            MetadataModel model = new MetadataModel();
            model.PropertysJson = MetadataPropertyListHelper.ToPropertyModelJson(propertys);
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            Contract metadata = new Contract(model.ID, propertys, this.ColdewObject);
            return metadata;
        }

        protected override List<Metadata> LoadFromDB()
        {
            List<Metadata> metadatas = new List<Metadata>();

            IList<MetadataModel> models = NHibernateHelper.CurrentSession.QueryOver<MetadataModel>().List();
            foreach (MetadataModel model in models)
            {
                Contract metadata = new Contract(model.ID, MetadataPropertyListHelper.GetPropertys(model.PropertysJson, this.ColdewObject), this.ColdewObject);

                metadatas.Add(metadata);
            }
            return metadatas;
        }
    }
}
