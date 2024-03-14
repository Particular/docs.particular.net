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
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        await endpointInstance.SendLocal(new LongRunningMessage { DataId = Guid.NewGuid() });

        Console.ReadKey();

        Console.WriteLine("Giving the endpoint 1 second to gracefully stop before sending a cancel signal to the cancellation token");

        #region StoppingEndpointWithCancellationToken
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
        await endpointInstance.Stop(tokenSource.Token);
        #endregion
    }
}
