using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static async Task Main()
    {
        Console.Title = "Samples.FaultTolerance.Server";
        var endpointConfiguration = new EndpointConfiguration("Samples.FaultTolerance.Server");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region disable
        //var recoverability = endpointConfiguration.Recoverability();
        //recoverability.Delayed(settings =>
        //{
        //    settings.NumberOfRetries(0);
        //});
        #endregion


        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
