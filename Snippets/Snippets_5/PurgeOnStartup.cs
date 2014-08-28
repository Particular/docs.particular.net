using NServiceBus;

public class PurgeOnStartup
{
    public void Simple()
    {
        #region PurgeOnStartupV5

        var configuration = new BusConfiguration();

        configuration.PurgeOnStartup(true);

        #endregion
    }

}