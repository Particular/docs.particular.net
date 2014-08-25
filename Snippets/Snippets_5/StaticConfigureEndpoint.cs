using NServiceBus;
using NServiceBus.Persistence;

public class StaticConfigureEndpoint
{
    public void Simple()
    {
        #region StaticConfigureEndpointReplacementV5

        var configuration = new BusConfiguration();

        // SendOnly
        configuration.SendOnly();

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