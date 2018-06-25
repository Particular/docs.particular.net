namespace Core6
{
    using System;
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Settings;
    using NServiceBus.Transport;

    class TransportTransactions
    {
        void Unreliable(EndpointConfiguration endpointConfiguration)
        {
            #region TransactionsDisable

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            transport.Transactions(TransportTransactionMode.None);

            #endregion
        }

        void TransportTransactionReceiveOnly(EndpointConfiguration endpointConfiguration)
        {
            #region TransportTransactionReceiveOnly

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            transport.Transactions(TransportTransactionMode.ReceiveOnly);

            #endregion
        }

        void TransportTransactionAtomicSendsWithReceive(EndpointConfiguration endpointConfiguration)
        {
            #region TransportTransactionAtomicSendsWithReceive

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

            #endregion
        }

        void TransportTransactionScope(EndpointConfiguration endpointConfiguration)
        {
            #region TransportTransactionScope

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            transport.Transactions(TransportTransactionMode.TransactionScope);

            #endregion
        }

        void TransportTransactionsWithScope(EndpointConfiguration endpointConfiguration)
        {
            #region TransactionsWrapHandlersExecutionInATransactionScope

            var unitOfWorkSettings = endpointConfiguration.UnitOfWork();
            unitOfWorkSettings.WrapHandlersInATransactionScope();

            #endregion
        }

        void CustomTransactionIsolationLevel(EndpointConfiguration endpointConfiguration)
        {
            #region CustomTransactionIsolationLevel

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            transport.Transactions(TransportTransactionMode.TransactionScope);
            transport.TransactionScopeOptions(
                isolationLevel: IsolationLevel.RepeatableRead);

            #endregion
        }

        void CustomTransactionTimeout(EndpointConfiguration endpointConfiguration)
        {
            #region CustomTransactionTimeout

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            transport.Transactions(TransportTransactionMode.TransactionScope);
            transport.TransactionScopeOptions(
                timeout: TimeSpan.FromSeconds(30));

            #endregion
        }
    }

    public class MyTransport :
        TransportDefinition, IMessageDrivenSubscriptionTransport
    {
        public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            throw new NotImplementedException();
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }

    static class MyTransportConfigurationExtensions
    {
        public static void TransactionScopeOptions(this TransportExtensions<MyTransport> transportExtensions, TimeSpan? timeout = null, IsolationLevel? isolationLevel = null)
        {
        }
    }
}