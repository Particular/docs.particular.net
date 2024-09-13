using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sales
{
    class Program
    {
        static Task Main()
        {
            var builder = Host.CreateApplicationBuilder();

            builder.AddServiceDefaults();

            var endpointConfiguration = new EndpointConfiguration("Sales");
            endpointConfiguration.EnableOpenTelemetry();

            var connectionString = builder.Configuration.GetConnectionString("transport");
            var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);
            var routing = endpointConfiguration.UseTransport(transport);

            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            endpointConfiguration.EnableInstallers();


            builder.UseNServiceBus(endpointConfiguration);

            var host = builder.Build();

            return host.RunAsync();
        }
    }
}