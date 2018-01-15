using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using Serilog;

class Program
{
    static async Task Main()
    {
        #region serilog

        Log.Logger = new LoggerConfiguration()
            .WriteTo.ColoredConsole()
            .CreateLogger();
        LogManager.Use<SerilogFactory>();

        #endregion

        Console.Title = "Samples.HangfireScheduler.Scheduler";
        var endpointConfiguration = new EndpointConfiguration("Samples.HangfireScheduler.Scheduler");
        endpointConfiguration.UseTransport<LearningTransport>();

        #region Configuration

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        // store the endpointInstance in a static helper class
        EndpointHelper.Instance = endpointInstance;

        // use in memory storage. Production should use more robust alternatives:
        // SqlServer, Msmq, Redis etc
        GlobalConfiguration.Configuration.UseMemoryStorage();

        // create and start scheduler instance
        var scheduler = new BackgroundJobServer();

        #endregion

        #region scheduleJob

        // Tell Hangfire to schedule the job and trigger every minute
        RecurringJob.AddOrUpdate("myJob", () => SendMessageJob.Run(), Cron.Minutely);

        #endregion

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        #region shutdown

        scheduler.Dispose();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
        Log.CloseAndFlush();

        #endregion
    }
}