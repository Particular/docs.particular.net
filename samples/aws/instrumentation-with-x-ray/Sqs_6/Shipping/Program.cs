using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

const string EndpointName = "Shipping";

Sdk.SetDefaultTextMapPropagator(new AWSXRayPropagator());

var host = Host.CreateDefaultBuilder(args)
               .ConfigureServices((builder, services) =>
               {
                   var otlpExporterEndpoint = new Uri("http://localhost:4317");
                   services.AddOpenTelemetry()
                           .ConfigureResource(resourceBuilder => resourceBuilder
                               .AddService(EndpointName)
                               .AddTelemetrySdk())
                           .WithTracing(tracingBuilder => tracingBuilder
                                                          .AddSource("NServiceBus.Core")
                                                          .AddAWSInstrumentation()
                                                          .AddXRayTraceId()
                                                          .AddOtlpExporter(options =>
                                                          {
                                                              options.Endpoint = otlpExporterEndpoint;
                                                          })
                                                          .AddConsoleExporter())
                           .WithMetrics(metricsBuilder => metricsBuilder
                                                          .AddMeter("NServiceBus.Core")
                                                          .AddOtlpExporter(options =>
                                                          {
                                                              options.Endpoint = otlpExporterEndpoint;
                                                          }));

                   //Sdk.SetDefaultTextMapPropagator(new AWSXRayPropagator());

                   // connect traces with logs
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
               .UseNServiceBus(context =>
               {
                   var endpointConfiguration = new EndpointConfiguration(EndpointName);
                   endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
                   endpointConfiguration.UsePersistence<LearningPersistence>();

                   // Ensure environment variables contain AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY and AWS_REGION
                   // Reference: https://docs.particular.net/transports/sqs/#configuration
                   endpointConfiguration.UseTransport<SqsTransport>();
                   endpointConfiguration.EnableInstallers();
                   endpointConfiguration.EnableOpenTelemetry();

                   endpointConfiguration.Recoverability().Immediate(immediate => immediate.NumberOfRetries(0));
                   endpointConfiguration.Recoverability().Delayed(delayed => delayed.NumberOfRetries(3));

                   return endpointConfiguration;
               })
               .Build();

var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
Console.Title = hostEnvironment.ApplicationName;
host.Run();