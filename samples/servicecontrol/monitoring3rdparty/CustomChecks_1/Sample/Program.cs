using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.CustomChecks.Monitor3rdParty";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomChecks.Monitor3rdParty");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        busConfiguration.ReportCustomChecksTo("Particular.ServiceControl");

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}