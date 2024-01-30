using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace Receiver
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            await ProceedIfBrokerIsAlive.WaitForBroker("rabbitmq");

            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .UseNServiceBus(ctx =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Receiver");
                    #region TransportConfiguration

                    var connectionString = "host=rabbitmq";
                    var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);
                    endpointConfiguration.UseTransport(transport);

                    #endregion

                    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
                    endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);
                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                });
        }
    }
}