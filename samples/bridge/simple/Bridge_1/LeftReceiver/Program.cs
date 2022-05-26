using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Bridge.LeftReceiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.Bridge.LeftReceiver");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport(new LearningTransport());

        endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Name == "OrderResponse");
        endpointConfiguration.Conventions().DefiningEventsAs(t => t.Name == "OrderReceived");

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}