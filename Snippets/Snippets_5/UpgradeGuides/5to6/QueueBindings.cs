using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Unicast.Queuing;

namespace Snippets5.UpgradeGuides._5to6
{
    #region 5to6queuebindings
    public class QueueRegistration : IWantQueueCreated
    {
        public Address AuditQueue { get; set; }

        public Address Address
        {
            get { return AuditQueue; }
        }

        public bool Enabled { get; set; }

        public bool ShouldCreateQueue()
        {
            return Enabled;
        }
    }

    public class FeatureThatRequiresAQueue : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Container.ConfigureComponent<QueueRegistration>(DependencyLifecycle.InstancePerCall)
                .ConfigureProperty(p => p.Enabled, true)
                .ConfigureProperty(t => t.AuditQueue, Address.Parse("someQueue"));
        }
    }
    #endregion
}