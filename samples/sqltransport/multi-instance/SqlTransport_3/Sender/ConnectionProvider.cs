using System.Data.SqlClient;
using System.Threading.Tasks;

#region SenderConnectionProvider

static class ConnectionProvider
{
    const string DefaultConnectionString = @"Data Source=.\SqlExpress;Database=SenderCatalog;Integrated Security=True;Max Pool Size=100";
    const string ReceiverConnectionString = @"Data Source=.\SqlExpress;Database=ReceiverCatalog;Integrated Security=True;Max Pool Size=100";

    public static async Task<SqlConnection> GetConnection(string transportAddress)
    {
        var connectionString = transportAddress.StartsWith("Samples.SqlServer.MultiInstanceReceiver")
            ? ReceiverConnectionString
            : DefaultConnectionString;

        var connection = new SqlConnection(connectionString);

        await connection.OpenAsync()
            .ConfigureAwait(false);

        return connection;
    }
}
#endregion