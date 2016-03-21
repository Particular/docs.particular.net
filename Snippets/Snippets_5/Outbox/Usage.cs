namespace Snippets5.Outbox
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region OutboxEnablineInCode

            busConfiguration.EnableOutbox();

            #endregion
        }

    }
}