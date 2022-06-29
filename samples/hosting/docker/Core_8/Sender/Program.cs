﻿using Microsoft.Extensions.DependencyInjection;
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

                    var routing = endpointConfiguration.UseTransport(new RabbitMQTransport(Topology.Conventional, "host=rabbitmq"));

                    routing.RouteToEndpoint(typeof(RequestMessage), "Samples.Docker.Receiver");

                    endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
                    endpointConfiguration.EnableInstallers();
                    endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);
                    return endpointConfiguration;
                })
                .ConfigureServices(services => services.AddHostedService<MessageSender>());
        }
    }
}