using System;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Microsoft.Extensions.Hosting;

Console.Title = "LoggingDefault";

var host = Host.CreateDefaultBuilder(args)
    .UseConsoleLifetime()
    .UseNServiceBus(_ =>
    {
        #region ConfigureLogging
        var defaultFactory = NServiceBus.Logging.LogManager.Use<NServiceBus.Logging.DefaultFactory>();

        // Log directory — current folder for this example
        defaultFactory.Directory(".");

        // Default log level is Info
        defaultFactory.Level(NServiceBus.Logging.LogLevel.Debug);

        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.Default");
        #endregion

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();

        return endpointConfiguration;
    })
    .Build();

await host.StartAsync();

var endpointInstance = host.Services.GetRequiredService<IMessageSession>();

var myMessage = new MyMessage();
await endpointInstance.SendLocal(myMessage);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();