using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sales;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Sales";

        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("Sales");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.UsePersistence<LearningPersistence>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");

        // So that when we test recoverability, we don't have to wait so long
        // for the failed message to be sent to the error queue
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            delayed =>
            {
                delayed.TimeIncrease(TimeSpan.FromSeconds(2));
            }
        );

        builder.UseNServiceBus(endpointConfiguration);

        await builder.Build().RunAsync();
    }
}