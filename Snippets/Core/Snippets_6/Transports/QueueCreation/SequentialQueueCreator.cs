namespace Snippets6.Transports.QueueCreation
{
    using System.Threading.Tasks;
    using NServiceBus.Transports;

    #region SequentialCustomQueueCreator
    class SequentialQueueCreator : ICreateQueues
    {
        public async Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
        {
            foreach (string address in queueBindings.ReceivingAddresses)
            {
                await CreateQueue(address);
            }
        }
        #endregion
        static Task CreateQueue(string address)
        {
            return Task.FromResult(address);
        }
    }

}