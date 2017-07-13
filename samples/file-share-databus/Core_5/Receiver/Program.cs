using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.DataBus.Receiver";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.DataBus.Receiver");
        var dataBus = busConfiguration.UseDataBus<FileShareDataBus>();
        dataBus.BasePath("..\\..\\..\\storage");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}