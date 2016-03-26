using System.Data.SqlClient;
using System.Threading.Tasks;

#region ConnectionProvider
public class ConnectionProvider
{
    const string ReceiverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ReceiverCatalog;Integrated Security=True";
    const string SenderConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SenderCatalog;Integrated Security=True";

    public static async Task<SqlConnection> GetConnecton(string transportAddress)
    {
        var connectionString = transportAddress.Equals("Samples.SqlServer.MultiInstanceSender@[dbo]") 
                                                ? ReceiverConnectionString : SenderConnectionString;
        var connection = new SqlConnection(connectionString);

        await connection.OpenAsync();

        return connection;
    }
}
#endregion
