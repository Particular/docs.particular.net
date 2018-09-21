namespace ASBAndASBSSideBySide
{
    //  Edit The Project And See The Reference 
    #region transport-asb-alias-definition
    extern alias TransportASB;
    using NServiceBus;
    using TransportASB::NServiceBus;
    #endregion

    public class SideBySide
    {
        public SideBySide()
        {
            #region transport-asb-alias-usage

            var configurationASB = new EndpointConfiguration("ASB");
            var transportASB = configurationASB.UseTransport<TransportASB::NServiceBus.AzureServiceBusTransport>();

            transportASB.Queues().EnableBatchedOperations(true);

            var configurationASBS = new EndpointConfiguration("ASBS");
            var transportASBS = configurationASBS.UseTransport<NServiceBus.AzureServiceBusTransport>();

            transportASBS.PrefetchMultiplier(1);

            #endregion
        }
    }
}