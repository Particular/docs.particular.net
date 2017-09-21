using System;
using System.Threading.Tasks;
using FluentScheduler;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.FluentScheduler.Scheduler";
        var endpointConfiguration = new EndpointConfiguration("Samples.FluentScheduler.Scheduler");
        endpointConfiguration.UseTransport<LearningTransport>();

        #region logging

        var logger = LogManager.GetLogger(typeof(JobManager).FullName);
        JobManager.JobException += info =>
        {
            logger.Error($"Error occurred in job: {info.Name}", info.Exception);
        };
        JobManager.JobStart += info =>
        {
            logger.Info($"Start job: {info.Name}. Duration: {info.StartTime}");
        };
        JobManager.JobEnd += info =>
        {
            logger.Info($"End job: {info.Name}. Duration: {info.Duration}. NextRun: {info.NextRun}.");
        };

        #endregion

        #region Configuration

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        JobManager.AddJob(
            new SendMessageJob(endpointInstance),
            schedule =>
            {
                schedule
                    .ToRunNow()
                    .AndEvery(3).Seconds();
            });

        #endregion

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        #region shutdown

        JobManager.StopAndBlock();
        await endpointInstance.Stop()
            .ConfigureAwait(false);

        #endregion
    }
}