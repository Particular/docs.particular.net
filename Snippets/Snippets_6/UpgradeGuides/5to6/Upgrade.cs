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

            BusConfiguration busConfiguration = new BusConfiguration();
            #region 5to6header-static-endpoint
            busConfiguration.AddHeaderToAllOutgoingMessages("MyGlobalHeader", "some static value");
            #endregion
        }


        public void TransportTransactions()
        {
            #region 5to6DoNotWrapHandlersInTransaction

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.ReceiveOnly);

            #endregion
        }


        public void CriticalError()
        {
            // ReSharper disable RedundantDelegateCreation
            // ReSharper disable ConvertToLambdaExpression
            #region 5to6CriticalError
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.DefineCriticalErrorAction(
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
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(isolationLevel: IsolationLevel.RepeatableRead, timeout: TimeSpan.FromSeconds(30));
            #endregion
        }

        public void WrapHandlersExecutionInATransactionScope()
        {
            #region 5to6WrapHandlersExecutionInATransactionScope
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope();
            #endregion
        }
    }
}
