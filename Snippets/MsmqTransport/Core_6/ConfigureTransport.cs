using NServiceBus;
class ConfigureTransport
{
    ConfigureTransport(EndpointConfiguration endpointConfiguration)
    {
        #region msmq-config-basic [6.0,)

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.ConnectionString("deadLetter=false;");

        #endregion
    }
}
