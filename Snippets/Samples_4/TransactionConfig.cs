using NServiceBus;


public class TransactionConfig
{
    public void Simple()
    {
        #region TransactionConfigV4
        //Enable
        Configure.Transactions.Enable();

        // Disable
        Configure.Transactions.Disable();
        #endregion
    }

}