using System;
using System.Threading.Tasks;
using AzureFunctions.Messages.NServiceBusMessages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace AzureFunctions.KafkaTrigger.FunctionsHostBuilder;

public class Program
{
    public static async Task Main()
    {
        var host = new HostBuilder()
            .ConfigureServices(async services =>
            {
                var cfg = new EndpointConfiguration("SendOnly");
                cfg.SendOnly();

                var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsServiceBus");
                var transport = new AzureServiceBusTransport(connectionString);
                var routing = cfg.UseTransport(transport);

                routing.RouteToEndpoint(typeof(FollowUp), "Samples.KafkaTrigger.ConsoleEndpoint");

                var endpoint = await Endpoint.Start(cfg);

                services.AddSingleton<IMessageSession>(endpoint);
            })
            .ConfigureFunctionsWorkerDefaults()
            .Build();

        await host.RunAsync();
    }
}