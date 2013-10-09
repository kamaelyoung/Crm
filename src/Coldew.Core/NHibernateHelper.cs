using System;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using System.IO;

namespace Coldew.Core
{
    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;
        static ISession _currentSession;

        static NHibernateHelper()
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Coldew.HB.config");
            sessionFactory = new NHibernate.Cfg.Configuration().Configure(configFilePath).BuildSessionFactory();
            
            _currentSession = sessionFactory.OpenSession();
        }

        public static ISession CurrentSession
        {
            get
            {
                return _currentSession;
            }
        }

        public static void CloseSession()
        {
            _currentSession.Close();
        }

        public static void CloseSessionFactory()
        {
            if (sessionFactory != null)
            {
                sessionFactory.Close();
            }
        }
    }
}
