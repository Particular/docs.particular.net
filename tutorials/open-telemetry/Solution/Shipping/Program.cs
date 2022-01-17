using Messages;
using NServiceBus;
using System;
using System.Data.SqlClient;

namespace Shipping
{
    using Azure.Monitor.OpenTelemetry.Exporter;
    using Honeycomb.OpenTelemetry;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
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
                    var endpointConfiguration = new EndpointConfiguration("Shipping");
                    endpointConfiguration.EnableInstallers();

                    var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
                    transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString"));

                    var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                    persistence.ConnectionBuilder(() => new SqlConnection(Environment.GetEnvironmentVariable("SQLServerConnectionString")));
                    persistence.SqlDialect<SqlDialect.MsSqlServer>();

                    var routing = transport.Routing();
                    routing.RouteToEndpoint(typeof(ShipOrder), "Shipping");
                    routing.RouteToEndpoint(typeof(ShipWithMaple), "Shipping");
                    routing.RouteToEndpoint(typeof(ShipWithAlpine), "Shipping");

                    return endpointConfiguration;
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddLogging(builder =>
                    {
                        builder.AddApplicationInsights(Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY"));
                    });
                    services.AddOpenTelemetryTracing(builder => builder
                                                                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(EndpointName))
                                                                .AddSqlClientInstrumentation()
                                                                .AddSource("NServiceBus")
                                                                .AddSource("Azure.*")
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
                                                                    Dataset = "spike-core"
                                                                })
                    );
                });


        public static string EndpointName => "Shipping";
    }
}