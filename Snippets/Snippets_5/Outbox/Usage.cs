namespace Snippets5.Outbox
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region OutboxEnablineInCode

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.EnableOutbox();

            #endregion
        }

    }
}