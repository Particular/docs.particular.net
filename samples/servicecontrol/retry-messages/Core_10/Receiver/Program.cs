using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

Console.Title = "Receiver";

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("RetryFailedMessages.Receiver");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

#region DisableRetries

var recoverability = endpointConfiguration.Recoverability();

recoverability.Delayed(
    customizations: retriesSettings =>
    {
        retriesSettings.NumberOfRetries(0);
    });
recoverability.Immediate(
    customizations: retriesSettings =>
    {
        retriesSettings.NumberOfRetries(0);
    });

#endregion

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<Toggle>();

var host = builder.Build();
await host.RunAsync();