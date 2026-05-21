using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        var host = builder.Build();
        var messageSession = host.Services.GetRequiredService<IMessageSession>();
        await host.StartAsync();

        // store the endpointInstance in a static helper class
        EndpointHelper.MessageSession = messageSession;

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
        await host.StopAsync();

        #endregion
    }
}