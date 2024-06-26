using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

class Program
{
    static Task Main(string[] args)
    {
        Console.Title = "Scheduler";

        #region ConfigureHost
        var host = new HostBuilder()
            .ConfigureLogging(logging => logging.AddConsole())
            .UseNServiceBus(_ =>
            {
                var endpointConfig = new EndpointConfiguration("Scheduler");
                endpointConfig.UseTransport(new LearningTransport());
                endpointConfig.UseSerialization<SystemJsonSerializer>();

                return endpointConfig;
            })
            .ConfigureServices(services => services.AddHostedService<SendMessageJob>())
            .Build();
        #endregion

        return host.RunAsync();
    }
}
