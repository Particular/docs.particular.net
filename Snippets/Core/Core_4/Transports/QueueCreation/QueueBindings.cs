using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Unicast.Queuing;

namespace Core5.Transports
{

    #region queuebindings

    public class QueueRegistration :
        IWantQueueCreated
    {
        public QueueRegistration(Address queueAddress)
        {
            Address = queueAddress;
        }

        public Address Address { get; }

        public bool IsDisabled => true;

    }

    public class FeatureThatRequiresAQueue :
        Feature
    {
        public override void Initialize()
        {
            Configure.Component(
                componentFactory: () =>
                {
                    return new QueueRegistration(Address.Parse("someQueue"));
                },
                lifecycle: DependencyLifecycle.InstancePerCall);
        }

    }

    #endregion
}