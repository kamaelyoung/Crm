using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Coldew.Data;
using Newtonsoft.Json;

namespace Coldew.Core.UI
{
    public class FormManager
    {
        protected ColdewObjectManager _objectManager;
        protected ColdewObject _coldewObject;
        List<Form> _forms;
        protected ReaderWriterLock _lock;

        public FormManager(ColdewObjectManager objectManager, ColdewObject coldewObject)
        {
            this._objectManager = objectManager;
            this._coldewObject = coldewObject;
            this._forms = new List<Form>();
            this._lock = new ReaderWriterLock();
            this._coldewObject.FieldDeleted += new TEventHandler<ColdewObject, Field>(ColdewObject_FieldDeleted);
            this._coldewObject.FieldCreated += new TEventHandler<ColdewObject, Field>(ColdewObject_FieldCreated);
        }

        void ColdewObject_FieldCreated(ColdewObject sender, Field field)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                foreach (Form form in this._forms)
                {
                    form.Sections[0].Inputs.Add(new Input(field, field.Index));
                }
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        void ColdewObject_FieldDeleted(ColdewObject sender, Field field)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                foreach (Form form in this._forms)
                {
                    form.ClearFieldData(field);
                }
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public Form Create(string code, string title, List<Section> sections, List<RelatedObject> relateds)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                string sectionModelsJson = null;
                if (sections != null)
                {
                    sectionModelsJson = JsonConvert.SerializeObject(sections.Select(x => x.MapModel()).ToList());
                }

                string relatedModelsJson = null;
                if(relateds != null)
                {
                    var models = relateds.Select(x => x.MapModel()).ToList();
                    relatedModelsJson = JsonConvert.SerializeObject(models);
                }

                FormModel model = new FormModel
                {
                    Code = code,
                    Title = title,
                    RelatedsJson = relatedModelsJson,
                    SectionsJson = sectionModelsJson,
                    ObjectId = this._coldewObject.ID
                };
                model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
                NHibernateHelper.CurrentSession.Flush();

                return this.Create(model);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private Form Create(FormModel model)
        {
            List<SectionModel> sectionModels = JsonConvert.DeserializeObject<List<SectionModel>>(model.SectionsJson);
            List<RelatedObjectModel> relatedModels = new List<RelatedObjectModel>();
            if (!string.IsNullOrEmpty(model.RelatedsJson))
            {
                relatedModels = JsonConvert.DeserializeObject<List<RelatedObjectModel>>(model.RelatedsJson);
            }
            Form form = this.Create(model.ID, model.Code, model.Title, sectionModels, relatedModels);
            this._forms.Add(form);
            return form;
        }

        protected virtual Form Create(string id, string code, string title, List<SectionModel> sectionModels, List<RelatedObjectModel> relatedModels)
        {
            List<Section> sections = sectionModels.Select(x => {
                var inputs = x.Inputs.Select(input => new Input(this._coldewObject.GetFieldByCode(input.FieldCode), input.Index)).ToList();
                return new Section(x.Title, x.ColumnCount, inputs);
            }).ToList();

            List<RelatedObject> relateds = relatedModels.Select(x => new RelatedObject(x.ObjectCode, x.FieldCodes, this._objectManager)).ToList();

            return new Form(id, code, title, sections, relateds);
        }

        public Form GetFormById(string formId)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                return this._forms.Find(x => x.ID == formId);
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public Form GetFormByCode(string code)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                return this._forms.Find(x => x.Code == code);
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Form> GetForms()
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                return this._forms.ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        internal void Load()
        {
            IList<FormModel> models = NHibernateHelper.CurrentSession.QueryOver<FormModel>().Where(x => x.ObjectId == this._coldewObject.ID).List();
            foreach (FormModel model in models)
            {
                this.Create(model);
            }
        }
    }
}
