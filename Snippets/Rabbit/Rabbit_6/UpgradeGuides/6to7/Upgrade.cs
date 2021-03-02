using NServiceBus;

partial class Upgrade
{
    void CertificatePath(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7certificatepath6

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.SetClientCertificate("path", "password");

        #endregion
    }

    void PrefetchCount(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7prefetchcount6

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

        transport.PrefetchCount(100);
        //or
        transport.PrefetchMultiplier(7);

        #endregion
    }
}