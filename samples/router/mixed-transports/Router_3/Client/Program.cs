using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Router.TrafficMirroring.Client";
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        var random = new Random();
        var endpointConfiguration = new EndpointConfiguration("Samples.Router.TrafficMirroring.Client");

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString")); //PROD
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.Pipeline.Register(new DivertBehavior(), "Diverts test traffic to a separate namespace via a router");

        transport.Routing().RouteToEndpoint(typeof(MyMessage), "Samples.Router.TrafficMirroring.Server");
            
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(immediate => immediate.NumberOfRetries(0));
        recoverability.Delayed(delayed => delayed.NumberOfRetries(0));

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press <s> to send a message, <p> to publish and event, <S> to send a test message and <P> to publish a test event");
        while (true)
        {
            var id = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
            var letter = Console.ReadKey().KeyChar;

            if (letter == 's')
            {
                var message = new MyMessage
                {
                    Id = id
                };
                await endpointInstance.Send(message)
                    .ConfigureAwait(false);
            }
            else if (letter == 'S')
            {
                var message = new MyMessage
                {
                    Id = id
                };
                var sendOptions = new SendOptions();
                sendOptions.GetExtensions().Set("Tenant", "test");
                await endpointInstance.Send(message, sendOptions)
                    .ConfigureAwait(false);
            }
            else if (letter == 'p')
            {
                var message = new MyEvent
                {
                    Id = id
                };
                await endpointInstance.Publish(message)
                    .ConfigureAwait(false);
            }
            else if (letter == 'P')
            {
                var message = new MyEvent
                {
                    Id = id
                };
                var sendOptions = new PublishOptions();
                sendOptions.GetExtensions().Set("Tenant", "test");
                await endpointInstance.Publish(message, sendOptions)
                    .ConfigureAwait(false);
            }
        }
    }
}