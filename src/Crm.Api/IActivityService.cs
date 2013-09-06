using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    public interface IActivityService
    {
        List<ActivityInfo> GetActivitys(string account, int skipCount, int takeCount, out int totalCount);

        ActivityInfo Create(string opUserAccount, string subject, string contactId, List<PropertyOperationInfo> propertys);

        void Modify(string opUserAccount, string activityId, string subject, List<PropertyOperationInfo> propertys);

        void Delete(string opUserAccount, string activityId);

        ActivityInfo GetActivityById(string id);

        List<ActivityInfo> Search(string account, List<string> keywords, int skipCount, int takeCount, out int totalCount);
    }
}
