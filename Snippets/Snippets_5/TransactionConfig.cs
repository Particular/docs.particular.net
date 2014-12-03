using System;
using System.Transactions;
using NServiceBus;


public class TransactionConfig
{
    public void Simple()
    {
        #region TransactionConfig

        var configuration = new BusConfiguration();

        //Enable
        configuration.Transactions().Enable();

        // Disable
        configuration.Transactions().Disable();

        // IsolationLevel
        configuration.Transactions().IsolationLevel(IsolationLevel.Chaos);

        // DefaultTimeout
        configuration.Transactions().DefaultTimeout(TimeSpan.FromMinutes(5));

        #endregion
    }


}