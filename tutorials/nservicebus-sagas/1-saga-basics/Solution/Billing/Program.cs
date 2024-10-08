using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Billing;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Billing";

        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("Billing");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");

        // Decrease the default delayed delivery interval so that we don't
        // have to wait too long for the message to be moved to the error queue
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