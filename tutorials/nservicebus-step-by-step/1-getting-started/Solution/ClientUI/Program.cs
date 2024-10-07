using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace ClientUI;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("ClientUI");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var routing = endpointConfiguration.UseTransport(new LearningTransport());

        builder.UseNServiceBus(endpointConfiguration);

        await builder.Build().RunAsync();
    }
}