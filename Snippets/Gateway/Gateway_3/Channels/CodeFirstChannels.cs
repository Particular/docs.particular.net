using NServiceBus;
using NServiceBus.Gateway;

public class CodeFirstChannels
{
    public CodeFirstChannels()
    {
        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

        #region CodeFirstChannels

        var gatewayConfig = endpointConfiguration.Gateway(new InMemoryDeduplicationConfiguration());

        gatewayConfig.AddReceiveChannel("http://Headquarter.mycorp.com/", isDefault: true);
        gatewayConfig.AddReceiveChannel("http://Headquarter.myotherdomain.com/", maxConcurrency: 10);

        #endregion
    }
}
