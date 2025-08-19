using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Scheduler";
        var endpointConfiguration = new EndpointConfiguration("Samples.HangfireScheduler.Scheduler");
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        #region Configuration

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        // store the endpointInstance in a static helper class
        EndpointHelper.Instance = endpointInstance;

        // use in memory storage. Production should use more robust alternatives:
        // SqlServer, Redis etc
        GlobalConfiguration.Configuration.UseMemoryStorage();

        GlobalConfiguration.Configuration.UseColouredConsoleLogProvider();

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
        await endpointInstance.Stop();

        #endregion
    }
}