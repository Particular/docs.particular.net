namespace Snippets6.Transports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Settings;
    using NServiceBus.Transports;

    #region RegisteringTheQueueCreator
    class MyTransport : TransportDefinition
    {
        protected override TransportReceivingConfigurationResult ConfigureForReceiving(TransportReceivingConfigurationContext context)
        {
            return new TransportReceivingConfigurationResult(
                c => default(IPushMessages),
                () => new YourQueueCreator(),
                () => Task.FromResult(StartupCheckResult.Success));
        }
        #endregion

        protected override TransportSendingConfigurationResult ConfigureForSending(TransportSendingConfigurationContext context)
        {
            return null;
        }

        public override IEnumerable<Type> GetSupportedDeliveryConstraints()
        {
            yield break;
        }

        public override TransactionSupport GetTransactionSupport()
        {
            return TransactionSupport.None;
        }

        public override IManageSubscriptions GetSubscriptionManager()
        {
            return null;
        }

        public override string GetDiscriminatorForThisEndpointInstance(ReadOnlySettings settings)
        {
            return null;
        }

        public override string ToTransportAddress(LogicalAddress logicalAddress)
        {
            return null;
        }

        public override OutboundRoutingPolicy GetOutboundRoutingPolicy(ReadOnlySettings settings)
        {
            return null;
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }

    class YourQueueCreator : ICreateQueues
    {
        public Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
        {
            return Task.FromResult(0);
        }
    }

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