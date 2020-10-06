using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

partial class Program
{
    static ILog log = LogManager.GetLogger<Program>();

    static async Task Main()
    {
        Console.Title = "Samples.Scheduling.Timer";
        var endpointConfiguration = new EndpointConfiguration("Samples.Scheduling.Timer");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #region ScheduleUsingTimer
        var interval = TimeSpan.FromSeconds(5);

        var timer = new Timer(async state =>
        {
            await endpointInstance.SendLocal(new MyScheduledTask());
            log.Info(nameof(MyScheduledTask) + " scheduled");
        }, null, interval, interval);

        #endregion

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}