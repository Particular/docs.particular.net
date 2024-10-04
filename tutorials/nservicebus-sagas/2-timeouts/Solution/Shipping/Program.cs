using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Shipping;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Shipping";

        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("Shipping");

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