using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.CustomRouting.Billing";
        var endpointConfiguration = new EndpointConfiguration("Samples.CustomRouting.Billing");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.EnableAutomaticRouting(AutomaticRoutingConst.ConnectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}