namespace Core4.Transports
{
    using NServiceBus;
    using NServiceBus.Transports;

    class RegisterQueueCreator
    {
        RegisterQueueCreator()
        {
            #region RegisteringTheQueueCreator

            Configure.Component<YourQueueCreator>(DependencyLifecycle.InstancePerCall);

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