namespace ASBAndASBSSideBySide
{
    extern alias TransportASBS; //  Edit The Project And See The Reference 
    using NServiceBus;

    public class SideBySide
    {
        public SideBySide()
        {
            var configurationASB = new EndpointConfiguration("ASB");
            var transportASB = configurationASB.UseTransport<NServiceBus.AzureServiceBusTransport>();

            transportASB.Queues().EnableBatchedOperations(true);

            var configurationASBS = new EndpointConfiguration("ASBS");
            var transportASBS = configurationASBS.UseTransport<TransportASBS::NServiceBus.AzureServiceBusTransport>();

            transportASBS.PrefetchMultiplier(1);
        }
    }
}