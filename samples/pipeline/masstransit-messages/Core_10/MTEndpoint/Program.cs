using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MTEndpoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<MessageConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", rabbitConfig =>
                    {
                        rabbitConfig.Username("guest");
                        rabbitConfig.Password("guest");
                    });
                    cfg.ConfigureEndpoints(context);
                });

                x.AddConfigureEndpointsCallback((name, cfg) =>
                {
                    if (cfg is IRabbitMqReceiveEndpointConfigurator rmq)
                        rmq.SetQuorumQueue(3);
                });
            });

            builder.Services.AddHostedService<Worker>();

            builder.Build().Run();
        }
    }
}
