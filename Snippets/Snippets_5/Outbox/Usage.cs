namespace Snippets5.Outbox
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region OutboxEnablineInCode [5,6]

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.EnableOutbox();

            #endregion
        }

    }
}