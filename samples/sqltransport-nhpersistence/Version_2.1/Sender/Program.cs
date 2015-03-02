using System;
using System.Linq;
using Messages;
using NServiceBus;
using NServiceBus.Transports.SQLServer;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
            var random = new Random();
            var busConfig = new BusConfiguration();

            #region SenderConfiguration
            busConfig.UseTransport<SqlServerTransport>().DefaultSchema("sender")
                .UseSpecificConnectionInformation(EndpointConnectionInfo.For("receiver").UseSchema("receiver"));
            busConfig.UsePersistence<NHibernatePersistence>();
            #endregion

            var bus = Bus.Create(busConfig).Start();
            while (true)
            {
                Console.WriteLine("Press <enter> to send a message");
                Console.ReadLine();

                var orderId = new String(Enumerable.Range(0,4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                bus.Publish(new OrderSubmitted()
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                });
            }
        }
    }
}
