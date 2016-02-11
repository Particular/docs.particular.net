using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        EndpointConfiguration busConfiguration = new EndpointConfiguration();
        busConfiguration.EndpointName("Samples.Scaleout.Slave1");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        Run(busConfiguration).GetAwaiter().GetResult();
    }

    private static async Task Run(EndpointConfiguration busConfiguration)
    {
        var endpoint = await Endpoint.Start(busConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }
}