using System.Security.Cryptography.X509Certificates;
using NServiceBus;

partial class Upgrade
{
    void CertificatePath(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7certificatepath7

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost");

        transport.ClientCertificate = new X509Certificate2("path", "password");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }

    void PrefetchCount(EndpointConfiguration endpointConfiguration)
    {
        #region 6to7prefetchcount7

        var transport = new RabbitMQTransport(Topology.Conventional, "host=localhost");

        transport.PrefetchCountCalculation = _ => 100;
        //or
        transport.PrefetchCountCalculation = concurrency => concurrency * 7;

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}