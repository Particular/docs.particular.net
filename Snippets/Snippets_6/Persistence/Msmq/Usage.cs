namespace Snippets6.Persistence.Msmq
{
    using NServiceBus;
    using NServiceBus.Persistence.Legacy;

    class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringMsmqPersistence

            endpointConfiguration.UsePersistence<MsmqPersistence>();

            #endregion
        }

    }
}