using NServiceBus.Transports.SQLServer;

#region ConnectionProvider
public static class ConnectionProvider
{
    public const string ReceiverConnectionString = @"Data Source=.\SqlExpress;Database=ReceiverCatalog;Integrated Security=True";
    public const string SenderConnectionString = @"Data Source=.\SqlExpress;Database=SenderCatalog;Integrated Security=True";

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
