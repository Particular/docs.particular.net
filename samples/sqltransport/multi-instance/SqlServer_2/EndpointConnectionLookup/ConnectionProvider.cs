using NServiceBus.Transports.SQLServer;

#region ConnectionProvider
public class ConnectionProvider
{
    public const string ReceiverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ReceiverCatalog;Integrated Security=True";
    public const string SenderConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SenderCatalog;Integrated Security=True";

    public static ConnectionInfo GetConnection(string transportAddress)
    {
        string connectionString = transportAddress.Equals("Samples.SqlServer.MultiInstanceSender")
                                                ? SenderConnectionString : ReceiverConnectionString;

        return ConnectionInfo.Create()
                .UseConnectionString(connectionString);
    }
}
#endregion
