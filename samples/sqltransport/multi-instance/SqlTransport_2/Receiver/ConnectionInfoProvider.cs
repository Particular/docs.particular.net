using NServiceBus.Transports.SQLServer;

static class ConnectionInfoProvider
{
    public const string DefaultConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceReceiver;Integrated Security=True";
    const string SenderConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceSender;Integrated Security=True";

    public static ConnectionInfo GetConnection(string transportAddress)
    {
        string connectionString;
        if (transportAddress.StartsWith("Samples.SqlServer.MultiInstanceSender"))
        {
            connectionString = SenderConnectionString;
        }
        else
        {
            connectionString = DefaultConnectionString;
        }

        return ConnectionInfo
            .Create()
            .UseConnectionString(connectionString);
    }
}