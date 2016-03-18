using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transports.SQLServer;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.SQLOutboxStoreAndForward.Receiver";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.SQLOutboxStoreAndForward.Receiver");

        #region ReceiverConfiguration

        busConfiguration.UseTransport<SqlServerTransport>()
            .UseSpecificConnectionInformation(
                EndpointConnectionInfo.For("Samples.SQLOutboxStoreAndForward.Sender")
                    .UseConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=sender;Integrated Security=True"));

        busConfiguration.UsePersistence<NHibernatePersistence>();
        busConfiguration.EnableOutbox();

        #endregion

        busConfiguration.DisableFeature<SecondLevelRetries>();

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}