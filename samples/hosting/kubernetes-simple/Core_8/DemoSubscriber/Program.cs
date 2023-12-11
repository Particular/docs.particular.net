using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Demo Subscriber";
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
                var endpointConfiguration = new EndpointConfiguration("KubernetesDemo.Subscriber");
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();

                endpointConfiguration.EnableInstallers();

                var transport = new LearningTransport
                {
                    StorageDirectory = @"transport"
                };
                endpointConfiguration.UseTransport(transport);

                var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();
                persistence.SagaStorageDirectory("sagas");

                endpointConfiguration.Recoverability().Immediate(r => r.NumberOfRetries(0)).Delayed(d => d.NumberOfRetries(0));                

                return endpointConfiguration;
            });
    }
}