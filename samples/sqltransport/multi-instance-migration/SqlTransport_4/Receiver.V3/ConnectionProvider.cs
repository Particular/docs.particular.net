using System.Data.SqlClient;
using System.Threading.Tasks;

static class ConnectionProvider
{
    public const string DefaultConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceReceiver3;Integrated Security=True;Max Pool Size=100";
    const string SenderConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceSender3;Integrated Security=True;Max Pool Size=100";

    public static async Task<SqlConnection> GetConnection(string transportAddress)
    {
        string connectionString;
        if (transportAddress.StartsWith("Samples.SqlServer.MultiInstanceSender"))
        {
            connectionString = SenderConnectionString;
        }
        else
        {
            connectionString = DefaultConnectionString;
        }

        var connection = new SqlConnection(connectionString);

        await connection.OpenAsync()
            .ConfigureAwait(false);

        return connection;
    }
}