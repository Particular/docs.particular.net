using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.SenderSideScaleOut.Client";
        var endpointConfiguration = new EndpointConfiguration("Samples.SenderSideScaleOut.Client");
        endpointConfiguration.UsePersistence<NonDurablePersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        var routing = endpointConfiguration.UseTransport(new MsmqTransport());

        #region Logical-Routing

        routing.RouteToEndpoint(
            messageType: typeof(DoSomething),
            destination: "Samples.SenderSideScaleOut.Server");

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
            var command = new DoSomething { SequenceId = ++sequenceId };
            await endpointInstance.Send(command)
                .ConfigureAwait(false);
            Console.WriteLine($"Message {command.SequenceId} Sent");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
