using System.Data.SqlClient;
using System.Threading.Tasks;

#region SenderConnectionProvider

static class ConnectionProvider
{
    public const string DefaultConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceSender;Integrated Security=True;Max Pool Size=100";
    const string ReceiverConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceReceiver;Integrated Security=True;Max Pool Size=100";

    public static async Task<SqlConnection> GetConnection(string transportAddress)
    {
        string connectionString;
        if (transportAddress.StartsWith("Samples.SqlServer.MultiInstanceReceiver"))
        {
            connectionString = ReceiverConnectionString;
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
#endregion