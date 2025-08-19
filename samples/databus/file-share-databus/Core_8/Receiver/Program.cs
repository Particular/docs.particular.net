using NServiceBus;
using Shared;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");
        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
        var storagePath = new SolutionDirectoryFinder().GetDirectory("storage");
        dataBus.BasePath(storagePath);
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<XmlSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
