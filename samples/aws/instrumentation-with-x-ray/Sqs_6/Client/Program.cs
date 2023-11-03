using Commands;
using OpenTelemetry;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace RootCauseExample.Client;

public class Program
{
    private const string EndpointName = "ClientAPI";

    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        Sdk.SetDefaultTextMapPropagator(new AWSXRayPropagator());
        
        return Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                   .ConfigureServices((_, services) =>
                   {
                       var otlpExporterEndpoint = new Uri("http://localhost:4317");
                       services.AddOpenTelemetry()
                               .ConfigureResource(resourceBuilder => resourceBuilder
                                   .AddService(EndpointName)
                                   .AddTelemetrySdk())
                               .WithTracing(tracingBuilder => tracingBuilder
                                                              .AddSource("NServiceBus.Core")
                                                              .AddAspNetCoreInstrumentation()
                                                              .AddAWSInstrumentation()
                                                              .AddXRayTraceId()
                                                              .AddOtlpExporter(options =>
                                                              {
                                                                  options.Protocol = OtlpExportProtocol.Grpc;
                                                                  options.ExportProcessorType = ExportProcessorType.Simple;
                                                                  options.Endpoint = otlpExporterEndpoint;
                                                              })
                                                              .AddConsoleExporter()
                                                              .AddJaegerExporter(c =>
                                                              {
                                                                  c.AgentHost = "localhost";
                                                                  c.AgentPort = 6831;
                                                              }))
                               .WithMetrics(metricsBuilder => metricsBuilder
                                                              .AddAspNetCoreInstrumentation()
                                                              .AddMeter("NServiceBus.Core")
                                                              .AddOtlpExporter(options =>
                                                              {
                                                                  options.Endpoint = otlpExporterEndpoint;
                                                              }));

                       // correlate traces with logs
                       services.AddLogging(loggingBuilder =>
                           loggingBuilder.AddOpenTelemetry(otelLoggerOptions =>
                           {
                               otelLoggerOptions.IncludeFormattedMessage = true;
                               otelLoggerOptions.IncludeScopes = true;
                               otelLoggerOptions.ParseStateValues = true;
                               otelLoggerOptions.AddConsoleExporter();
                               otelLoggerOptions.AddOtlpExporter(options =>
                               {
                                   options.Endpoint = otlpExporterEndpoint;
                               });
                           }).AddConsole()
                       );
                   })
                   .UseNServiceBus(_ =>
                   {
                       var endpointConfiguration = new EndpointConfiguration(EndpointName);
                       endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
                       endpointConfiguration.UsePersistence<LearningPersistence>();

                       // Ensure environment variables contain AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY and AWS_REGION
                       // Reference: https://docs.particular.net/transports/sqs/#configuration
                       var transport = endpointConfiguration.UseTransport<SqsTransport>();
                       transport.Routing().RouteToEndpoint(typeof(PlaceOrder), "Sales");

                       endpointConfiguration.EnableInstallers();
                       endpointConfiguration.EnableOpenTelemetry();

                       endpointConfiguration.Recoverability().Immediate(immediate => immediate.NumberOfRetries(0));
                       endpointConfiguration.Recoverability().Delayed(delayed => delayed.NumberOfRetries(3));

                       return endpointConfiguration;
                   });
    }
}