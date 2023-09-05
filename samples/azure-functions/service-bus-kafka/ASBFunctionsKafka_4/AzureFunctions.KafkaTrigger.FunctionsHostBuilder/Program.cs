using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace AzureFunctions.KafkaTrigger.FunctionsHostBuilder;

public class Program
{
    static async Task Main(string[] args)
    {
        // FunctionsAssemblyResolver.RedirectAssembly();

        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(async services =>
            {
                var cfg = new EndpointConfiguration("SendOnly");
                cfg.SendOnly();

                // cfg.AssemblyScanner().ExcludeAssemblies("Google.Protobuf.dll", "Azure.Core.dll");

                var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsServiceBus");
                var transport = new AzureServiceBusTransport(connectionString);
                var routing = cfg.UseTransport(transport);

                routing.RouteToEndpoint(typeof(FollowUp), "Samples.KafkaTrigger.ConsoleEndpoint");

                var endpoint = await Endpoint.Start(cfg);

            })
            .Build();

        await host.RunAsync();
    }
}