using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using Quartz;
using Quartz.Impl;
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

        Console.Title = "Samples.QuartzScheduler.Scheduler";
        var endpointConfiguration = new EndpointConfiguration("Samples.QuartzScheduler.Scheduler");
        endpointConfiguration.UseTransport<LearningTransport>();

        #region Configuration

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var schedulerFactory = new StdSchedulerFactory();

        var scheduler = await schedulerFactory.GetScheduler()
            .ConfigureAwait(false);

        // inject the endpointInstance into the scheduler context
        scheduler.SetEndpointInstance(endpointInstance);

        await scheduler.Start()
            .ConfigureAwait(false);
        #endregion

        #region scheduleJob

        // define the job and tie it to the SendMessageJob class
        var job = JobBuilder.Create<SendMessageJob>()
            .WithIdentity("job1", "group1")
            .Build();

        // Trigger the job to run now, and then repeat every 3 seconds
        var trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(
                action: builder =>
                {
                    builder
                        .WithIntervalInSeconds(3)
                        .RepeatForever();
                })
            .Build();

        // Tell quartz to schedule the job using the trigger
        await scheduler.ScheduleJob(job, trigger)
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        #region shutdown

        await scheduler.Shutdown()
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);

        #endregion
    }
}