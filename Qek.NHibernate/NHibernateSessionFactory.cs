using NHibernate;
using Qek.Common;
using System;
using System.Web;

namespace Qek.NHibernate
{
    /// <summary>
    /// DateTime:   2013/07/31
    /// Author:     Sam.SH_Chang#21978
    /// Main function is to offer a session to Application client.
    /// </summary>
    public class NHibernateSessionFactory
    {
        private const string SESSIONKEY = "NHIBERNATE.SESSION";

        private static ISessionFactory _sessionFactory = null;

        /// <summary>
        /// http://theburningmonk.com/2010/10/threadstatic-vs-threadlocal/
        /// 
        /// Occa­sion­ally you might want to make the value of a sta­tic or instance field local to a thread 
        /// (i.e. each thread holds an inde­pen­dent copy of the field), 
        /// what you need in this case, is a thread-local stor­age
        /// 
        /// The lim­i­ta­tions :
        /// 1.The Thread­Sta­tic attribute doesn’t work with instance fields, it com­piles and runs but does nothing
        /// 2.Field always start with the default value
        /// </summary>
        [ThreadStatic]
        private static ISession _session; //this session is not used in web

        private static ProjectType _projectType = ConfigHelper.GetAppSetting<ProjectType>("ProjectType");

        private static bool IsWeb
        { get { return _projectType == ProjectType.WebApplication || _projectType == ProjectType.WebSite; } }

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var config = new NHibernateCfgBuilder().Build();
                    _sessionFactory = config.BuildSessionFactory();
                    if (_sessionFactory == null)
                    {
                        throw new InvalidOperationException("Could not build SessionFactory");
                    }
                }
                return _sessionFactory;
            }
        }

        #region NHibernate Sessions
        /// <summary>
        /// Opens the stateless session.
        /// </summary>
        /// <returns></returns>
        public static IStatelessSession OpenStatelessSession()
        {
            return SessionFactory.OpenStatelessSession();
        }

        /// <summary>
        /// Explicitly open a session. If you have an open session, close it first.
        /// </summary>
        /// <returns>The <see cref="NHibernate.ISession"/></returns>
        public static ISession OpenSession()
        {
            ISession session = SessionFactory.OpenSession();
            if (IsWeb)
            {
                if (HttpContext.Current.Items.Contains(SESSIONKEY))
                {
                    HttpContext.Current.Items.Remove(SESSIONKEY);
                }
                HttpContext.Current.Items.Add(SESSIONKEY, session);
            }
            else
            {
                _session = session;
            }
            return session;
        }

        /// <summary>
        /// Gets the current <see cref="NHibernate.ISession"/>. Although this is a singleton, this is specific to the thread/ asp session. If you want to handle multiple sessions, use <see cref="OpenSession"/> directly. If a session it not open, a new open session is created and returned.
        /// </summary>
        /// <value>The <see cref="NHibernate.ISession"/></value>
        public static ISession Session
        {
            get
            {
                //use threadStatic or asp.net web session.
                ISession session = IsWeb ? HttpContext.Current.Items[SESSIONKEY] as ISession : _session;
                //if using CurrentSessionContext, SessionFactory.GetCurrentSession() can be used

                //if it's an open session, that's all
                if (session != null && session.IsOpen)
                {
                    return session;
                }

                //if not open, open a new session
                return OpenSession();
            }
        }
        #endregion
    }
}