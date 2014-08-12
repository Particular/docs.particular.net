using System;
using System.Transactions;
using NServiceBus;


public class TransactionConfig
{
    public void Simple()
    {
        #region TransactionConfigV5

        //Enable
        Configure.With(b => b.Transactions(t => t.Enable()));

        // Disable
        Configure.With(b => b.Transactions(t => t.Disable()));

        // IsolationLevel
        Configure.With(b => b.Transactions(t => t.Advanced(a => a.IsolationLevel(IsolationLevel.Chaos))));

        // IsolationLevel
        Configure.With(b => b.Transactions(t => t.Advanced(a => a.DefaultTimeout(TimeSpan.FromMinutes(5)))));

        #endregion
    }

    
}