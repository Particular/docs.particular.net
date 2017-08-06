using NServiceBus;
using NServiceBus.Transport.Kafka;

class Usage
{
    void KafkaTransport(EndpointConfiguration endpointConfiguration)
    {
        #region KafkaTransport

        var transport = endpointConfiguration.UseTransport<KafkaTransport>();
        transport.ConnectionString("127.0.0.1:9092");

        #endregion
    }
}
