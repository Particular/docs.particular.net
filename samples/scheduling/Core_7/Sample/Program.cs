using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static ILog log = LogManager.GetLogger<Program>();

    static async Task Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Console.Title = "Samples.Scheduling";
        var endpointConfiguration = new EndpointConfiguration("Samples.Scheduling");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        #region Schedule

        // Send a message every 5 seconds
        await endpointInstance.ScheduleEvery(
                timeSpan: TimeSpan.FromSeconds(5),
                task: pipelineContext =>
                {
                    var message = new MyMessage();
                    return pipelineContext.SendLocal(message);
                })
            .ConfigureAwait(false);

        // Name a schedule task and invoke it every 5 seconds
        await endpointInstance.ScheduleEvery(
                timeSpan: TimeSpan.FromSeconds(5),
                name: "MyCustomTask",
                task: pipelineContext =>
                {
                    log.Info("Custom Task executed");
                    return Task.CompletedTask;
                })
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}