namespace Core6
{
    using NServiceBus;

    class PurgingMessagesAtStartup
    {
        PurgingMessagesAtStartup()
        {
            #region PurgeMessagesAtStartup

            var endpointConfiguration = new EndpointConfiguration("endpoint_name");
            endpointConfiguration.PurgeOnStartup(true);

            #endregion
        }

    }
}