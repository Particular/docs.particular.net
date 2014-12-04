using NServiceBus;

public class PurgeOnStartup
{
    public void Simple()
    {
        #region PurgeOnStartup

        var configuration = new BusConfiguration();

        configuration.PurgeOnStartup(true);

        #endregion
    }

}