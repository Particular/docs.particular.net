using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

internal class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.UseConsoleLifetime();

        builder.UseMicrosoftLogFactoryLogging();
        builder.ConfigureLogging((ctx, logging) =>
        {
            logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));

            logging.AddEventLog();
            logging.AddConsole();
        });

        #region back-end-use-nservicebus

        builder.UseNServiceBus(ctx =>
        {
            var endpointConfiguration = new EndpointConfiguration("Sample.BackEnd");
            endpointConfiguration.UseTransport<LearningTransport>();

            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

            return endpointConfiguration;
        });

        #endregion

        #region back-end-register-service
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<ICalculateStuff, CalculateStuff>();
        });
        #endregion

        return builder;
    }

    private static async Task OnCriticalError(ICriticalErrorContext context)
    {
        var fatalMessage = $"The following critical error was " +
                           $"encountered: {Environment.NewLine}{context.Error}{Environment.NewLine}Process is shutting down. " +
                           $"StackTrace: {Environment.NewLine}{context.Exception.StackTrace}";
        
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
}
