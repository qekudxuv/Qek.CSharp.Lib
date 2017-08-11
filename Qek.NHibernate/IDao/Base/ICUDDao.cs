namespace Qek.NHibernate.Dao
{
    /// <summary>
    /// DateTime:   2013/07/31
    /// Author:     Sam.SH_Chang#21978
    /// Interface for DB operation including CUD(Create,Update,Delete) 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICUDDao<T> : INHibernateSessionProvider
    {
        T Save(T entity);

        void SaveOrUpdate(T entity);

        void Update(T entity);

        void Delete(T entity);

        int Delete(string deleteSql);

        T Merge(T entity);
    }
}
