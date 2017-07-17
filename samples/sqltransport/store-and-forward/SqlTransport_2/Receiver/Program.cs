using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transports.SQLServer;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.StoreAndForwardReceiver";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.StoreAndForwardReceiver");

        #region ReceiverConfiguration

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        var receiver = @"Data Source=.\SqlExpress;Database=NsbSamplesStoreAndForwardReceiver;Integrated Security=True";
        transport.ConnectionString(receiver);
        var sender = @"Data Source=.\SqlExpress;Database=NsbSamplesStoreAndForwardSender;Integrated Security=True";
        var connectionInfo = EndpointConnectionInfo.For("Samples.SqlServer.StoreAndForwardSender")
            .UseConnectionString(sender);
        transport.UseSpecificConnectionInformation(connectionInfo);

        busConfiguration.UsePersistence<InMemoryPersistence>();

        #endregion

        busConfiguration.DisableFeature<SecondLevelRetries>();
        SqlHelper.EnsureDatabaseExists(receiver);

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}