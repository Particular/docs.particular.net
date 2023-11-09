using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace ClientHub;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.NearRealTimeClients.ClientHub";

        var builder = WebApplication.CreateBuilder();

        builder.Host.UseNServiceBus(hostBuilderContext =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.NearRealTimeClients.ClientHub");
            endpointConfiguration.UsePersistence<NonDurablePersistence>();

            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            endpointConfiguration.UseTransport(new LearningTransport());

            endpointConfiguration.SendFailedMessagesTo("error");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningEventsAs(type => type == typeof(StockTick));

            return endpointConfiguration;
        });

        builder.Services.AddSignalR();

        var app = builder.Build();
        app.MapHub<StockTicksHub>("/StockTicksHub");

        var url = "http://localhost:9756/";

        var webAppTask = app.RunAsync(url);

        Console.WriteLine($"SignalR server running at {url}");
        Console.WriteLine("NServiceBus subscriber running");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey(true);

        await Task.WhenAll(
            app.StopAsync(),
            webAppTask);
    }
}