using NServiceBus;

partial class Upgrade
{
    void CustomConnectionStringName(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5CustomConnectionStringName

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionStringName("MyConnectionStringName");

        #endregion
    }

}