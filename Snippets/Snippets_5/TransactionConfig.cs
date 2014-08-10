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

        #endregion
    }

}