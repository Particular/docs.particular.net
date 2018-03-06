using System;
using System.Threading.Tasks;
using NServiceBus;
using Store.Messages.RequestResponse;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Store.ContentManagement";
        var endpointConfiguration = new EndpointConfiguration("Store.ContentManagement");
        endpointConfiguration.ApplyCommonConfiguration(
            transport =>
            {
                var routing = transport.Routing();
                routing.RouteToEndpoint(typeof(ProvisionDownloadRequest), "Store.Operations");
            });

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}