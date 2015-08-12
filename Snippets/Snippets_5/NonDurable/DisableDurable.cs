namespace Snippets5.NonDurable
{
    using NServiceBus;

    public class DisableDurable
    {
        public DisableDurable()
        {
            #region set-to-non-durable

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.DisableDurableMessages();

            #endregion
        }

    }
}