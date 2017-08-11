using NHibernate;

namespace Qek.NHibernate.Dao
{
    /// <summary>
    /// DateTime:   2013/07/31
    /// Author:     Sam.SH_Chang#21978
    /// a generic Dao of implementing ICUDDao<T>, IRDao<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CRUDDao<T> : RDao<T>, ICUDDao<T>, IRDao<T> where T : class
    {
        public T Save(T entity)
        {
            using (ITransaction tran = Session.BeginTransaction())
            {
                try
                {
                    Session.Save(entity);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                return entity;
            }
        }
        public void SaveOrUpdate(T entity)
        {
            using (ITransaction tran = Session.BeginTransaction())
            {
                try
                {
                    Session.SaveOrUpdate(entity);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
        public void Update(T entity)
        {
            using (ITransaction tran = Session.BeginTransaction())
            {
                try
                {
                    Session.Update(entity);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
        public void Delete(T entity)
        {
            using (ITransaction tran = Session.BeginTransaction())
            {
                try
                {
                    Session.Delete(entity);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
        public int Delete(string deleteSql)
        {
            using (ITransaction tran = Session.BeginTransaction())
            {
                try
                {
                    int num = Session.Delete(deleteSql);
                    tran.Commit();
                    return num;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
        public T Merge(T entity)
        {
            using (ITransaction tran = Session.BeginTransaction())
            {
                try
                {
                    Session.Merge(entity);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                return entity;
            }
        }
    }
}
