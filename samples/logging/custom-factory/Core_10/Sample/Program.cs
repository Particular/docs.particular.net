using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Logging;

var host = Host.CreateDefaultBuilder(args)
    .UseConsoleLifetime()
    .UseNServiceBus(_ =>
    {
        #region ConfigureLogging
        var loggerDefinition = LogManager.Use<ConsoleLoggerDefinition>();
        loggerDefinition.Level(LogLevel.Info);
        #endregion
        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.CustomFactory");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();

        return endpointConfiguration;
    })
    .Build();

await host.StartAsync();

var endpointInstance = host.Services.GetRequiredService<IMessageSession>();
await endpointInstance.SendLocal(new MyMessage());

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();