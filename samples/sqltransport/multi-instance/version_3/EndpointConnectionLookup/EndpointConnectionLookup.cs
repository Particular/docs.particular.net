using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EndpointConnectionStringLookup
{
    public class EndpointConnectionLookup
    {
        private const string ReceiverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ReceiverCatalog;Integrated Security=True";
        private const string SenderConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SenderCatalog;Integrated Security=True";

        public static Func<string, Task<SqlConnection>> GetLookupFunc()
        {
            Func<string, Task<SqlConnection>> lookupTask = (async endpointName =>
            {
                var connectionString = endpointName.Contains("MultiInstanceReceiver") ? ReceiverConnectionString : SenderConnectionString;
                var connection = new SqlConnection(connectionString);

                await connection.OpenAsync();

                return connection;
            });

            return lookupTask;
        }
    }
}
