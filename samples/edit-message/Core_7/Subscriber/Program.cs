using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.EditMessage.Subscriber";
        var endpointConfiguration = new EndpointConfiguration("Samples.EditMessage.Subscriber");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.DisableFeature<TimeoutManager>();
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}