using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace Qek.NHibernate.Dao
{
    /// <summary>
    /// DateTime:   2013/07/31
    /// Author:     Sam.SH_Chang#21978
    /// a generic Dao of implementing IRDao<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RDao<T> : BaseDao, IRDao<T> where T : class
    {
        public T Load(object id)
        {
            return (T)Session.Load(typeof(T), id);
        }
        public T Get(object id)
        {
            return (T)Session.Get(typeof(T), id);
        }
        public IList<T> LoadAll()
        {
            return Session.CreateCriteria(typeof(T)).List<T>();
        }
        public IList<T> HqlQuery(string hql)
        {
            return Session.CreateQuery(hql).List<T>();
        }
        public IList<T> FindByCriteria(params ICriterion[] crits)
        {
            ICriteria crit = Session.CreateCriteria(typeof(T));
            foreach (ICriterion c in crits)
            {
                crit.Add(c);
            }
            return crit.List<T>();
        }
    }
}
