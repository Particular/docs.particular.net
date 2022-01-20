namespace Billing
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using Azure.Monitor.OpenTelemetry.Exporter;
    using Honeycomb.OpenTelemetry;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using OpenTelemetry.Logs;
    using OpenTelemetry.Resources;
    using OpenTelemetry.Trace;

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

                    endpointConfiguration.EnableInstallers();


                    if (null != Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString"))
                    {
                        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
                        transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString"));
                    }
                    else
                    {
                        var transport = endpointConfiguration.UseTransport<LearningTransport>();
                    }

                    if (null != Environment.GetEnvironmentVariable("SQLServerConnectionString"))
                    {
                        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                        persistence.ConnectionBuilder(() => new SqlConnection(Environment.GetEnvironmentVariable("SQLServerConnectionString")));
                        persistence.SqlDialect<SqlDialect.MsSqlServer>();
                    }
                    else
                    {
                        var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();
                    }

                    endpointConfiguration.RegisterComponents(
                        c =>
                        {
                            c.AddScoped<OrderCalculator>();
                        }
                    );

                    return endpointConfiguration;
                })
                .ConfigureServices((_, services) =>
                {
                    AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);
                    services.AddLogging(builder =>
                    {
                        builder.AddConsole();
                        builder.AddApplicationInsights(Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY"));
                        builder.AddOpenTelemetry(o => o.AddConsoleExporter());
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
                                                                    ApiKey = Environment.GetEnvironmentVariable("HONEYCOMB_APIKEY"),
                                                                    Dataset = "full-telemetry"
                                                                })
                    );
                    services.AddHostedService<TestService>();
                });
        public static string EndpointName => "Billing";
    }
}
