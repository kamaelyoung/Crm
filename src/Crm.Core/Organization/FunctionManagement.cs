using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data.Organization;
using NHibernate.Criterion;
using Crm.Api.Organization;

namespace Crm.Core.Organization
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

        private void Load()
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
                                Function function = new Function(model, this._orgMnger);
                                this._funtions.Add(function.ID, function);
                            }
                        }
                        _loaded = true;
                    }
                }
            }
        }

        public Function Create(FunctionCreateInfo createInfo)
        {
            this.Load();
            FunctionModel model = new FunctionModel
                {
                    IconClass = createInfo.IconClass,
                    ID = createInfo.ID,
                    Name = createInfo.Name,
                    ParentId = createInfo.ParentId,
                    Sort = createInfo.Sort,
                    Url = createInfo.Url
                };
            NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();

            Function function = new Function(model, this._orgMnger);
            this._funtions.Add(function.ID, function);

            return function;
        }

        public Function GetFunctionInfoById(string id)
        {
            this.Load();
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

        public List<Function> GetTopFunctions()
        {
            this.Load();
            return this._funtions.Values
                .Where(x => x.Parent == null && this.HasChildren(x))
                .OrderBy(x => x.Sort)
                .ToList();
        }

        private bool HasChildren(Function function)
        {
            List<Function> functions = this._orgMnger.FunctionManager.GetChildren(function);
            if (functions != null && functions.Count > 0)
            {
                return true;
            }
            return false;
        }

        public List<Function> GetAllFunction()
        {
            this.Load();
            return this._funtions.Values.Where(x => x.Parent != null).ToList();
        }

        public List<Function> GetChildren(Function parent)
        {
            this.Load();
            return this._funtions.Values.Where(x => x.Parent == parent).ToList();
        }
    }
}
