using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    static Task Main(string[] args)
    {
        Console.Title = "Scheduler";

        #region ConfigureHost

        var builder = Host.CreateApplicationBuilder();

        var endpointConfig = new EndpointConfiguration("Scheduler");
        endpointConfig.UseTransport(new LearningTransport());
        endpointConfig.UseSerialization<SystemJsonSerializer>();

        builder.UseNServiceBus(endpointConfig);

        builder.Services.AddHostedService<SendMessageJob>();

        #endregion

        var host = builder.Build();
        return host.RunAsync();
    }
}
