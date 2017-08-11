using NHibernate;
using Qek.Common;
using System;
using System.Transactions;

namespace Qek.NHibernate
{
    public class NHibernateService
    {
        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void Execute(Action<DateTime> action,
            TransactionScopeOption tsOption = TransactionScopeOption.Required,
            int timeoutSec = 3600,
            IsolationLevel isoLevel = IsolationLevel.ReadCommitted)
        {
            using (TransactionScope scope = TranScopeHelper.GetTranScope(tsOption, timeoutSec, isoLevel))
            {
                DateTime now = GetNow();
                action(now);

                scope.Complete();
            }
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void Execute(Action<ISession, DateTime> action,
            bool isOpenNewSession = false,
            TransactionScopeOption tsOption = TransactionScopeOption.Required,
            int timeoutSec = 3600,
            IsolationLevel isoLevel = IsolationLevel.ReadCommitted)
        {
            using (TransactionScope scope = TranScopeHelper.GetTranScope(tsOption, timeoutSec, isoLevel))
            using (ISession session = GetSession(isOpenNewSession))
            {
                DateTime now = GetNow();
                action(session, now);

                scope.Complete();
            }
        }

        /// <summary>
        /// Executes the boolean.
        /// </summary>
        /// <param name="f">The f.</param>
        public static void ExecuteBoolean(Func<DateTime, bool> f,
            TransactionScopeOption tsOption = TransactionScopeOption.Required,
            int timeoutSec = 3600,
            IsolationLevel isoLevel = IsolationLevel.ReadCommitted)
        {
            using (TransactionScope scope = TranScopeHelper.GetTranScope(tsOption, timeoutSec, isoLevel))
            {
                DateTime now = GetNow();
                bool flag = f(now);

                if (flag) scope.Complete();
            }
        }

        /// <summary>
        /// Executes the boolean.
        /// </summary>
        /// <param name="f">The f.</param>
        public static void ExecuteBoolean(Func<ISession, DateTime, bool> f,
            bool isOpenNewSession = false,
            TransactionScopeOption tsOption = TransactionScopeOption.Required,
            int timeoutSec = 3600,
            IsolationLevel isoLevel = IsolationLevel.ReadCommitted)
        {
            using (TransactionScope scope = TranScopeHelper.GetTranScope(tsOption, timeoutSec, isoLevel))
            using (ISession session = GetSession(isOpenNewSession))
            {
                DateTime now = GetNow();
                bool flag = f(session, now);

                if (flag) scope.Complete();
            }
        }

        private static ISession GetSession(bool isOpenNewSession)
        {
            return isOpenNewSession ?
                NHibernateSessionFactory.OpenSession() : NHibernateSessionFactory.Session;
        }

        private static DateTime GetNow(string format = "yyyy/MM/dd HH:mm:ss")
        {
            return Convert.ToDateTime(DateTime.Now.ToString(format));
        }
    }
}
