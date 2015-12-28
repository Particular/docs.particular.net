namespace Snippets6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class Upgrade
    {
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
            var busConfiguration = new BusConfiguration();
            busConfiguration.DefineCriticalErrorAction(
                new CriticalErrorAction((endpointInstance, error, exception) =>
                {
                    // place you custom handling here 
                    return Task.FromResult(0);
                }));
            #endregion
            // ReSharper restore RedundantDelegateCreation
            // ReSharper restore ConvertToLambdaExpression
        }
    }
}
