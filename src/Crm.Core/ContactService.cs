using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Core.Organization;

namespace Crm.Core
{
    public class ContactService : IContactService
    {
        CrmManager _crmManager;
        public ContactService(CrmManager crmManager)
        {
            this._crmManager = crmManager;
        }

        public List<ContactInfo> GetContacts(string account, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            return this._crmManager.ContactManager
                .GetContacts(user, skipCount, takeCount, out totalCount)
                .Select(x => x.Map())
                .ToList();
        }

        public List<ContactInfo> GetContacts(string account)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            return this._crmManager.ContactManager
                .GetContacts(user)
                .Select(x => x.Map())
                .ToList();
        }

        public ContactInfo Create(string opUserAccount, string name, string customerId, List<PropertyOperationInfo> propertys)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Customer customer = this._crmManager.CustomerManager.GetById(customerId) as Customer;

            Contact contact = this._crmManager.ContactManager.Create(new ContactCreateInfo { OpUser = opUser, Name = name, Customer = customer, PropertyInfos = propertys });
            return contact.Map();
        }

        public void Modify(string opUserAccount, string contactId, string name, List<PropertyOperationInfo> propertys)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Contact contact = this._crmManager.ContactManager.GetContactById(contactId);
            contact.Modify(new ContactModifyInfo { OpUser = opUser, Name = name, PropertyInfos = propertys });
        }

        public void Delete(string opUserAccount, string contactId)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Contact contact = this._crmManager.ContactManager.GetContactById(contactId);
            contact.Delete(opUser);
        }

        public ContactInfo GetContactById(string id)
        {
            Contact contact = this._crmManager.ContactManager.GetContactById(id);
            if (contact != null)
            {
                return contact.Map();
            }
            return null;
        }

        public List<ContactInfo> Search(string account, List<string> keywords, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            List<Contact> contacts = this._crmManager.ContactManager.Search(user, keywords, skipCount, takeCount, out totalCount);
            return contacts.Select(x => x.Map()).ToList();
        }

        public List<ContactInfo> Search(string account, List<string> keywords)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            List<Contact> contacts = this._crmManager.ContactManager.Search(user, keywords);
            return contacts.Select(x => x.Map()).ToList();
        }
    }
}
