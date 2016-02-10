namespace Snippets6.Persistence.Msmq
{
    using NServiceBus;
    using NServiceBus.Persistence.Legacy;

    class Usage
    {
        public void Foo()
        {
            #region ConfiguringMsmqPersistence

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UsePersistence<MsmqPersistence>();

            #endregion
        }

    }
}