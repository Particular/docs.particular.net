#pragma warning disable 1998
namespace Core6.Transports.QueueCreation
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Transport;

    class RegisterQueueCreator
    {
        RegisterQueueCreator(EndpointConfiguration configuration)
        {
            #region RegisteringTheQueueCreator

            configuration.RegisterComponents(
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
        public async Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
        {
            // create the queues here
        }

        #endregion
    }
}