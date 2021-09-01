using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Cooperative.Cancellation";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);
        var endpointConfiguration = new EndpointConfiguration("Samples.Cooperative.Cancellation");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport(new LearningTransport());

        #region StartingEndpointWithCancellationToken
        var tokenSource = new CancellationTokenSource();

        var endpointInstance = await Endpoint.Start(endpointConfiguration, tokenSource.Token)
            .ConfigureAwait(false);
        #endregion

        Console.WriteLine("Endpoint started with cancellation token");

        await endpointInstance.SendLocal(new LongRunningMessage { DataId = Guid.NewGuid() });

        Console.ReadKey();

        Console.WriteLine("Sending the Cancel signal to the cancellation token");

        #region StoppingEndpointWithCancellationToken
        tokenSource.Cancel();
        #endregion
    }
}