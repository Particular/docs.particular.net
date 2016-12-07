using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.CustomChecks.Monitor3rdParty";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomChecks.Monitor3rdParty");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        endpointConfiguration.CustomCheckPlugin("Particular.ServiceControl");

        Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}