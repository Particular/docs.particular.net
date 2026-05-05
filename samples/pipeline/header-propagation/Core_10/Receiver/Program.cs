using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

public static class Program
{
    public static async Task Main()
    {
        Console.Title = "Receiver";

        var endpointConfiguration = new EndpointConfiguration("Receiver");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region register-behavior
        endpointConfiguration.Pipeline.Register(
            new PropagateCustomerIdHeaderBehavior(),
            "Copies CustomerId header from incoming to outgoing messages"
        );
        #endregion

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        var host = builder.Build();
        await host.StartAsync();

        Console.WriteLine("Press [ESC] to quit.");

        while (Console.ReadKey(true).Key != ConsoleKey.Escape)
        {
        }

        await host.StopAsync();
    }
}


