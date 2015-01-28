using System;
using NServiceBus;

class Program
{
    static string BasePath = "..\\..\\..\\storage";

    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Sample.DataBus.Receiver");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UseDataBus<FileShareDataBus>().BasePath(BasePath);
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            Console.WriteLine("\r\nPress enter key to stop program\r\n");
            Console.Read();
        }
    }
}