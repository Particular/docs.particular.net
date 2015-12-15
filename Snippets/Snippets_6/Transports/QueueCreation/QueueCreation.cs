namespace Snippets6.Transports
{
    using System;
    using System.Collections.Generic;
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
                () => new YourMessagePump(),
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

        public override TransportTransactionMode GetSupportedTransactionMode()
        {
            throw new NotImplementedException();
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

}