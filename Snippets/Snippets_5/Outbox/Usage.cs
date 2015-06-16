namespace Snippets5.Outbox
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region OutboxEnablineInFluent

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.EnableOutbox();

            #endregion
        }

    }
}