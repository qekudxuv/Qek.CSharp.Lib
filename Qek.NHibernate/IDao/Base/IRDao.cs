using NHibernate.Criterion;
using System.Collections.Generic;

namespace Qek.NHibernate.Dao
{
    /// <summary>
    /// DateTime:   2013/07/31
    /// Author:     Sam.SH_Chang#21978
    /// Interface for DB operation, but read only. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRDao<T> : INHibernateSessionProvider
    {
        T Load(object ID);
        T Get(object ID);

        IList<T> LoadAll();
        IList<T> HqlQuery(string hql);
        IList<T> FindByCriteria(params ICriterion[] crit);
    }
}
