using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "ChangeMessageIdentity.Phase1";

        var endpointConfiguration = new EndpointConfiguration("ChangeMessageIdentity.Phase1");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var newOrder = new CreateOrderPhase1
        {
            OrderDate = DateTime.Now
        };
        await endpointInstance.Send("ChangeMessageIdentity.Phase2", newOrder);
        Console.WriteLine("CreateOrder Sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}