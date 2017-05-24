namespace Core7
{
    using NServiceBus;

    class PurgingMessagesAtStartup
    {
        PurgingMessagesAtStartup(EndpointConfiguration endpointConfiguration)
        {
            #region PurgeMessagesAtStartup

            endpointConfiguration.PurgeOnStartup(true);

            #endregion
        }

    }
}