using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.CustomChecks.Monitor3rdParty");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}