using NServiceBus;
using NServiceBus.Gateway;

public class CodeFirstChannels
{
    public CodeFirstChannels()
    {
        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

        #region CodeFirstChannels

        var config = new NonDurableDeduplicationConfiguration();
        var gatewayConfig = endpointConfiguration.Gateway(config);

        gatewayConfig.AddReceiveChannel("http://Headquarter.mycorp.com/", isDefault: true);
        gatewayConfig.AddReceiveChannel("http://Headquarter.myotherdomain.com/", maxConcurrency: 10);

        #endregion
    }
}
