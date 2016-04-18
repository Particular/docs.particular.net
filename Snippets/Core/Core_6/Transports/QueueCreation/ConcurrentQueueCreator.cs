namespace Core6.Transports.QueueCreation
{
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus.Transports;

    #region ConcurrentCustomQueueCreator
    class ConcurrentQueueCreator : ICreateQueues
    {
        public async Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
        {
            await Task.WhenAll(queueBindings.SendingAddresses.Select(CreateQueue));
        }
        #endregion
        static Task CreateQueue(string address)
        {
            return Task.FromResult(address);
        }
    }
}