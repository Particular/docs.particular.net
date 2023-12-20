using NServiceBus;
using NServiceBus.Features;

class TransportAddresses
{
#pragma warning disable IDE0059 // Unnecessary assignment of a value
#pragma warning disable CS0618 // Type or member is obsolete
    #region core-8to9-audit-transportadresses-features
    class MyFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            var localAddress = context.Settings.LocalAddress();
            var instanceSpecificQueueAddress = context.Settings.InstanceSpecificQueue();
        }
    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore IDE0059 // Unnecessary assignment of a value
}