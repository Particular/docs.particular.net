using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

Console.Title = "PipelineFeatureToggle";

var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.PipelineFeatureToggle");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region enable-feature

var toggles = endpointConfiguration.EnableFeatureToggles();
toggles.AddToggle(ctx => ctx.MessageHandler.HandlerType == typeof(Handler2));

#endregion

Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetService<IMessageSession>();
var logger = host.Services.GetService<ILogger<Program>>();
Console.WriteLine("Press 'Enter' to send a Message or 'Escape' to exit");

while (true)
{
    var key = Console.ReadKey();
    if (key.Key == ConsoleKey.Escape)
    {
        break;
    }
    if (key.Key == ConsoleKey.Enter)
    {
        logger.LogInformation("Message sent");
        var message = new Message();
        await messageSession.SendLocal(message);
    }
}

await host.StopAsync();