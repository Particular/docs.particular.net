using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.CustomDistributionStrategy.Client";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomDistributionStrategy.Client");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        #region Logical-Routing

        routing.RouteToEndpoint(typeof(DoSomething), "Samples.CustomDistributionStrategy.Server");
        // Distribute all messages using weighted algorithm
        routing.SetMessageDistributionStrategy(
            endpointName: "Samples.CustomDistributionStrategy.Server",
            distributionStrategy: new WeightedDistributionStrategy());

        #endregion

        #region File-Based-Routing

        var instanceMappingFile = routing.InstanceMappingFile();
        instanceMappingFile.FilePath("routes.xml");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");

        var sequenceId = 0;
        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            var command = new DoSomething
            {
                SequenceId = ++sequenceId
            };
            await endpointInstance.Send(command)
                .ConfigureAwait(false);
            Console.WriteLine($"Message {command.SequenceId} Sent");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}