﻿using System;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Logging.NLogCustom";

        #region ConfigureNLog

        var config = new LoggingConfiguration();

        var consoleTarget = new ColoredConsoleTarget
        {
            Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
        };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));

        LogManager.Configuration = config;

        #endregion

        #pragma warning disable CS0618 // Type or member is obsolete
        #region UseConfig

        NServiceBus.Logging.LogManager.Use<NLogFactory>();

        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.NLogCustom");

        #endregion
        #pragma warning restore CS0618 // Type or member is obsolete

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}