using System.Data.SqlClient;
using System.Threading.Tasks;

#region ConnectionProvider
public class ConnectionProvider
{
    string ReceiverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ReceiverCatalog;Integrated Security=True";
    string SenderConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SenderCatalog;Integrated Security=True";

    public static async Task<SqlConnection> GetConnection(string transportAddress)
    {
        var connectionString = transportAddress.StartsWith("Samples.SqlServer.MultiInstanceSender")
                                                ? SenderConnectionString
                                                : ReceiverConnectionString;
        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync()
            .ConfigureAwait(false);
        return connection;
    }
}
#endregion
