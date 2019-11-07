using NServiceBus;
using NServiceBus.Transport.SQLServer;

class EnableMessageDrivenPubSubCompatibilityMode
{
    public EnableMessageDrivenPubSubCompatibilityMode(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-enable-message-driven-pub-sub-compatibility-mode

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var pubSubCompatibilityMode = transport.EnableMessageDrivenPubSubCompatibilityMode();

        #endregion
    }
}