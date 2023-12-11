using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Demo Publisher";
        CreateHostBuilder(args).Build().Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
            })
            .UseNServiceBus(ctx =>
            {
                var endpointConfiguration = new EndpointConfiguration("KubernetesDemo.Publisher");
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();

                endpointConfiguration.EnableInstallers();

                var transport = new LearningTransport
                {
                    StorageDirectory = @"transport"
                };
                endpointConfiguration.UseTransport(transport);
                

                endpointConfiguration.Recoverability().Immediate(r => r.NumberOfRetries(0)).Delayed(d => d.NumberOfRetries(0));

                return endpointConfiguration;
            }).ConfigureServices(services => { services.AddHostedService<PublisherService>(); });
    }
}