using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Unicast.Queuing;

namespace Snippets5.UpgradeGuides._5to6
{
    #region 5to6queuebindings
    public class QueueRegistration : IWantQueueCreated
    {
        public QueueRegistration(Address queueAddress)
        {
            Address = queueAddress;
        }

        public Address Address { get; private set; }

        public bool ShouldCreateQueue()
        {
            return true;
        }
    }

    public class FeatureThatRequiresAQueue : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Container.ConfigureComponent(() => new QueueRegistration(Address.Parse("someQueue")), DependencyLifecycle.InstancePerCall);
        }
    }
    #endregion
}