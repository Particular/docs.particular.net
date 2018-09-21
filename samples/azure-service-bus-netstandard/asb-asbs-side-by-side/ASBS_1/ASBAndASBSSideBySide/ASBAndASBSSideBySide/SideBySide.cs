namespace ASBAndASBSSideBySide
{
    //  Edit The Project And See The Reference 
    #region transport-asb-alias-definition
    extern alias TransportASB;
    #endregion

    using NServiceBus;
    using TransportASB::NServiceBus;

    public class SideBySide
    {
        public SideBySide()
        {
            var configurationASB = new EndpointConfiguration("ASB");

            #region transport-asb-alias-usage

            var transportASB = configurationASB.UseTransport<TransportASB::NServiceBus.AzureServiceBusTransport>();

            #endregion

            transportASB.Queues().EnableBatchedOperations(true);

            var configurationASBS = new EndpointConfiguration("ASBS");
            var transportASBS = configurationASBS.UseTransport<NServiceBus.AzureServiceBusTransport>();

            transportASBS.PrefetchMultiplier(1);
        }
    }
}