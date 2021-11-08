using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MTEndpoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
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
                    });

                    services.AddMassTransitHostedService();

                    services.AddHostedService<Worker>();
                });
    }
}
