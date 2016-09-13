using NServiceBus.Transports.SQLServer;

#region SenderConnectionProvider

static class ConnectionInfoProvider
{
    const string ReceiverConnectionString = @"Data Source=.\SqlExpress;Database=ReceiverCatalog;Integrated Security=True";
    public const string DefaultConnectionString = @"Data Source=.\SqlExpress;Database=SenderCatalog;Integrated Security=True";

    public static ConnectionInfo GetConnection(string transportAddress)
    {
        var connectionString = transportAddress.StartsWith("Samples.SqlServer.MultiInstanceReceiver")
            ? ReceiverConnectionString
            : DefaultConnectionString;

        return ConnectionInfo
                .Create()
                .UseConnectionString(connectionString);
    }
}

#endregion