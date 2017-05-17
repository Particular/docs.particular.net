namespace Core4
{
    using NServiceBus;

    class PurgingMessagesAtStartup
    {
        PurgingMessagesAtStartup(Configure configure)
        {
            #region PurgeMessagesAtStartup

            configure.PurgeOnStartup(true);

            #endregion
        }

    }
}