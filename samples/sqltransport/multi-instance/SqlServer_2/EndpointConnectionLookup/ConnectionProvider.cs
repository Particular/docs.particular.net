using NServiceBus.Transports.SQLServer;

#region ConnectionProvider
public static class ConnectionProvider
{
    public const string ReceiverConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=ReceiverCatalog;Integrated Security=True";
    public const string SenderConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SenderCatalog;Integrated Security=True";

    public static ConnectionInfo GetConnection(string transportAddress)
    {
        var connectionString = GetConnectionString(transportAddress);
        return ConnectionInfo.Create()
                .UseConnectionString(connectionString);
    }

    static string GetConnectionString(string transportAddress)
    {
        if (transportAddress.StartsWith("Samples.SqlServer.MultiInstanceSender"))
        {
            return SenderConnectionString;
        }
        return ReceiverConnectionString;
    }
}
#endregion
