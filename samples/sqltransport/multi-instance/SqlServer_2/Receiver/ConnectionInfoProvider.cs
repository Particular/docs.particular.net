using NServiceBus.Transports.SQLServer;

static class ConnectionInfoProvider
{
    public const string DefaultConnectionString = @"Data Source=.\SqlExpress;Database=ReceiverCatalog;Integrated Security=True";
    const string SenderConnectionString = @"Data Source=.\SqlExpress;Database=SenderCatalog;Integrated Security=True";

    public static ConnectionInfo GetConnection(string transportAddress)
    {
        var connectionString = transportAddress.StartsWith("Samples.SqlServer.MultiInstanceSender")
            ? SenderConnectionString
            : DefaultConnectionString;

        return ConnectionInfo
            .Create()
            .UseConnectionString(connectionString);
    }
}