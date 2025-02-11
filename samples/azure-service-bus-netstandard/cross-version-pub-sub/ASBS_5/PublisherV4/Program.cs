using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace PublisherV4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Publisher 1 running Azure Service Bus transport V4");
            var builder = Host.CreateApplicationBuilder(args);

            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddConsole();

            var endpointConfiguration = new EndpointConfiguration("Publisher1");

            var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString");
            var routing = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString));
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            // Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
            endpointConfiguration.EnableInstallers();

            builder.UseNServiceBus(endpointConfiguration);

            builder.Services.AddHostedService<PublisherWorker>();

            var host = builder.Build();
            await host.RunAsync();
        }
    }
}