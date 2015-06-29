using System;
using NServiceBus;
using NServiceBus.Transports.SQLServer;
using NServiceBus.Persistence;

class Program
{
    static void Main()
    {
        Console.Title = "Receiver";

        using (ReceiverDataContext ctx = new ReceiverDataContext())
        {
            ctx.Database.Initialize(true);
        }

        #region ReceiverConfiguration

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.UseTransport<SqlServerTransport>().UseSpecificConnectionInformation(
            EndpointConnectionInfo.For("sender")
                .UseConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=sender;Integrated Security=True"));

        busConfiguration.UsePersistence<NHibernatePersistence>().RegisterManagedSessionInTheContainer();
        busConfiguration.EnableOutbox();

        #endregion

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
