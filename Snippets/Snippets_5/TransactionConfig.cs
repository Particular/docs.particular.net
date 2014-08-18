using System;
using System.Transactions;
using NServiceBus;


public class TransactionConfig
{
    public void Simple()
    {
        #region TransactionConfigV5

        //Enable
        Configure.With(b => b.Transactions().Enable());

        // Disable
        Configure.With(b => b.Transactions().Disable());

        // IsolationLevel
        Configure.With(b => b.Transactions().IsolationLevel(IsolationLevel.Chaos));

        // DefaultTimeout
        Configure.With(b => b.Transactions().DefaultTimeout(TimeSpan.FromMinutes(5)));

        #endregion
    }

    
}