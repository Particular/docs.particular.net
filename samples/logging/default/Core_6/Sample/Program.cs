using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.Logging.Default";
        #region ConfigureLogging
        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.Default");
        // No config is required in version 5 and
        // higher since logging is enabled by default
        #endregion
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
