using NServiceBus;

public class WildcardChannels
{
    public WildcardChannels(EndpointConfiguration endpointConfiguration)
    {
        #region configureWildcardGatewayChannel

        var gatewayConfig = endpointConfiguration.Gateway();

        gatewayConfig.AddReceiveChannel("http://+:25899/RemoteSite/");
        gatewayConfig.AddReceiveChannel("http://gateway.mycorp.com:25900/RemoteSite/", isDefault: true);

        #endregion
    }
}