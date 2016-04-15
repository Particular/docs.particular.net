namespace Snippets5.Persistence.Msmq
{
    using NServiceBus;
    using NServiceBus.Persistence.Legacy;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region ConfiguringMsmqPersistence

            busConfiguration.UsePersistence<MsmqPersistence>();

            #endregion
        }

    }
}