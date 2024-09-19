using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Billing
{
    class Program
    {
        static Task Main()
        {
            var builder = Host.CreateApplicationBuilder();

            builder.AddServiceDefaults();

            var endpointConfiguration = new EndpointConfiguration("Billing");
            endpointConfiguration.EnableOpenTelemetry();

            var connectionString = builder.Configuration.GetConnectionString("transport");
            var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);
            var routing = endpointConfiguration.UseTransport(transport);

            endpointConfiguration.EnableInstallers();

            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            builder.UseNServiceBus(endpointConfiguration);

            return builder.Build().RunAsync();
        }
    }
}