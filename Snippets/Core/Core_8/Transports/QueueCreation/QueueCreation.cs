#pragma warning disable 1998
namespace Core8.Transports.QueueCreation
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Transport;

    class RegisterQueueCreator
    {
        RegisterQueueCreator(EndpointConfiguration configuration)
        {
            #region RegisteringTheQueueCreator

            configuration.RegisterComponents(serviceCollection =>
            {
                serviceCollection.AddTransient<ICreateQueues, YourQueueCreator>();
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