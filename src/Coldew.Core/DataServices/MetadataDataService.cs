using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;

namespace Coldew.Core.DataServices
{
    public class MetadataDataService
    {
        protected ColdewObject _cobject;
        public MetadataDataService(ColdewObject cobject)
        {
            this._cobject = cobject;
        }

        public virtual Metadata Create(List<MetadataProperty> propertys)
        {
            MetadataModel model = new MetadataModel();
            model.PropertysJson = MetadataPropertyListHelper.ToPropertyModelJson(propertys);
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            Metadata metadata = this.Create(model.ID, model.PropertysJson);
            return metadata;
        }

        public virtual void Update(string id, List<MetadataProperty> propertys)
        {
            MetadataModel model = NHibernateHelper.CurrentSession.Get<MetadataModel>(id);
            model.PropertysJson = MetadataPropertyListHelper.ToPropertyModelJson(propertys);

            NHibernateHelper.CurrentSession.Update(model);
        }

        public virtual void Delete(string id)
        {
            MetadataModel model = NHibernateHelper.CurrentSession.Get<MetadataModel>(id);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        public virtual List<Metadata> LoadFromDB()
        {
            List<Metadata> metadatas = new List<Metadata>();

            IList<MetadataModel> models = NHibernateHelper.CurrentSession.QueryOver<MetadataModel>().List();
            foreach (MetadataModel model in models)
            {
                Metadata metadata = this.Create(model.ID, model.PropertysJson);

                metadatas.Add(metadata);
            }
            return metadatas;
        }

        public virtual Metadata Create(string id, string propertysJson)
        {
            List<MetadataProperty> propertys = MetadataPropertyListHelper.GetPropertys(propertysJson, this._cobject);
            Metadata metadata = new Metadata(id, propertys, this._cobject, this);
            return metadata;
        }
    }
}
