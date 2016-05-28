namespace Core6.Transports.QueueCreation
{
    using System.Threading.Tasks;
    using NServiceBus.Transports;

    #region SequentialCustomQueueCreator
    class SequentialQueueCreator : ICreateQueues
    {
        public async Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
        {
            foreach (var address in queueBindings.ReceivingAddresses)
            {
                await CreateQueue(address)
                    .ConfigureAwait(false);
            }
        }
        #endregion
        static Task CreateQueue(string address)
        {
            return Task.FromResult(address);
        }
    }

}