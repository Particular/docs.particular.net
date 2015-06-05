namespace Snippets5
{
    using NServiceBus;

    public class PurgeOnStartup
    {
        public void Simple()
        {
            #region PurgeOnStartup

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.PurgeOnStartup(true);

            #endregion
        }

    }
}