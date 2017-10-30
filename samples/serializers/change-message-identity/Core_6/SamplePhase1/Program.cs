using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "ChangeMessageIdentity.Phase1";

        var endpointConfiguration = new EndpointConfiguration("ChangeMessageIdentity.Phase1");
        endpointConfiguration.SharedConfig();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var newOrder = new CreateOrderPhase1
        {
            OrderDate = DateTime.Now
        };
        await endpointInstance.Send("ChangeMessageIdentity.Phase2", newOrder)
            .ConfigureAwait(false);
        Console.WriteLine("CreateNewOrder Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}