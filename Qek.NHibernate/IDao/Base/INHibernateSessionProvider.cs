using NHibernate;

namespace Qek.NHibernate.Dao
{
    public interface INHibernateSessionProvider
    {
        ISession Session { get; }
    }
}
