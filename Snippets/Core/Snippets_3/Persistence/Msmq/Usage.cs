namespace Core3.Persistence.Msmq
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region ConfiguringMsmqPersistence

            configure.MsmqSubscriptionStorage();

            #endregion
        }

    }
}