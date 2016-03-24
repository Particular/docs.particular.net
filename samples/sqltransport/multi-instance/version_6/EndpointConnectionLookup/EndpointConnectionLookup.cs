using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserImplementation
{
    public class EndpointConnectionLookup
    {
        private const string ServerConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = samples; Integrated Security = True";
        private const string ClientConnectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = samples; Integrated Security = True";

        public static Func<string, Task<SqlConnection>> GetLookupFunc()
        {
            Func<string, Task<SqlConnection>> lookupTask = (async endpointName =>
            {
                var connectionString = endpointName.Contains("MultiInstanceServer") ? ServerConnectionString : ClientConnectionString;
                var connection = new SqlConnection(connectionString);

                await connection.OpenAsync();

                return connection;
            });

            return lookupTask;
        }
    }
}
