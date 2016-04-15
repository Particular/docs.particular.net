using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Unicast.Queuing;

namespace Core5.Transports
{
    #region queuebindings
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