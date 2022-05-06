using NServiceBus;

partial class Upgrade
{
    void CertificatePath(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8certificatepath7

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.SetClientCertificate("path", "password");

        #endregion
    }

    void PrefetchCount(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8prefetchcount7

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

        transport.PrefetchCount(100);
        //or
        transport.PrefetchMultiplier(7);

        #endregion
    }
}