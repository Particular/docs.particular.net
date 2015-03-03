using System;
using NServiceBus;
using NServiceBus.Transports.SQLServer;

namespace Receiver
{
    class Program
    {
        static void Main()
        {
            using (ReceiverDataContext ctx = new ReceiverDataContext())
            {
                ctx.Database.Initialize(true);
            }

            #region ReceiverConfiguration

            BusConfiguration busConfig = new BusConfiguration();
            busConfig.UseTransport<SqlServerTransport>().UseSpecificConnectionInformation(
                EndpointConnectionInfo.For("sender")
                    .UseConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=sender;Integrated Security=True"));

            busConfig.UsePersistence<NHibernatePersistence>();
            busConfig.EnableOutbox();

            #endregion

            using (Bus.Create(busConfig).Start())
            {
                Console.WriteLine("Press <enter> to exit");
                Console.ReadLine();
            }
        }
    }
}