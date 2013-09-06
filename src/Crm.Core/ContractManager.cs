using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Crm.Core.Extend;
using System.Threading;
using Crm.Api;
using Crm.Api.Exceptions;
using Crm.Data;
using System.Text.RegularExpressions;

namespace Crm.Core
{
    public class ContractManager
    {
        private Dictionary<string, Contract> _contractDicById;
        private List<Contract> _contractList;
        OrganizationManagement _orgManger;
        CustomerManager _customerManager;
        Form _form;
        ReaderWriterLock _lock;

        public ContractManager(CustomerManager customerManager, OrganizationManagement orgManger, FormManager formManger)
        {
            this._contractDicById = new Dictionary<string, Contract>();
            this._contractList = new List<Contract>();
            this._orgManger = orgManger;
            this._customerManager = customerManager;
            this._form = formManger.GetForm(FormType.Contract);
            this._lock = new ReaderWriterLock();

            this.Load();
        }

        public Contract Create(ContractCreateInfo info)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                if (info.Customer == null)
                {
                    throw new CrmException("客户名称不能为空！");
                }

                Metadata metadata = Metadata.CreateMetadata(info.PropertyInfos, this._form);

                ContractModel model = new ContractModel();
                model.EndDate = info.EndDate;
                model.ExpiredComputeDays = info.ExpiredComputeDays;
                model.OwnerAccounts = string.Join(",", info.Owners.Select(x => x.Account));
                model.StartDate = info.StartDate;
                model.CreateTime = DateTime.Now;
                model.CreatorId = info.OpUser.ID;
                model.ModifiedTime = DateTime.Now;
                model.ModifiedUserId = info.OpUser.ID;
                model.Name = info.Name;
                model.MetadataId = metadata.ID;
                model.CustomerId = info.Customer.ID;
                model.Value = info.Value;

                model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
                NHibernateHelper.CurrentSession.Flush();
                Contract contact = new Contract(model.ID, model.Name, info.Customer, info.StartDate, info.EndDate, info.ExpiredComputeDays, 
                    info.Value, info.Owners, false, info.OpUser, model.CreateTime, info.OpUser, model.CreateTime, metadata);

                this._contractDicById.Add(contact.ID, contact);
                this._contractList.Insert(0, contact);

                this.BindEvent(contact);
                return contact;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private void BindEvent(Contract contact)
        {
            contact.Deleted += new TEventHanlder<Contract, User>(Contract_Deleted);
        }

        void Contract_Deleted(Contract contact, User opUser)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                this._contractDicById.Remove(contact.ID);
                this._contractList.Remove(contact);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public List<Contract> GetContracts(User user, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var contacts = this._contractList.Where(x => x.CanPreview(user)).ToList();
                totalCount = contacts.Count;
                return contacts.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Contract> GetExpiringContracts(User user, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var contacts = this._contractList.Where(x => x.CanPreview(user) && x.Expiring).ToList();
                totalCount = contacts.Count;
                return contacts.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Contract> GetNeedEmailNotifyContracts()
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var contacts = this._contractList.Where(x => (x.Expiring || x.Expired ) && !x.EmailNotified);
                return contacts.ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Contract> GetExpiredContracts(User user, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var contacts = this._contractList.Where(x => x.CanPreview(user) && x.Expired).ToList();
                totalCount = contacts.Count;
                return contacts.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Contract> GetContracts(User user)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                return this._contractList.Where(x => x.CanPreview(user)).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public Contract GetContractById(string id)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                if (this._contractDicById.ContainsKey(id))
                {
                    return this._contractDicById[id];
                }
                return null;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Contract> Search(User user, ContractSearcher seracher, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                var searchContacts = this._contractList.Where(x => x.CanPreview(user) && seracher.Accord(x)).ToList();
                totalCount = searchContacts.Count;
                return searchContacts.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Contract> Search(User user, ContractSearcher seracher)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                return this._contractList.Where(x => x.CanPreview(user) && seracher.Accord(x)).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private void Load()
        {
            IList<ContractModel> models = NHibernateHelper.CurrentSession.QueryOver<ContractModel>().OrderBy(x => x.CreateTime).Desc.List();
            foreach (ContractModel model in models)
            {
                User creator = this._orgManger.UserManager.GetUserById(model.CreatorId);
                User modifiedUser = this._orgManger.UserManager.GetUserById(model.ModifiedUserId);
                Metadata metadata = Metadata.LoadMetadata(model.MetadataId, this._form);
                Customer customer = this._customerManager.GetCustomerById(model.CustomerId);
                List<User> owners = model.OwnerAccounts.Split(',').Select(x => this._orgManger.UserManager.GetUserByAccount(x)).ToList();
                Contract contact = new Contract(model.ID, model.Name, customer, model.StartDate, model.EndDate, model.ExpiredComputeDays, 
                    model.Value, owners, model.EmailNotified, creator, model.CreateTime, modifiedUser, model.CreateTime, metadata);

                this._contractDicById.Add(contact.ID, contact);
                this._contractList.Add(contact);

                this.BindEvent(contact);
            }
        }
    }
}
