using System;
using Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

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
                            var transport = endpointConfiguration.UseTransport<LearningTransport>();

                            var routing = transport.Routing();
                            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

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
