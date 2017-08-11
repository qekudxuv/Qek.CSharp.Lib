using System;
using System.Transactions;

namespace Qek.DB
{
    public class TranScopeHelper
    {
        public static TransactionScope GetTranScope(
            TransactionScopeOption tsOption = TransactionScopeOption.Required,
            int timeoutSec = 3600,
            IsolationLevel isoLevel = IsolationLevel.ReadCommitted
            )
        {
            return new TransactionScope(
                    tsOption,
                    new TransactionOptions
                    {
                        Timeout = TimeSpan.FromSeconds(timeoutSec),
                        IsolationLevel = isoLevel
                    });
        }
    }
}
