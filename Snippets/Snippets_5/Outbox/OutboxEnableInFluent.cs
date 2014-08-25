using NServiceBus;

public class OutboxEnableInFluent
{
    public void Simple()
    {
        #region OutboxEnablineInFluent

        var configuration = new BusConfiguration();

        configuration.EnableOutbox();

        #endregion
    }

}