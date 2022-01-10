using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Sales
{
    using Azure.Monitor.OpenTelemetry.Exporter;
    using Honeycomb.OpenTelemetry;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using OpenTelemetry.Resources;
    using OpenTelemetry.Trace;
    using System.Diagnostics;

    class Program
    {
        static void Main(string[] args)
        {
            var listener = new ActivityListener
            {
                ShouldListenTo = _ => true,
                ActivityStopped = activity =>
                {
                    foreach (var (key, value) in activity.Baggage)
                    {
                        activity.AddTag(key, value);
                    }
                }
            };
            ActivitySource.AddActivityListener(listener);

            CreateHostBuilder(args).Build().Run();
            Console.Title = EndpointName;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseNServiceBus(hostBuilderContext =>
                {
                    var endpointConfiguration = new EndpointConfiguration(EndpointName);

                    var transport = endpointConfiguration.UseTransport<LearningTransport>();

                    var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();

                    return endpointConfiguration;
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddOpenTelemetryTracing(builder => builder
                                                                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(EndpointName))
                                                                .AddNServiceBusInstrumentation() // jimmy's package
                                                                .AddJaegerExporter(c =>
                                                                {
                                                                    c.AgentHost = "localhost";
                                                                    c.AgentPort = 6831;
                                                                })
                                                                .AddAzureMonitorTraceExporter(c => { c.ConnectionString = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY"); })
                                                                .AddHoneycomb(new HoneycombOptions
                                                                {
                                                                    ServiceName = "spike",
                                                                    ApiKey = Environment.GetEnvironmentVariable("HONEYCOMB_APIKEY"),
                                                                    Dataset = "spike"
                                                                })
                    );
                });

        public static string EndpointName => "Sales";
    }
}