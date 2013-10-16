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
        List<ColdewObject> _forms;

        public ColdewObjectManager(ColdewManager coldewManager)
        {
            this._coldewManager = coldewManager;
            this._forms = new List<ColdewObject>();
        }

        public ColdewObject Create(string name, string code)
        {
            ColdewObjectModel model = new ColdewObjectModel
            {
                Code = code,
                Name = name
            };
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            return this.Create(model);
        }

        private ColdewObject Create(ColdewObjectModel model)
        {
            ColdewObject form = this.Create(model.ID, model.Code, model.Name);
            this._forms.Add(form);
            return form;
        }

        protected virtual ColdewObject Create(string id, string code, string name)
        {
            return new ColdewObject(id, code, name, this._coldewManager);
        }

        public ColdewObject GetFormById(string objectId)
        {
            return this._forms.Find(x => x.ID == objectId);
        }

        public ColdewObject GetFormByCode(string code)
        {
            return this._forms.Find(x => x.Code == code);
        }

        public List<ColdewObject> GetForms()
        {
            return this._forms.ToList();
        }

        public Field GetFieldById(int fieldId)
        {
            foreach (ColdewObject form in this._forms)
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
            foreach (ColdewObject form in this._forms)
            {
                form.Load();
            }
        }
    }
}
