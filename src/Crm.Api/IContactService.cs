using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    public interface IContactService
    {
        List<ContactInfo> GetContacts(string account, int skipCount, int takeCount, out int totalCount);

        List<ContactInfo> GetContacts(string account);

        ContactInfo Create(string opUserAccount, string name, string customerId, List<PropertyOperationInfo> propertys);

        void Modify(string opUserAccount, string contactId, string name, List<PropertyOperationInfo> propertys);

        void Delete(string opUserAccount, string contactId);

        ContactInfo GetContactById(string id);

        List<ContactInfo> Search(string account, List<string> keywords, int skipCount, int takeCount, out int totalCount);

        List<ContactInfo> Search(string account, List<string> keywords);
    }
}
