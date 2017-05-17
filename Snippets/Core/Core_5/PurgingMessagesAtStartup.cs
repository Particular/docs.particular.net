namespace Core5
{
    using NServiceBus;

    class PurgingMessagesAtStartup
    {
        PurgingMessagesAtStartup(BusConfiguration busConfiguration)
        {
            #region PurgeMessagesAtStartup

            busConfiguration.PurgeOnStartup(true);

            #endregion
        }

    }
}