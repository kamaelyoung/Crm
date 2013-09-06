using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Crm.Data;
using Crm.Api;
using Crm.Core.Extend;
using System.Text.RegularExpressions;
using log4net.Util;
using Crm.Api.Exceptions;

namespace Crm.Core
{
    public class ContactManager
    {
        private Dictionary<string, Contact> _contactDicById;
        private List<Contact> _contactList;
        OrganizationManagement _orgManger;
        CustomerManager _customerManager;
        Form _form;
        ReaderWriterLock _lock;

        public ContactManager(CustomerManager customerManager, OrganizationManagement orgManger, FormManager formManger)
        {
            this._contactDicById = new Dictionary<string, Contact>();
            this._contactList = new List<Contact>();
            this._orgManger = orgManger;
            this._customerManager = customerManager;
            this._form = formManger.GetForm(FormType.Contact);
            this._lock = new ReaderWriterLock();

            this.Load();
        }

        public Contact Create(ContactCreateInfo info)
        {
            this._lock.AcquireWriterLock();
            try
            {
                if (info.Customer == null)
                {
                    throw new ContactCustomerNullException();
                }

                Metadata metadata = Metadata.CreateMetadata(info.PropertyInfos, this._form);

                ContactModel model = new ContactModel();
                model.CreateTime = DateTime.Now;
                model.CreatorId = info.OpUser.ID;
                model.ModifiedTime = DateTime.Now;
                model.ModifiedUserId = info.OpUser.ID;
                model.Name = info.Name;
                model.MetadataId = metadata.ID;
                model.CustomerId = info.Customer.ID;

                model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
                NHibernateHelper.CurrentSession.Flush();
                Contact contact = new Contact(model.ID, model.Name, info.Customer, info.OpUser, model.CreateTime, info.OpUser, model.CreateTime, metadata);

                this._contactDicById.Add(contact.ID, contact);
                this._contactList.Insert(0, contact);

                this.BindEvent(contact);
                return contact;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private void BindEvent(Contact contact)
        {
            contact.Deleted += new TEventHanlder<Contact, User>(Contract_Deleted);
        }

        void Contract_Deleted(Contact contact, User opUser)
        {
            this._lock.AcquireWriterLock();
            try
            {
                this._contactDicById.Remove(contact.ID);
                this._contactList.Remove(contact);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public List<Contact> GetContacts(User user, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock();
            try
            {
                var contacts = this._contactList.Where(x => x.CanPreview(user)).ToList();
                totalCount = contacts.Count;
                return contacts.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Contact> GetContacts(User user)
        {
            this._lock.AcquireReaderLock();
            try
            {
                return this._contactList.Where(x => x.CanPreview(user)).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public Contact GetContactById(string id)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (this._contactDicById.ContainsKey(id))
                {
                    return this._contactDicById[id];
                }
                return null;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Contact> Search(User user, List<string> keywords, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (keywords == null || keywords.Count == 0)
                {
                    return this.GetContacts(user, skipCount, takeCount, out totalCount);
                }

                List<Regex> regexs = keywords.Select(x => new Regex(x.ToLower())).ToList();
                var searchContacts = this._contactList.Where(x => regexs.All(regex => regex.IsMatch(x.Content))).Where(x => x.CanPreview(user)).ToList();
                totalCount = searchContacts.Count;
                return searchContacts.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Contact> Search(User user, List<string> keywords)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (keywords == null || keywords.Count == 0)
                {
                    return this.GetContacts(user);
                }

                List<Regex> regexs = keywords.Select(x => new Regex(x.ToLower())).ToList();
                var searchContacts = this._contactList.Where(x => regexs.All(regex => regex.IsMatch(x.Content))).Where(x => x.CanPreview(user));
                return searchContacts.ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private void Load()
        {
            IList<ContactModel> models = NHibernateHelper.CurrentSession.QueryOver<ContactModel>().OrderBy(x => x.CreateTime).Desc.List();
            foreach (ContactModel model in models)
            {
                User creator = this._orgManger.UserManager.GetUserById(model.CreatorId);
                User modifiedUser = this._orgManger.UserManager.GetUserById(model.ModifiedUserId);
                Metadata metadata = Metadata.LoadMetadata(model.MetadataId, this._form);
                Customer customer = this._customerManager.GetCustomerById(model.CustomerId);
                Contact contact = new Contact(model.ID, model.Name, customer, creator, model.CreateTime, modifiedUser, model.CreateTime, metadata);

                this._contactDicById.Add(contact.ID, contact);
                this._contactList.Add(contact);

                this.BindEvent(contact);
            }
        }
    }
}
