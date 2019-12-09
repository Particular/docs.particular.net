using NServiceBus;

public class CodeFirstSites
{
    public CodeFirstSites()
    {
        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

        #region CodeFirstSites

        var gatewayConfig = endpointConfiguration.Gateway();

        gatewayConfig.AddSite("SiteA", "http://SiteA.mycorp.com/");
        gatewayConfig.AddSite("SiteB", "http://SiteB.mycorp.com/");
        #endregion
    }
}
