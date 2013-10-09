using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Coldew.Api;

namespace Coldew.Core
{
    public class FormManager
    {
        protected ColdewManager _coldewManager;
        List<Form> _forms;

        public FormManager(ColdewManager coldewManager)
        {
            this._coldewManager = coldewManager;
            this._forms = new List<Form>();
        }

        public Form Create(string name, string code)
        {
            FormModel model = new FormModel
            {
                Code = code,
                Name = name
            };
            model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
            NHibernateHelper.CurrentSession.Flush();

            return this.Create(model);
        }

        private Form Create(FormModel model)
        {
            Form form = this.Create(model.ID, model.Code, model.Name);
            this._forms.Add(form);
            return form;
        }

        protected virtual Form Create(string id, string code, string name)
        {
            return new Form(id, code, name, this._coldewManager);
        }

        public Form GetFormById(string formId)
        {
            return this._forms.Find(x => x.ID == formId);
        }

        public Form GetFormByCode(string code)
        {
            return this._forms.Find(x => x.Code == code);
        }

        public List<Form> GetForms()
        {
            return this._forms.ToList();
        }

        public Field GetFieldById(int fieldId)
        {
            foreach (Form form in this._forms)
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
            IList<FormModel> models = NHibernateHelper.CurrentSession.QueryOver<FormModel>().List();
            foreach (FormModel model in models)
            {
                this.Create(model);
            }
            foreach (Form form in this._forms)
            {
                form.Load();
            }
        }
    }
}
