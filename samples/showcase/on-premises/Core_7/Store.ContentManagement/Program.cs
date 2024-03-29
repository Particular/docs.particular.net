using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "ContentManagement";
        var endpointConfiguration = new EndpointConfiguration("Store.ContentManagement");
        endpointConfiguration.ApplyCommonConfiguration(transport =>
        {
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(Store.Messages.RequestResponse.ProvisionDownloadRequest), "Store.Operations");
        });

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
