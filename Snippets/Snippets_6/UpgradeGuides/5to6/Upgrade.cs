namespace Snippets6.UpgradeGuides._5to6
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.ConsistencyGuarantees;
    using NServiceBus.MessageMutator;
    using NServiceBus.Settings;

    public class Upgrade
    {
        #region 5to6ReAddWinIdNameHeader

        public class WinIdNameMutator : IMutateOutgoingTransportMessages
        {
            public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
            {
                context.OutgoingHeaders["WinIdName"] = Thread.CurrentPrincipal.Identity.Name;
                return Task.FromResult(0);
            }
        }
        #endregion
        void StaticHeaders()
        {

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region 5to6header-static-endpoint
            endpointConfiguration.AddHeaderToAllOutgoingMessages("MyGlobalHeader", "some static value");
            #endregion
        }


        public void TransportTransactions()
        {
            #region 5to6DoNotWrapHandlersInTransaction

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.ReceiveOnly);

            #endregion
        }


        public void CriticalError()
        {
            // ReSharper disable RedundantDelegateCreation
            // ReSharper disable ConvertToLambdaExpression
            #region 5to6CriticalError
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.DefineCriticalErrorAction(
                new Func<ICriticalErrorContext,Task>(context =>
                {
                    // place you custom handling here 
                    return Task.FromResult(0);
                }));
            #endregion
            // ReSharper restore RedundantDelegateCreation
            // ReSharper restore ConvertToLambdaExpression
        }

        public void SuppressDistributedTransactions()
        {
            ReadOnlySettings readOnlySettings = null;

            #region 5to6SuppressDistributedTransactions

            bool suppressDistributedTransactions = readOnlySettings.GetRequiredTransactionModeForReceives() != TransportTransactionMode.TransactionScope;

            #endregion
        }

        public void IsTransactional()
        {
            ReadOnlySettings readOnlySettings = null;

            #region 5to6IsTransactional
        
            bool isTransactional = readOnlySettings.GetRequiredTransactionModeForReceives() != TransportTransactionMode.None;

            #endregion
        }

        public void TransportTransactionIsolationLevelAndTimeout()
        {
            #region 5to6TransportTransactionScopeOptions
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(isolationLevel: IsolationLevel.RepeatableRead, timeout: TimeSpan.FromSeconds(30));
            #endregion
        }

        public void WrapHandlersExecutionInATransactionScope()
        {
            #region 5to6WrapHandlersExecutionInATransactionScope
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope();
            #endregion
        }
        public async Task DelayedDelivery()
        {
            IMessageHandlerContext handlerContext = null;
            object message = null;

            #region 5to6delayed-delivery
            SendOptions sendOptions = new SendOptions();
            sendOptions.DelayDeliveryWith(TimeSpan.FromMinutes(30));
            //OR
            sendOptions.DoNotDeliverBefore(new DateTime(2016, 12, 25));

            await handlerContext.Send(message, sendOptions);
            #endregion
        }

        public void EnableTransactions()
        {
            #region 5to6EnableTransactions
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            // Using a transport will enable transactions automatically.
            endpointConfiguration.UseTransport<MyTransport>();
            #endregion
        }

        public void DisableTransactions()
        {
            #region 5to6DisableTransactions
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.None);
            #endregion
        }

        public void EnableDistributedTransactions()
        {
            #region 5to6EnableDistributedTransactions
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.TransactionScope);
            #endregion
        }

        public void DisableDistributedTransactions()
        {
            #region 5to6DisableDistributedTransactions
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.ReceiveOnly);
            // Or, if the transport supports it:
            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            #endregion
        }
    }

}
