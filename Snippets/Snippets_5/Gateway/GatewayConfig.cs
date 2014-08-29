using NServiceBus;
using NServiceBus.Features;

public class GatewayConfig
{
    public GatewayConfig()
    {
        #region GatewayConfiguration-v5

        var configuration = new BusConfiguration();

        configuration.EnableFeature<Gateway>();

        #endregion

    }

}
