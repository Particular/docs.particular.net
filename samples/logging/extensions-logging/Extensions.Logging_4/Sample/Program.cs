using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NServiceBus;
using NServiceBus.Extensions.Logging;

Console.Title = "ExtensionsLogging";

var host = Host.CreateDefaultBuilder(args)
    .UseConsoleLifetime()
    .UseNServiceBus(_ =>
    {
        #region NLogConfiguration
        var config = new LoggingConfiguration();

        var consoleTarget = new ColoredConsoleTarget
        {
            Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
        };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Debug, consoleTarget));

        NLog.LogManager.Configuration = config;
        #endregion

        #region MicrosoftExtensionsLoggingNLog
        Microsoft.Extensions.Logging.ILoggerFactory extensionsLoggerFactory = new NLogLoggerFactory();

        NServiceBus.Logging.ILoggerFactory nservicebusLoggerFactory =
            new ExtensionsLoggerFactory(loggerFactory: extensionsLoggerFactory);

        NServiceBus.Logging.LogManager.UseFactory(loggerFactory: nservicebusLoggerFactory);
        #endregion

        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.ExtensionsLogging");

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        return endpointConfiguration;
    })
    .Build();

await host.StartAsync();

var endpointInstance = host.Services.GetRequiredService<IMessageSession>();

await endpointInstance.SendLocal(new MyMessage());

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();