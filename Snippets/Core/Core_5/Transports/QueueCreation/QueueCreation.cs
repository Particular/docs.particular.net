namespace Core5.Transports
{
    using NServiceBus;
    using NServiceBus.Transports;

    class RegisterQueueCreator
    {
        RegisterQueueCreator(BusConfiguration busConfiguration)
        {
            #region RegisteringTheQueueCreator

            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<YourQueueCreator>(DependencyLifecycle.InstancePerCall);
                });

            #endregion
        }
    }

    #region CustomQueueCreator
    class YourQueueCreator :
        ICreateQueues
    {
        public void CreateQueueIfNecessary(Address address, string account)
        {
            // create the queues here
        }
        #endregion
    }
}