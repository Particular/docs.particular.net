namespace Snippets5.Persistence.Msmq
{
    using NServiceBus;
    using NServiceBus.Persistence.Legacy;

    class Usage
    {
        public void Foo()
        {
            #region ConfiguringMsmqPersistence

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UsePersistence<MsmqPersistence>();

            #endregion
        }

    }
}