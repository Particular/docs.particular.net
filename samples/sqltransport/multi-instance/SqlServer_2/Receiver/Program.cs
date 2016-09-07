using System;
using NServiceBus;
using NServiceBus.Transports.SQLServer;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceReceiver";

        #region ReceiverConfiguration

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.MultiInstanceReceiver");
        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSpecificConnectionInformation(ConnectionInfoProvider.GetConnection);
        transport.ConnectionString(ConnectionInfoProvider.DefaultConnectionString);
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        #endregion

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.WriteLine("Waiting for Order messages from the Sender");
            Console.ReadKey();
        }
    }

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
}