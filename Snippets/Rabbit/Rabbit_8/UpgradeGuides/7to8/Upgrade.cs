using System.Security.Cryptography.X509Certificates;
using NServiceBus;

partial class Upgrade
{
    void CertificatePath(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8certificatepath8

        var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), "host=localhost")
        {
            ClientCertificate = new X509Certificate2("path", "password")
        };

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void PrefetchCount(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8prefetchcount8

        var transportWithFixedPrefetchCount = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), "host=localhost")
        {
            PrefetchCountCalculation = _ => 100
        };
        endpointConfiguration.UseTransport(transportWithFixedPrefetchCount);

        //or

        var transportWithConcurrencyBasedPrefetchCount = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), "host=localhost")
        {
            PrefetchCountCalculation = concurrency => concurrency * 7
        };
        endpointConfiguration.UseTransport(transportWithConcurrencyBasedPrefetchCount);

        #endregion
    }
}
