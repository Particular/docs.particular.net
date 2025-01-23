namespace ClientUI;

using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(cfg => cfg.SetMinimumLevel(LogLevel.Warning))
            .ConfigureServices((_, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.SetupTransport(args);
                    x.AddConsumers(Assembly.GetExecutingAssembly());
                });

                services.AddSingleton<SimulatedCustomers>();
                services.AddSingleton<ConsoleBackgroundService>();
                
                services.AddHostedService(p => p.GetRequiredService<SimulatedCustomers>());
                services.AddHostedService(p => p.GetRequiredService<ConsoleBackgroundService>());
            });

        return host;
    }

    static async Task Main(string[] args)
    {
        Console.Title = "Load (ClientUI)";

        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }
}