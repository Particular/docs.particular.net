using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EndpointConnectionStringLookup
{
    #region ConnectionProvider
    public class ConnectionProvider
    {
        private const string ReceiverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ReceiverCatalog;Integrated Security=True";
        private const string SenderConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SenderCatalog;Integrated Security=True";

        public static async Task<SqlConnection> GetConnecton(string transportAddress)
        {
            var connectionString = transportAddress.Contains("MultiInstanceReceiver") ? ReceiverConnectionString : SenderConnectionString;
            var connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            return connection;
        }
    }
    #endregion
}
