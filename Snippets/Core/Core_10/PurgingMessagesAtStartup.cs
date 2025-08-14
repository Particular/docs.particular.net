namespace Core;

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