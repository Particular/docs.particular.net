namespace Core6.Persistence.Msmq
{
    using NServiceBus;
    using NServiceBus.Persistence.Legacy;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringMsmqPersistence

            endpointConfiguration.UsePersistence<MsmqPersistence>();

            #endregion
        }

    }
}