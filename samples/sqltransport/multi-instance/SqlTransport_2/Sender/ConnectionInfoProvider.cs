using NServiceBus.Transports.SQLServer;

#region SenderConnectionProvider

static class ConnectionInfoProvider
{
    const string ReceiverConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceReceiver;Integrated Security=True";
    public const string DefaultConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceSender;Integrated Security=True";

    public static ConnectionInfo GetConnection(string transportAddress)
    {
        string connectionString;
        if (transportAddress.StartsWith("Samples.SqlServer.MultiInstanceReceiver"))
        {
            connectionString = ReceiverConnectionString;
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

#endregion