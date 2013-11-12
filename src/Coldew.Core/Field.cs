using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Newtonsoft.Json;
using Coldew.Core.Organization;
using Coldew.Data;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public abstract class Field
    {
        public Field(FieldNewInfo info)
        {
            this.ID = info.ID;
            this.Tip = info.Tip;
            this.Name = info.Name;
            this.Required = info.Required;
            this.CanModify = info.CanModify;
            this._code = info.Code;
            this.CanInput = info.CanInput;
            this.Index = info.Index;
            this.ColdewObject = info.ColdewObject;
        }

        public int ID { set; get; }

        private string _code;
        public string Code
        {
            private set
            {
                this._code = value;
            }
            get 
            {
                if (string.IsNullOrEmpty(this._code))
                {
                    return "field" + this.ID;
                }
                return this._code;
            } 
        }

        public string Name { set; get; }

        public string Tip { set; get; }

        public bool Required { set; get; }

        public bool CanModify { set; get; }

        public bool CanInput { set; get; }

        public int Index { set; get; }

        public abstract string Type { get; }

        public abstract string TypeName { get; }

        public abstract MetadataValue CreateMetadataValue(JToken value);

        public ColdewObject ColdewObject { private set; get; }

        public event TEventHandler<Field, FieldModifyArgs> Modifying;
        public event TEventHandler<Field, FieldModifyArgs> Modified;

        protected void OnModifying(FieldModifyArgs modifyInfo)
        {
            if (this.Modifying != null)
            {
                this.Modifying(this, modifyInfo);
            }
        }

        protected void OnModifyed(FieldModifyArgs modifyInfo)
        {
            if (this.Modified != null)
            {
                this.Modified(this, modifyInfo);
            }
        }

        public event TEventHandler<Field, User> Deleting;
        public event TEventHandler<Field, User> Deleted;

        public void Delete(User opUser)
        {
            if (this.Deleting != null)
            {
                this.Deleting(this, opUser);
            }

            FieldModel model = NHibernateHelper.CurrentSession.Get<FieldModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();

            if (this.Deleted != null)
            {
                this.Deleted(this, opUser);
            }
        }

        public abstract FieldInfo Map();

        public void Fill(FieldInfo info)
        {
            info.Code = this.Code;
            info.ID = this.ID;
            info.Tip = this.Tip;
            info.CanModify = this.CanModify;
            info.Name = this.Name;
            info.Required = this.Required;
            info.Type = this.Type;
            info.TypeName = this.TypeName;
            info.CanInput = this.CanInput;
            info.Index = this.Index;
        }
    }
}
