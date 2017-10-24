using NServiceBus;
class ConfigureTransport
{
    ConfigureTransport(EndpointConfiguration endpointConfiguration)
    {
        #region msmq-config-basic
        
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.ConnectionString("deadLetter=false;");

        #endregion
    }
}
