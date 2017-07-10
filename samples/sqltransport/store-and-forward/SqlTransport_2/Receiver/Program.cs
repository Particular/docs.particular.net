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
        var connectionInfo = EndpointConnectionInfo.For("Samples.SqlServer.StoreAndForwardSender")
            .UseConnectionString(@"Data Source=.\SqlExpress;Database=sender;Integrated Security=True");
        transport.UseSpecificConnectionInformation(connectionInfo);

        busConfiguration.UsePersistence<InMemoryPersistence>();

        #endregion

        busConfiguration.DisableFeature<SecondLevelRetries>();

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}