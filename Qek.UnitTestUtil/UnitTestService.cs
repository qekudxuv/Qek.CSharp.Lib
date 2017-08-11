using NHibernate;
using Ploeh.AutoFixture;
using Qek.DB;
using Qek.NHibernate;
using System;
using System.Transactions;

namespace Qek.UnitTestUtil
{
    public class UnitTestService
    {
        /// <summary>
        /// Normal test.
        /// </summary>
        /// <param name="action">The test action.</param>
        /// <param name="ignoreEntityID">if set to <c>true</c> [ignore entity ID].</param>
        /// <param name="stringLength">Length of the fake string.</param>
        public static void NormalTest(Action<Fixture> action, bool ignoreEntityID = true, int stringLength = 30)
        {
            var fixture = AutoFixtureUtil.GetFixture(ignoreEntityID, stringLength);
            action(fixture);
        }

        /// <summary>
        /// Sands the box db test.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="ignoreEntityID">if set to <c>true</c> [ignore entity ID].</param>
        /// <param name="stringLength">Length of the string.</param>
        public static void SandBoxDbTest(Action<Fixture> action, bool ignoreEntityID = true, int stringLength = 30)
        {
            using (TransactionScope scope = TranScopeHelper.GetTranScope())
            {
                var fixture = AutoFixtureUtil.GetFixture(ignoreEntityID, stringLength);
                action(fixture);
            }
        }

        /// <summary>
        /// Sandbox db test.
        /// </summary>
        /// <param name="action">The test action.</param>
        /// <param name="ignoreEntityID">if set to <c>true</c> [ignore entity ID].</param>
        /// <param name="stringLength">Length of the fake string.</param>
        public static void SandBoxDbTest(Action<Fixture, ISession> action, bool ignoreEntityID = true, int stringLength = 30)
        {
            using (TransactionScope scope = TranScopeHelper.GetTranScope())
            using (ISession session = NHibernateSessionFactory.Session)
            {
                var fixture = AutoFixtureUtil.GetFixture(ignoreEntityID, stringLength);
                action(fixture, session);
            }
        }
    }
}
