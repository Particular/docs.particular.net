namespace Core5.NonDurable
{
    using NServiceBus;

    class DisableDurable
    {
        DisableDurable(BusConfiguration busConfiguration)
        {
            #region set-to-non-durable

            busConfiguration.DisableDurableMessages();

            #endregion
        }

    }
}