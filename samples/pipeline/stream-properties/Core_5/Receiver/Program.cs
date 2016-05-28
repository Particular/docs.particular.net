using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.PipelineStream.Receiver";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PipelineStream.Receiver");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SetStreamStorageLocation("..\\..\\..\\storage");
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}