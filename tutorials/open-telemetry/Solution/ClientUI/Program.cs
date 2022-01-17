using System;
using Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System.Data.SqlClient;

namespace ClientUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = EndpointName;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .UseNServiceBus(context =>
                        {
                            var endpointConfiguration = new EndpointConfiguration(EndpointName);
                            endpointConfiguration.EnableInstallers();

                            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
                            transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString"));

                            var routing = transport.Routing();
                            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

                            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                            persistence.ConnectionBuilder(() => new SqlConnection(Environment.GetEnvironmentVariable("SQLServerConnectionString")));
                            persistence.SqlDialect<SqlDialect.MsSqlServer>();

                            endpointConfiguration.SendFailedMessagesTo("error");
                            endpointConfiguration.AuditProcessedMessagesTo("audit");

                            return endpointConfiguration;

                        })
                       .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        });
        }

        public static string EndpointName => "ClientUI";
    }
}
