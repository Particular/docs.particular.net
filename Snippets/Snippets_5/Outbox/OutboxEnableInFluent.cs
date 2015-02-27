using NServiceBus;

public class OutboxEnableInFluent
{
    public void Simple()
    {
        #region OutboxEnablineInFluent

        BusConfiguration configuration = new BusConfiguration();

        configuration.EnableOutbox();

        #endregion
    }

}