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
    public class ActivityManager
    {
        private Dictionary<string, Activity> _activityDicById;
        private List<Activity> _activityList;
        OrganizationManagement _orgManger;
        ContactManager _contactManager;
        Form _form;
        ReaderWriterLock _lock;

        public ActivityManager(ContactManager contactManager, OrganizationManagement orgManger, FormManager formManager)
        {
            this._activityDicById = new Dictionary<string, Activity>();
            this._activityList = new List<Activity>();
            this._orgManger = orgManger;
            this._contactManager = contactManager;
            this._form = formManager.GetForm(FormType.Activity);
            this._lock = new ReaderWriterLock();

            this.Load();
        }

        public Activity Create(ActivityCreateInfo info)
        {
            this._lock.AcquireWriterLock();
            try
            {
                if (info.Contact == null)
                {
                    throw new ActivityContactNullException();
                }

                Metadata metadata = Metadata.CreateMetadata(info.PropertyInfos, this._form);

                ActivityModel model = new ActivityModel();
                model.CreateTime = DateTime.Now;
                model.CreatorId = info.OpUser.ID;
                model.ModifiedTime = DateTime.Now;
                model.ModifiedUserId = info.OpUser.ID;
                model.Subject = info.Subject;
                model.MetadataId = metadata.ID;
                model.ContactId = info.Contact.ID;

                model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
                NHibernateHelper.CurrentSession.Flush();
                Activity activity = new Activity(model.ID, model.Subject, info.Contact, info.OpUser, model.CreateTime, info.OpUser, model.CreateTime, metadata);

                this._activityDicById.Add(activity.ID, activity);
                this._activityList.Insert(0, activity);

                this.BindEvent(activity);
                return activity;
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private void BindEvent(Activity activity)
        {
            activity.Deleted += new TEventHanlder<Activity>(Activity_Deleted);
        }

        void Activity_Deleted(Activity activity)
        {
            this._lock.AcquireWriterLock();
            try
            {
                this._activityDicById.Remove(activity.ID);
                this._activityList.Remove(activity);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public List<Activity> GetActivitys(User user, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock();
            try
            {
                var activitys = this._activityList.Where(x => x.CanPreview(user)).ToList();
                totalCount = activitys.Count;
                return activitys.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public Activity GetActivityById(string id)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (this._activityDicById.ContainsKey(id))
                {
                    return this._activityDicById[id];
                }
                return null;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Activity> Search(User user, List<string> keywords, int skipCount, int takeCount, out int totalCount)
        {
            this._lock.AcquireReaderLock();
            try
            {
                if (keywords == null || keywords.Count == 0)
                {
                    return this.GetActivitys(user, skipCount, takeCount, out totalCount);
                }

                List<Regex> regexs = keywords.Select(x => new Regex(x.ToLower())).ToList();
                var searchActivitys = this._activityList.Where(x => regexs.All(regex => regex.IsMatch(x.Content))).Where(x => x.CanPreview(user)).ToList();
                totalCount = searchActivitys.Count;
                return searchActivitys.Skip(skipCount).Take(takeCount).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private void Load()
        {
            IList<ActivityModel> models = NHibernateHelper.CurrentSession.QueryOver<ActivityModel>().OrderBy(x => x.CreateTime).Desc.List();
            foreach (ActivityModel model in models)
            {
                User creator = this._orgManger.UserManager.GetUserById(model.CreatorId);
                User modifiedUser = this._orgManger.UserManager.GetUserById(model.ModifiedUserId);
                Metadata metadata = Metadata.LoadMetadata(model.MetadataId, this._form);
                Contact contact = this._contactManager.GetContactById(model.ContactId);
                Activity activity = new Activity(model.ID, model.Subject, contact, creator, model.CreateTime, modifiedUser, model.CreateTime, metadata);

                this._activityDicById.Add(activity.ID, activity);
                this._activityList.Add(activity);

                this.BindEvent(activity);
            }
        }
    }
}
