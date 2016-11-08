namespace Subscriber2
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using Subscriber2.Contracts;

    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Samples.MultipleInheritance.Subscriber1";
            LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
            var endpointConfiguration = new EndpointConfiguration("Samples.MultipleInheritance.Subscriber2");
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();

            transport.Routing().RegisterPublisher(typeof(Subscriber2Event), "Samples.MultipleInheritance.Publisher");


            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");

            Console.ReadKey();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        class Subscriber1EventHandler : IHandleMessages<Subscriber2Event>
        {
            public Task Handle(Subscriber2Event message, IMessageHandlerContext context)
            {
                return Console.Out.WriteLineAsync(message.Subscriber2Property);
            }
        }
    }
}