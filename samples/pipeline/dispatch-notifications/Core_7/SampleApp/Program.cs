using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.DispatchNotification";

        #region endpoint-configuration
        var endpointConfiguration = new EndpointConfiguration("Samples.DispatchNotification");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.NotifyDispatch(new SampleDispatchWatcher());
        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to send a message");
        Console.WriteLine("Press Escape to exit");

        while (Console.ReadKey(true).Key != ConsoleKey.Escape)
        {
            await endpoint.SendLocal(new SomeMessage())
                .ConfigureAwait(false);
        }

        await endpoint.Stop()
            .ConfigureAwait(false);
    }
}