using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;

namespace Coldew.Core.DataServices
{
    public class TModelMetadataDataService<TModel> : MetadataDataService 
        where TModel :MetadataModel 
    {
        public TModelMetadataDataService(ColdewObject cobject)
            : base(cobject)
        {
            
        }

        public override Metadata Create(List<MetadataProperty> propertys)
        {
            TModel model = default(TModel);
            model.PropertysJson = MetadataPropertyListHelper.ToPropertyModelJson(propertys);
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            Metadata metadata = this.Create(model.ID, model.PropertysJson);
            return metadata;
        }

        public override void Update(string id, List<MetadataProperty> propertys)
        {
            TModel model = NHibernateHelper.CurrentSession.Get<TModel>(id);
            model.PropertysJson = MetadataPropertyListHelper.ToPropertyModelJson(propertys);

            NHibernateHelper.CurrentSession.Update(model);
        }

        public override void Delete(string id)
        {
            TModel model = NHibernateHelper.CurrentSession.Get<TModel>(id);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        public override List<Metadata> LoadFromDB()
        {
            List<Metadata> metadatas = new List<Metadata>();

            IList<TModel> models = NHibernateHelper.CurrentSession.QueryOver<TModel>().List();
            foreach (TModel model in models)
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
