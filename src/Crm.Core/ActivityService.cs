using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Core.Organization;

namespace Crm.Core
{
    public class ActivityService : IActivityService
    {
        CrmManager _crmManager;
        public ActivityService(CrmManager crmManager)
        {
            this._crmManager = crmManager;
        }

        public List<ActivityInfo> GetActivitys(string account, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            return this._crmManager.ActivityManager
                .GetActivitys(user, skipCount, takeCount, out totalCount)
                .Select(x => x.Map())
                .ToList();
        }

        public ActivityInfo Create(string opUserAccount, string subject, string contactId, List<PropertyOperationInfo> propertys)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Contact contact = this._crmManager.ContactManager.GetContactById(contactId);

            Activity activity = this._crmManager.ActivityManager.Create(new ActivityCreateInfo { OpUser = opUser, Subject = subject, Contact = contact, PropertyInfos = propertys });
            return activity.Map();
        }

        public void Modify(string opUserAccount, string activityId, string subject, List<PropertyOperationInfo> propertys)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Activity activity = this._crmManager.ActivityManager.GetActivityById(activityId);
            activity.Modify(new ActivityModifyInfo { OpUser = opUser, Subject = subject, PropertyInfos = propertys });
        }

        public void Delete(string opUserAccount, string activityId)
        {
            User opUser = this._crmManager.OrgManager.UserManager.GetUserByAccount(opUserAccount);
            Activity activity = this._crmManager.ActivityManager.GetActivityById(activityId);
            activity.Delete(opUser);
        }

        public ActivityInfo GetActivityById(string id)
        {
            Activity activity = this._crmManager.ActivityManager.GetActivityById(id);
            if (activity != null)
            {
                return activity.Map();
            }
            return null;
        }

        public List<ActivityInfo> Search(string account, List<string> keywords, int skipCount, int takeCount, out int totalCount)
        {
            User user = this._crmManager.OrgManager.UserManager.GetUserByAccount(account);
            List<Activity> activitys = this._crmManager.ActivityManager.Search(user, keywords, skipCount, takeCount, out totalCount);
            return activitys.Select(x => x.Map()).ToList();
        }
    }
}
