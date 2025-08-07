using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        #region logging
        services.AddLogging(x =>
        {
            x.SetMinimumLevel(LogLevel.Critical);
        });
        #endregion

    })
    .UseConsoleLifetime()
    .UseNServiceBus(context =>
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


var messageSession = host.Services.GetRequiredService<IMessageSession>();

var message = new MyMessage
{
    Property = "PropertyValue"
};

await messageSession.SendLocal(message);

Console.WriteLine("Press Ctrl+C to exit");
await host.RunAsync();