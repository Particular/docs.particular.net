namespace Snippets6.Persistence.Msmq
{
    using NServiceBus;
    using NServiceBus.Persistence.Legacy;

    class Usage
    {
        public void Foo()
        {
            #region ConfiguringMsmqPersistence

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UsePersistence<MsmqPersistence>();

            #endregion
        }

    }
}