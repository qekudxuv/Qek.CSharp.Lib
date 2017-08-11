using FluentNHibernate.Testing;
using Qek.NHibernate;
using System;

namespace Qek.UnitTestUtil
{
    public static class VerifyMappingUtil
    {
        public static void Verify<T>(T entity)
        {
            var now = DateTime.Now;
            using (var session = NHibernateSessionFactory.Session)
            using (var tran = session.BeginTransaction())
            {
                try
                {
                    new PersistenceSpecification<T>(session)
                        .VerifyTheMappings(entity);
                }
                finally
                {
                    tran.Rollback();
                }
            }
        }
    }
}
