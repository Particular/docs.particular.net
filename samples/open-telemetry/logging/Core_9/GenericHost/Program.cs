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

    static HostApplicationBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        #region otel-config

        builder.Services.AddOpenTelemetry()
                .ConfigureResource(resourceBuilder => resourceBuilder.AddService(EndpointName))
                .WithTracing(builder => builder.AddConsoleExporter());

        #endregion

        #region otel-logging

        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"))
            .AddOpenTelemetry(loggingOptions =>
            {
                loggingOptions.IncludeFormattedMessage = true;
                loggingOptions.IncludeScopes = true;
                loggingOptions.ParseStateValues = true;
                loggingOptions.AddConsoleExporter();
            })
            .AddConsole();

        #endregion

        #region otel-nsb-config

        var endpointConfiguration = new EndpointConfiguration(EndpointName);
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        endpointConfiguration.EnableOpenTelemetry();

        builder.UseNServiceBus(endpointConfiguration);

        #endregion

        builder.Services.AddHostedService<MessageGenerator>();

        return builder;
    }
}
