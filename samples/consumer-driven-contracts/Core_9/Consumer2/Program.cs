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
                Console.Title = "Consumer2";
            }).UseNServiceBus(x =>
            {
                var endpointConfiguration = new EndpointConfiguration("Samples.ConsumerDrivenContracts.Consumer2");
                var transport = endpointConfiguration.UseTransport(new LearningTransport());
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();

                endpointConfiguration.SendFailedMessagesTo("error");
                endpointConfiguration.EnableInstallers();
                return endpointConfiguration;
            });

}