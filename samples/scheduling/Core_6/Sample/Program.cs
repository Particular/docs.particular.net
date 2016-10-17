using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static ILog log = LogManager.GetLogger<Program>();

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Scheduling";
        var endpointConfiguration = new EndpointConfiguration("Samples.Scheduling");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        try
        {
            #region Schedule

            // Send a message every 5 seconds
            await endpointInstance.ScheduleEvery(
                timeSpan: TimeSpan.FromSeconds(5),
                task: pipelineContext =>
                {
                    var message = new MyMessage();
                    return pipelineContext.SendLocal(message);
                });

            // Name a schedule task and invoke it every 5 seconds
            await endpointInstance.ScheduleEvery(
                timeSpan: TimeSpan.FromSeconds(5),
                name: "MyCustomTask",
                task: pipelineContext =>
                {
                    log.Info("Custom Task executed");
                    return Task.CompletedTask;
                });

            #endregion

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}