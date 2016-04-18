using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transports.SQLServer;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SqlServer.StoreAndForwardReceiver";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SqlServer.StoreAndForwardReceiver");

        #region ReceiverConfiguration

        busConfiguration.UseTransport<SqlServerTransport>()
            .UseSpecificConnectionInformation(
                EndpointConnectionInfo.For("Samples.SqlServer.StoreAndForwardSender")
                    .UseConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=sender;Integrated Security=True"));

        busConfiguration.UsePersistence<NHibernatePersistence>();

        #endregion

        busConfiguration.DisableFeature<SecondLevelRetries>();

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}