using System;
using System.Threading.Tasks;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Backend";
        var endpointConfiguration = new EndpointConfiguration("Samples.Router.UpdateAndPublish.Backend");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        var transport = endpointConfiguration.UseTransport<LearningTransport>();

        #region Routing

        var routerConnector = transport.Routing().ConnectToRouter("Samples.Router.UpdateAndPublish.Router");
        routerConnector.RegisterPublisher(
            eventType: typeof(OrderAccepted),
            publisherEndpointName: "Samples.Router.UpdateAndPublish.Frontend");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}