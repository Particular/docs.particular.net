namespace Core7.Handlers.DataAccess
{
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using NServiceBus;

    public class Program
    {
        public void Main()
        {
            var transport = new EndpointConfiguration("dummy").UseTransport<LearningTransport>();

            #region BusinessData-ConfigureTransactionScope

            transport.Transactions(TransportTransactionMode.TransactionScope);

            #endregion
        }
    }

    public class TransactionScope : IHandleMessages<MyMessage>
    {
        #region BusinessData-InsideTransactionScope

        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var connection = new SqlConnection(Configuration.ConnectionString);
            var command = CreateStoreOrderCommand(message);
            await command.ExecuteReaderAsync().ConfigureAwait(false);
        }

        #endregion

        SqlCommand CreateStoreOrderCommand(MyMessage message)
        {
            return new SqlCommand();
        }
    }

    public static class Configuration
    {
        public static string ConnectionString { get; set; }
    }
}
