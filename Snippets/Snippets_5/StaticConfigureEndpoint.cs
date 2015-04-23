using NServiceBus;

public class StaticConfigureEndpoint
{
    public void Simple()
    {
        #region StaticConfigureEndpoint

        BusConfiguration busConfiguration = new BusConfiguration();

        // SendOnly
        Bus.CreateSendOnly(busConfiguration);

        // AsVolatile
        busConfiguration.Transactions().Disable();
        busConfiguration.DisableDurableMessages();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        // DisableDurableMessages
        busConfiguration.DisableDurableMessages();

        // EnableDurableMessages
        busConfiguration.EnableDurableMessages();

        #endregion
    }

}