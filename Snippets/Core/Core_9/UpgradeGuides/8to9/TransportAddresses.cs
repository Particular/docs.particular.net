using NServiceBus;
using NServiceBus.Features;

class TransportAddresses
{
#pragma warning disable IDE0059 // Unnecessary assignment of a value
    #region core-8to9-audit-transportadresses-features
    class MyFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            var localAddress = context.LocalQueueAddress();
            var instanceSpecificQueueAddress = context.InstanceSpecificQueueAddress();
        }
    }
    #endregion
#pragma warning restore IDE0059 // Unnecessary assignment of a value
}