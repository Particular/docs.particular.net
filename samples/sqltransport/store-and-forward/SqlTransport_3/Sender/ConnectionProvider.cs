using System.Data.SqlClient;
using System.Threading.Tasks;

public class ConnectionProvider
{
    const string ReceiverConnectionString = @"Data Source=.\SqlExpress;Database=receiver;Integrated Security=True;Min Pool Size=2;Max Pool Size=100";
    const string SenderConnectionString = @"Data Source=.\SqlExpress;Database=sender;Integrated Security=True;Min Pool Size=2;Max Pool Size=100";

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