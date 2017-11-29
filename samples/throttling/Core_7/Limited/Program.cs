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

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region RegisterBehavior

        endpointConfiguration.Pipeline.Register<ThrottlingRegistration>();

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}