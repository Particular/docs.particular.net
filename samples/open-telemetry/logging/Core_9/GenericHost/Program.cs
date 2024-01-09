using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Threading.Tasks;

internal class Program
{
    const string EndpointName = "Samples.Hosting.GenericHost";

    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        #region otel-config

        builder.ConfigureServices(services =>
        {
            services.AddOpenTelemetry()
                .ConfigureResource(resourceBuilder => resourceBuilder.AddService(EndpointName))
                .WithTracing(builder => builder.AddConsoleExporter());
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

            logging.AddConsole();
        });

        #endregion

        #region otel-nsb-config
        builder.UseNServiceBus(ctx =>
        {
            var endpointConfiguration = new EndpointConfiguration(EndpointName);
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            endpointConfiguration.UseTransport(new LearningTransport());

            endpointConfiguration.EnableOpenTelemetry();

            return endpointConfiguration;
        });
        #endregion

        return builder.ConfigureServices(services => services.AddHostedService<MessageGenerator>());
    }
}
