namespace Core5.Transports
{
    using NServiceBus;
    using NServiceBus.Transports;

    #region RegisteringTheQueueCreator
    class RegisterQueueCreator :
        INeedInitialization
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<YourQueueCreator>(DependencyLifecycle.InstancePerCall));
        }
    }
    #endregion

    class YourQueueCreator :
        ICreateQueues
    {
        public void CreateQueueIfNecessary(Address address, string account)
        {
        }
    }

    #region SequentialCustomQueueCreator
    class SequentialQueueCreator :
        ICreateQueues
    {
        public void CreateQueueIfNecessary(Address address, string account)
        {
            // create the queues here
        }
        #endregion
    }
}