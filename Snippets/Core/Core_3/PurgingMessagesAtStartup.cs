namespace Core3
{
    using NServiceBus;

    class PurgingMessagesAtStartup
    {
        PurgingMessagesAtStartup()
        {
            #region PurgeMessagesAtStartup

            var configure = Configure.With();
            configure.PurgeOnStartup(true);

            #endregion
        }

    }
}