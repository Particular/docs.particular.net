using NServiceBus;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.CustomChecks.Monitor3rdParty";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomChecks.Monitor3rdParty");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.ReportCustomChecksTo("Particular.ServiceControl");

        await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}