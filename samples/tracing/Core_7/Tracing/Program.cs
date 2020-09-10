using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Tracing.AppInsights;

namespace Tracing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Samples.Tracing.AppInsights";

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            var host = Host
                .CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureLogging((ctx, logging) =>
                {
                    logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .UseNServiceBus(ctx =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Samples.Tracking.AppInsights");
                    endpointConfiguration.UsePersistence<LearningPersistence>();
                    endpointConfiguration.UseTransport<LearningTransport>();

                    endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

                    endpointConfiguration.EnableFeature<MessageTracing>();

                    endpointConfiguration.PurgeOnStartup(true);

                    return endpointConfiguration;
                }).ConfigureServices(s =>
                {
                    s.AddApplicationInsightsTelemetryWorkerService(o =>
                    {
                        o.InstrumentationKey = GetAppInsightsInstrumentationKey();
                    });
                    s.AddSingleton<ITelemetryModule, NServiceBusInsightsModule>();
                }).Build();

            _ = host.RunAsync();

            var endpointInstance = host.Services.GetRequiredService<IMessageSession>();
            var telemetryClient = host.Services.GetRequiredService<TelemetryClient>();

            try
            {
                Console.WriteLine("Endpoint started. Press 'enter' to send a message");
                Console.WriteLine("Press ESC key to quit");

                while (true)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        break;
                    }

                    using (var operation = telemetryClient.StartOperation<RequestTelemetry>("InitialCommand"))
                    {
                        await endpointInstance.SendLocal(new InitialCommand())
                            .ConfigureAwait(false);
                    };
                }
            }
            finally
            {
                await host.StopAsync()
                    .ConfigureAwait(false);
            }
        }

        static string GetAppInsightsInstrumentationKey()
        {
            var envInstrumentationKey = "ApplicationInsightsKey";
            var instrumentationKey = Environment.GetEnvironmentVariable(envInstrumentationKey);

            if (string.IsNullOrEmpty(instrumentationKey))
            {
                throw new Exception($"Environment variable '{envInstrumentationKey}' required.");
            }

            Console.WriteLine("Using application insights application key: {0}", instrumentationKey);
            return instrumentationKey;
        }

        static async Task OnCriticalError(ICriticalErrorContext context)
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
    }
}