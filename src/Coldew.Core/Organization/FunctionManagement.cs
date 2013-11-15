using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data.Organization;
using NHibernate.Criterion;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public class FunctionManagement
    {
        OrganizationManagement _orgMnger;
        Dictionary<string, Function> _funtions;
        bool _loaded;

        public FunctionManagement(OrganizationManagement orgMnger)
        {
            _loaded = false;
            this._orgMnger = orgMnger;
            _funtions = new Dictionary<string,Function>();
        }

        internal void Load()
        {
            if (!_loaded)
            {
                lock (this)
                {
                    if (!_loaded)
                    {
                        IList<FunctionModel> funcModels = NHibernateHelper.CurrentSession.QueryOver<FunctionModel>().List();
                        if (funcModels != null)
                        {
                            foreach (FunctionModel model in funcModels)
                            {
                                List<Member> members = new List<Member>();
                                string[] memberIds = model.OwnerMemberIds.Split(',');
                                foreach(string memberId in memberIds)
                                {
                                    Member member = this._orgMnger.GetMember(memberId);
                                    if(member != null)
                                    {
                                        members.Add(member);
                                    }
                                }
                                Function function = new Function(model.ID, model.Name, model.Url, model.IconClass, model.Sort, members, this._orgMnger);
                                this._funtions.Add(function.ID, function);
                            }
                        }
                        _loaded = true;
                    }
                }
            }
        }

        public Function Create(string id, string name, string url, string iconClass, int sort, List<Member> members)
        {
            string memberIds = string.Join(",", members);
            
            FunctionModel model = new FunctionModel
                {
                    IconClass = iconClass,
                    ID = id,
                    Name = name,
                    Sort = sort,
                    Url = url,
                    OwnerMemberIds = memberIds
                };
            NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();

            Function function = new Function(id, name, url, iconClass, sort, members, this._orgMnger);
            this._funtions.Add(function.ID, function);

            return function;
        }

        public Function GetFunctionInfoById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            try
            {
                return this._funtions[id];
            }
            catch (KeyNotFoundException)
            {

            }
            return null;
        }

        public List<Function> GetAllFunction()
        {
            return this._funtions.Values.ToList();
        }
    }
}
