using System;
using System.Diagnostics;
using System.Linq;
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
        var builder = Host.CreateDefaultBuilder(args);

        var isService = !(Debugger.IsAttached || args.Contains("--console"));

        builder = isService ? builder.UseWindowsService() : builder.UseConsoleLifetime();

        builder.UseMicrosoftLogFactoryLogging();

        builder.ConfigureLogging((ctx, logging) =>
        {
            logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));

            if (isService)
                logging.AddEventLog();
            else
                logging.AddConsole();
        });

        builder.UseNServiceBus(ctx =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.Hosting.GenericHost");

            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.UsePersistence<LearningPersistence>();

            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

            return endpointConfiguration;
        });

        return builder.ConfigureServices(services => { services.AddHostedService<Worker>(); });
    }

    private static async Task OnCriticalError(ICriticalErrorContext context)
    {
        var fatalMessage =
            $"The following critical error was encountered:{Environment.NewLine}{context.Error}{Environment.NewLine}Process is shutting down. StackTrace: {Environment.NewLine}{context.Exception.StackTrace}";

        EventLog.WriteEntry(".NET Runtime", fatalMessage, EventLogEntryType.Error);

        if (Environment.UserInteractive)
            // so that user can see on their screen the problem
            await Task.Delay(10_000)
                .ConfigureAwait(false);

        Environment.FailFast(fatalMessage, context.Exception);
    }
}