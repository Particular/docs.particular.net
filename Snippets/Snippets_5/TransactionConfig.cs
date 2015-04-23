using System;
using System.Transactions;
using NServiceBus;


public class TransactionConfig
{
    public void Simple()
    {
        #region TransactionConfig

        BusConfiguration busConfiguration = new BusConfiguration();

        //Enable
        busConfiguration.Transactions().Enable();

        // Disable
        busConfiguration.Transactions().Disable();

        // IsolationLevel
        busConfiguration.Transactions().IsolationLevel(IsolationLevel.Chaos);

        // DefaultTimeout
        busConfiguration.Transactions().DefaultTimeout(TimeSpan.FromMinutes(5));

        #endregion
    }


}