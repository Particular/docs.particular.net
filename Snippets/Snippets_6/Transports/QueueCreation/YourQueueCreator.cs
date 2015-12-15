namespace Snippets6.Transports
{
    using System.Threading.Tasks;
    using NServiceBus.Transports;

    class YourQueueCreator : ICreateQueues
    {
        public Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
        {
            return Task.FromResult(0);
        }
    }
}