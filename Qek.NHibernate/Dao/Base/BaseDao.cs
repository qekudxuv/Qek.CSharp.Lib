using NHibernate;

namespace Qek.NHibernate.Dao
{
    public abstract class BaseDao
    {
        public ISession Session
        {
            get
            {
                return NHibernateSessionFactory.Session;
            }
        }

        protected IStatelessSession StatelessSession
        {
            get
            {
                return NHibernateSessionFactory.OpenStatelessSession();
            }
        }
    }
}
