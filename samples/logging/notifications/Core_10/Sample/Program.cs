using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;


var host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(x =>
    {
        #region endpointConfig

        var endpointConfiguration = new EndpointConfiguration("Samples.Notifications");
        SubscribeToNotifications.Subscribe(endpointConfiguration);

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        #region customDelayedRetries

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Delayed(
            customizations: delayed =>
            {
                delayed.TimeIncrease(TimeSpan.FromSeconds(1));
            });

        #endregion

        return endpointConfiguration;
    }).Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

var message = new MyMessage
{
    Property = "PropertyValue"
};

await messageSession.SendLocal(message);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();