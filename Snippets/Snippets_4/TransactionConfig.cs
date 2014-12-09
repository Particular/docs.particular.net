using NServiceBus;


public class TransactionConfig
{
    public void Simple()
    {
        #region TransactionConfig

        //Enable
        Configure.Transactions.Enable();

        // Disable
        Configure.Transactions.Disable();

        #endregion
    }

}