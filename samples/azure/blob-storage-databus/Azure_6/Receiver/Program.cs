using System;
using NServiceBus;
using NServiceBus.DataBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.AzureBlobStorageDataBus.Receiver";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.AzureBlobStorageDataBus.Receiver");
        busConfiguration.UseSerialization<JsonSerializer>();
        var dataBus = busConfiguration.UseDataBus<AzureDataBus>();
        dataBus.ConnectionString("UseDevelopmentStorage=true");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
    }
}