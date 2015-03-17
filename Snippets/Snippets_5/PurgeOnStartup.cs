using NServiceBus;

public class PurgeOnStartup
{
    public void Simple()
    {
        #region PurgeOnStartup

        BusConfiguration configuration = new BusConfiguration();

        configuration.PurgeOnStartup(true);

        #endregion
    }

}