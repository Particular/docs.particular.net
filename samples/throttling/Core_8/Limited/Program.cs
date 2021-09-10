using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Throttling.Limited";

        #region LimitConcurrency

        var endpointConfiguration = new EndpointConfiguration("Samples.Throttling.Limited");
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);

        #endregion

        endpointConfiguration.UseTransport(new LearningTransport());

        #region RegisterBehavior

        endpointConfiguration.Pipeline.Register(typeof(ThrottlingBehavior), "API throttling for GitHub");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}