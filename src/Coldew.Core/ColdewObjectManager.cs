using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Coldew.Api;

namespace Coldew.Core
{
    public class ColdewObjectManager
    {

        protected ColdewManager _coldewManager;
        List<ColdewObject> _objects;

        public ColdewObjectManager(ColdewManager coldewManager)
        {
            this._coldewManager = coldewManager;
            this._objects = new List<ColdewObject>();
        }

        public ColdewObject Create(ColdewObjectCreateInfo createInfo)
        {
            ColdewObjectModel model = new ColdewObjectModel
            {
                Code = createInfo.Code,
                Name = createInfo.Name,
                Type = (int)createInfo.Type,
                IsSystem = createInfo.IsSystem
            };
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            ColdewObject cobject = this.Create(model);
            cobject.CreateSystemFields(createInfo.NameFieldName);

            return cobject;
        }

        private ColdewObject Create(ColdewObjectModel model)
        {
            ColdewObject form = this.Create(model.ID, model.Code, (ColdewObjectType)model.Type, model.Name, model.IsSystem);
            this._objects.Add(form);
            return form;
        }

        protected virtual ColdewObject Create(string id, string code, ColdewObjectType type, string name, bool isSystem)
        {
            return new ColdewObject(id, code, name, type, isSystem, this._coldewManager);
        }

        public ColdewObject GetObjectById(string objectId)
        {
            return this._objects.Find(x => x.ID == objectId);
        }

        public ColdewObject GetObjectByCode(string code)
        {
            return this._objects.Find(x => x.Code == code);
        }

        public List<ColdewObject> GetObjects()
        {
            return this._objects.ToList();
        }

        public Field GetFieldById(int fieldId)
        {
            foreach (ColdewObject form in this._objects)
            {
                Field field = form.GetFieldById(fieldId);
                if (field != null)
                {
                    return field;
                }
            }
            return null;
        }

        internal void Load()
        {
            IList<ColdewObjectModel> models = NHibernateHelper.CurrentSession.QueryOver<ColdewObjectModel>().List();
            foreach (ColdewObjectModel model in models)
            {
                this.Create(model);
            }
            foreach (ColdewObject form in this._objects)
            {
                form.Load();
            }
        }
    }
}
