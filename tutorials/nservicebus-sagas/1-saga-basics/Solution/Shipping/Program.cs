using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System.Threading.Tasks;

namespace Shipping
{
    class Program
    {
        static Task Main()
        {
            var builder = Host.CreateApplicationBuilder();

            var endpointConfiguration = new EndpointConfiguration("Shipping");
            endpointConfiguration.EnableOpenTelemetry();

            builder.AddServiceDefaults();

            var connectionString = builder.Configuration.GetConnectionString("transport");
            var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);
            var routing = endpointConfiguration.UseTransport(transport);

            var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            endpointConfiguration.EnableInstallers();

            builder.UseNServiceBus(endpointConfiguration);

            return builder.Build().RunAsync();
        }
    }
}