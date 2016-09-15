using System.Data.SqlClient;
using System.Threading.Tasks;

static class ConnectionProvider
{
    const string DefaultConnectionString = @"Data Source=.\SqlExpress;Database=ReceiverCatalog;Integrated Security=True";
    const string SenderConnectionString = @"Data Source=.\SqlExpress;Database=SenderCatalog;Integrated Security=True";

    public static async Task<SqlConnection> GetConnection(string transportAddress)
    {
        var connectionString = transportAddress.StartsWith("Samples.SqlServer.MultiInstanceSender")
            ? SenderConnectionString
            : DefaultConnectionString;

        var connection = new SqlConnection(connectionString);

        await connection.OpenAsync()
            .ConfigureAwait(false);

        return connection;
    }
}