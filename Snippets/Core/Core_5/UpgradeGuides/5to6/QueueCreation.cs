namespace Core5.UpgradeGuides._5to6
{
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Transports;
    using NServiceBus.Unicast.Queuing;

    #region 5to6-QueueCreation

    public class QueueRegistration :
        IWantQueueCreated
    {
        public QueueRegistration(Address queueAddress)
        {
            Address = queueAddress;
        }

        public Address Address { get; }

        public bool ShouldCreateQueue()
        {
            return true;
        }
    }

    public class FeatureThatRequiresAQueue :
        Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            var container = context.Container;
            container.ConfigureComponent(
                componentFactory: () =>
                {
                    return new Transports.QueueRegistration(Address.Parse("someQueue"));
                },
                dependencyLifecycle: DependencyLifecycle.InstancePerCall);
        }
    }

    class YourQueueCreator :
        ICreateQueues
    {
        public void CreateQueueIfNecessary(Address address, string account)
        {
            // create the queues here
        }
    }

    #endregion
}