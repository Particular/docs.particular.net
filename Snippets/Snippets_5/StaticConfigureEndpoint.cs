using NServiceBus;

public class StaticConfigureEndpoint
{
    public void Simple()
    {
        #region StaticConfigureEndpoint

        BusConfiguration configuration = new BusConfiguration();

        // SendOnly
        Bus.CreateSendOnly(configuration);

        // AsVolatile
        configuration.Transactions().Disable();
        configuration.DisableDurableMessages();
        configuration.UsePersistence<InMemoryPersistence>();

        // DisableDurableMessages
        configuration.DisableDurableMessages();

        // EnableDurableMessages
        configuration.EnableDurableMessages();

        #endregion
    }

}