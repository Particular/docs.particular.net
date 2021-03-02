using System.Security.Cryptography.X509Certificates;
using NServiceBus;

partial class Upgrade
{
    void CertificatePath(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7certificatepath7

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            ClientCertificate = new X509Certificate2("path", "password")
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void PrefetchCount(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7prefetchcount7

        var transportWithFixedPrefetchCount = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            PrefetchCountCalculation = _ => 100
        };
        endpointConfiguration.UseTransport(transportWithFixedPrefetchCount);

        //or

        var transportWithConcurrencyBasedPrefetchCount = new RabbitMQTransport(Topology.Conventional, "host=localhost")
        {
            PrefetchCountCalculation = concurrency => concurrency * 7
        };
        endpointConfiguration.UseTransport(transportWithConcurrencyBasedPrefetchCount);

        #endregion
    }
}
