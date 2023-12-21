using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NServiceBus;

using Shared;

namespace Sender
{
    class Program
    {
        public static void Main(string[] args)
        {
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
                .ConfigureServices(sp => sp.AddSingleton<IHostedService>(new ProceedIfRabbitMqIsAlive("rabbitmq")))
                .UseNServiceBus(ctx =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Sender");

                    var rabbitMqConnectionString = "host=rabbitmq";
                    var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), rabbitMqConnectionString);
                    var routing = endpointConfiguration.UseTransport(transport);

                    routing.RouteToEndpoint(typeof(RequestMessage), "Samples.Docker.Receiver");

                    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
                    endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);

                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                })
                .ConfigureServices(services => services.AddHostedService<MessageSender>());
        }
    }
}