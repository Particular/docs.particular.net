using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "DispatchNotification";

        #region endpoint-configuration
        var endpointConfiguration = new EndpointConfiguration("Samples.DispatchNotification");
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.NotifyDispatch(new SampleDispatchNotifier());
        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to send a message");
        Console.WriteLine("Press Escape to exit");

        while (Console.ReadKey(true).Key != ConsoleKey.Escape)
        {
            await endpoint.SendLocal(new SomeMessage());
        }

        await endpoint.Stop();
    }
}
