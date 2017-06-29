using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;
using System;
using System.Threading.Tasks;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("Subscriber");

            endpointConfiguration.UseTransport<MsmqTransport>()
                .Routing()
                .RegisterPublisher(typeof(Messages.SomethingHappened), "Publisher");

            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
            Console.ReadKey();

            await endpointInstance.Stop()
                    .ConfigureAwait(false);
        }
    }
}
