using System.Data.SqlClient;
using System.Threading.Tasks;

public class ConnectionProvider
{
    const string ReceiverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=receiver;Integrated Security=True";
    const string SenderConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=sender;Integrated Security=True";

    public static async Task<SqlConnection> GetConnecton(string transportAddress)
    {
        var connectionString = transportAddress.StartsWith("Samples.SqlServer.StoreAndForwardSender") || transportAddress == "error"
            ? SenderConnectionString
            : ReceiverConnectionString;

        var connection = new SqlConnection(connectionString);

        await connection.OpenAsync()
            .ConfigureAwait(false);

        return connection;
    }
}