using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data;
using Crm.Api;

namespace Crm.Core.Extend
{
    public class FormManager
    {
        List<Form> _forms;

        public FormManager()
        {
            this._forms = new List<Form>();
            this.Load();
        }

        public Form Create(FormType type, string name)
        {
            FormModel model = new FormModel
            {
                Type = (int)type,
                Name = name
            };
            model.ID = (int)NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();

            return this.Create(model);
        }

        private Form Create(FormModel model)
        {
            Form form = new Form(model.ID, (FormType)model.Type, model.Name);
            this._forms.Add(form);
            return form;
        }

        public Form GetForm(FormType type)
        {
            return this._forms.Find(x => x.Type == type);
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

        private void Load()
        {
            IList<FormModel> models = NHibernateHelper.CurrentSession.QueryOver<FormModel>().List();
            foreach (FormModel model in models)
            {
                Form form = this.Create(model);
                form.Load();
            }
        }
    }
}
