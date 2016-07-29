namespace Core6.Transports.QueueCreation
{
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus.Transport;

    #region ConcurrentCustomQueueCreator
    class ConcurrentQueueCreator :
        ICreateQueues
    {
        public Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
        {
            return Task.WhenAll(queueBindings.SendingAddresses.Select(CreateQueue));
        }
        #endregion
        static Task CreateQueue(string address)
        {
            return Task.FromResult(address);
        }
    }
}