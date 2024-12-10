using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Operations";
        var endpointConfiguration = new EndpointConfiguration("Store.Operations");
        endpointConfiguration.ApplyCommonConfiguration();
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
