using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.UI;
using Coldew.Data;
using Newtonsoft.Json;

namespace Coldew.Core.UI
{
    public class Form
    {
        public Form(string id, string code, string title, List<Section> sections, List<RelatedObject> relateds)
        {
            this.ID = id;
            this.Code = code;
            this.Title = title;
            this.Sections = sections;
            this.Relateds = relateds;
        }

        public string ID { private set; get; }

        public string Code { private set; get; }

        public string Title {private set; get; }

        public List<Section> Sections { private set; get; }

        public List<RelatedObject> Relateds { private set; get; }

        public void ClearFieldData(Field field)
        {
            this.Sections.ForEach(x => x.ClearFieldData(field));
            this.Relateds.ForEach(x => x.ClearFieldData(field));

            FormModel model = NHibernateHelper.CurrentSession.Get<FormModel>(this.ID);
            model.SectionsJson = JsonConvert.SerializeObject(this.Sections.Select(x => x.MapModel()));
            model.RelatedsJson = JsonConvert.SerializeObject(this.Relateds.Select(x => x.MapModel()));

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        public FormInfo Map()
        {
            return new FormInfo
            {
                Title = this.Title,
                Code = this.Code,
                ID = this.ID,
                Relateds = this.Relateds == null ? null : this.Relateds.Select(x => x.Map()).ToList(),
                Sections = this.Sections == null ? null : this.Sections.Select(x => x.Map()).ToList(),
            };
        }
    }
}
