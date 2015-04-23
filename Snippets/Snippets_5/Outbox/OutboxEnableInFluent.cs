using NServiceBus;

public class OutboxEnableInFluent
{
    public void Simple()
    {
        #region OutboxEnablineInFluent

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EnableOutbox();

        #endregion
    }

}