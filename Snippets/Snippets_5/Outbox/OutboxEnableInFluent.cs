using NServiceBus;

public class OutboxEnableInFluent
{
    public void Simple()
    {
        #region OutboxEnablineInFluent

        Configure.With(b => b.EnableOutbox());

        #endregion
    }

}