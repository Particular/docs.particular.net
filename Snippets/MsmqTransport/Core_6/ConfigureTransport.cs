using NServiceBus;
class ConfigureTransport
{
    ConfigureTransport(EndpointConfiguration endpointConfiguration)
    {
        #region msmq-config-basic
        
        endpointConfiguration.UseTransport<MsmqTransport>();

        #endregion
    }
}
