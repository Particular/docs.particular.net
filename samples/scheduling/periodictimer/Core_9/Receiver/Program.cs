using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    static Task Main(string[] args)
    {
        Console.Title = "Receiver";

        var host = new HostBuilder()
            .ConfigureLogging(logging => logging.AddConsole())
            .UseNServiceBus(_ =>
            {
                var endpointConfig = new EndpointConfiguration("Receiver");
                endpointConfig.UseTransport(new LearningTransport());
                endpointConfig.UseSerialization<SystemJsonSerializer>();
                return endpointConfig;
            })
            .Build();

        return host.RunAsync();
    }
}
