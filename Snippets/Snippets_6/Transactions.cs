namespace Snippets6
{
    using System;
    using System.Collections.Generic;
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Settings;
    using NServiceBus.Transports;

    public class Transactions
    {
        public void Unreliable()
        {
            #region TransactionsDisable
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.None);
            #endregion
        }

        public void TransportTransactions()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region TransactionsDisableDistributedTransactionsReceiveOnly
            busConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.ReceiveOnly);
            #endregion

            #region TransactionsDisableDistributedTransactionsAtomic
            busConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            #endregion

        }

        public void AmbientTransactions()
        {
            #region TransactionsEnable
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.TransactionScope);
            #endregion
        }

        public void TransportTransactionsWithScope()
        {
            #region TransactionsWrapHandlersExecutionInATransactionScope
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions().WrapHandlersExecutionInATransactionScope();
            #endregion
        }

        public void CustomTransactionTimeout()
        {
            #region CustomTransactionTimeout
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions().DefaultTimeout(TimeSpan.FromSeconds(30));
            #endregion
        }

        public void CustomTransactionIsolationLevel()
        {
            #region CustomTransactionIsolationLevel
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions().IsolationLevel(IsolationLevel.RepeatableRead);
            #endregion
        }
    }

    public class MyTransport : TransportDefinition
    {
        protected override TransportReceivingConfigurationResult ConfigureForReceiving(TransportReceivingConfigurationContext context)
        {
            throw new NotImplementedException();
        }

        protected override TransportSendingConfigurationResult ConfigureForSending(TransportSendingConfigurationContext context)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Type> GetSupportedDeliveryConstraints()
        {
            throw new NotImplementedException();
        }

        public override TransportTransactionMode GetSupportedTransactionMode()
        {
            throw new NotImplementedException();
        }

        public override IManageSubscriptions GetSubscriptionManager()
        {
            throw new NotImplementedException();
        }

        public override EndpointInstance BindToLocalEndpoint(EndpointInstance instance, ReadOnlySettings settings)
        {
            throw new NotImplementedException();
        }

        public override string ToTransportAddress(LogicalAddress logicalAddress)
        {
            throw new NotImplementedException();
        }

        public override OutboundRoutingPolicy GetOutboundRoutingPolicy(ReadOnlySettings settings)
        {
            throw new NotImplementedException();
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }
}