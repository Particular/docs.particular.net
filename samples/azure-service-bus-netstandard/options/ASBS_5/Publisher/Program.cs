using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;
using Shared;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddAzureServiceBusTopology(builder.Configuration);

var endpointConfiguration = new EndpointConfiguration("Samples.AzureServiceBus.Options.Publisher");

#region OptionsLoading
var section = builder.Configuration.GetSection("AzureServiceBus");
var topologyOptions = section.GetSection("Topology").Get<TopologyOptions>()!;
var topology = TopicTopology.FromOptions(topologyOptions);
var transport = new AzureServiceBusTransport(section["ConnectionString"]!, topology)
{
    Topology =
    {
        // Validation is already done by the generic host so we can disable in the transport
        OptionsValidator = new TopologyOptionsDisableValidationValidator()
    }
};
endpointConfiguration.UseTransport(transport);
#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Publishing messages... Press [Ctrl] + [C] to cancel");

using (var cts = new CancellationTokenSource())
{
    Console.CancelKeyPress += (s, e) =>
    {
        Console.WriteLine("Cancellation Requested...");
        cts.Cancel();
        e.Cancel = true;
    };

    try
    {
        var number = 0;

        while (true)
        {
            await messageSession.Publish(new EventOne
            {
                Content = $"EventOne {number++}",
                PublishedOnUtc = DateTime.UtcNow
            });

            await Task.Delay(1000, cts.Token);

            await messageSession.Publish(new EventTwo
            {
                Content = $"EventTwo {number}",
                PublishedOnUtc = DateTime.UtcNow
            }, cts.Token);
        }
    }
    catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
    {
        // graceful shutdown
    }
}

await host.StopAsync();