using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System.Threading.Tasks;

public static class Program
{
    public static Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .UseNServiceBus(context =>
            {
                var endpointConfiguration = new EndpointConfiguration("Samples.Callbacks.WebSender");
                endpointConfiguration.UseTransport<LearningTransport>();

                endpointConfiguration.MakeInstanceUniquelyAddressable("1");
                endpointConfiguration.EnableCallbacks();

                return endpointConfiguration;
            })
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
            .Build();

        return host.RunAsync();
    }
}
