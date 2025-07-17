using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                Console.Title = "Receiver";
            }).UseNServiceBus(x =>
            {
                var endpointConfiguration = new EndpointConfiguration("Samples.Callbacks.Receiver");
                endpointConfiguration.UsePersistence<LearningPersistence>();
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();
                endpointConfiguration.UseTransport(new LearningTransport());
                endpointConfiguration.EnableCallbacks(makesRequests: false);

                return endpointConfiguration;
            });

}
