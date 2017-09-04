using NServiceBus;

public class CodeFirstChannels
{
    public CodeFirstChannels()
    {
        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

        #region CodeFirstChannels

        var gatewayConfig = endpointConfiguration.Gateway();

        gatewayConfig.AddReceiveChannel("http://Headquarter.mycorp.com/", isDefault: true);
        gatewayConfig.AddReceiveChannel("http://Headquarter.myotherdomain.com/", maxConcurrency: 10);

        #endregion
    }
}
