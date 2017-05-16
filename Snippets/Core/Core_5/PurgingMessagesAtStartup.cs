namespace Core5
{
    using NServiceBus;

    class PurgingMessagesAtStartup
    {
        PurgingMessagesAtStartup()
        {
            #region PurgeMessagesAtStartup

            var busConfiguration = new BusConfiguration();
            busConfiguration.PurgeOnStartup(true);

            #endregion
        }

    }
}