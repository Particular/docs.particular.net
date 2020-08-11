using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

internal class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        #region generic-host-service-lifetime

        var builder = Host.CreateDefaultBuilder(args);
        builder.UseWindowsService();

        #endregion

        /*
        #region generic-host-console-lifetime

        var builder = Host.CreateDefaultBuilder(args);
        builder.UseConsoleLifetime();

        #endregion
        */

        #region generic-host-logging

        builder.ConfigureLogging((ctx, logging) =>
        {
            logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));

            logging.AddEventLog();
            logging.AddConsole();
        });

        #endregion

        #region generic-host-nservicebus

        builder.UseNServiceBus(ctx =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.Hosting.GenericHost");
            endpointConfiguration.UseTransport<LearningTransport>();

            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

            return endpointConfiguration;
        });

        #endregion

        #region generic-host-worker-registration

        return builder.ConfigureServices(services => { services.AddHostedService<Worker>(); });

        #endregion
    }

    #region generic-host-critical-error

    private static async Task OnCriticalError(ICriticalErrorContext context)
    {
        var fatalMessage =
            $"The following critical error was encountered:{Environment.NewLine}{context.Error}{Environment.NewLine}Process is shutting down. StackTrace: {Environment.NewLine}{context.Exception.StackTrace}";
        EventLog.WriteEntry(".NET Runtime", fatalMessage, EventLogEntryType.Error);

        try
        {
            await context.Stop().ConfigureAwait(false);
        }
        finally
        {
            Environment.FailFast(fatalMessage, context.Exception);
        }
    }

    #endregion
}
