using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

internal class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.UseNServiceBus(ctx =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.Hosting.GenericHost");
            endpointConfiguration.UseTransport(new LearningTransport());

            return endpointConfiguration;
        });

        #region otel-config

        builder.ConfigureServices(services =>
        {
            services.AddOpenTelemetryTracing(config => config
                                                       .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MyEndpointName"))
                                                       .AddSource("NServiceBus.*")
                                                       .AddConsoleExporter());

        });

        #endregion

        #region otel-logging

        builder.ConfigureLogging((ctx, logging) =>
        {
            logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));

            logging.AddOpenTelemetry(loggingOptions =>
            {
                loggingOptions.IncludeFormattedMessage = true;
                loggingOptions.IncludeScopes = true;
                loggingOptions.ParseStateValues = true;
                loggingOptions.AddConsoleExporter();
            });

            logging.AddEventLog();
            logging.AddConsole();
        });

        #endregion

        return builder.ConfigureServices(services => services.AddHostedService<Worker>());
    }
}