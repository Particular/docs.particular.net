﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static async Task Main()
    {
        Console.Title = "Log4NetCustom";
        #region ConfigureLog4Net
        var layout = new PatternLayout
        {
            ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n"
        };
        layout.ActivateOptions();
        var consoleAppender = new ConsoleAppender
        {
            Threshold = Level.Info,
            Layout = layout
        };
        consoleAppender.ActivateOptions();
        var executingAssembly = Assembly.GetExecutingAssembly();
        var repository = log4net.LogManager.GetRepository(executingAssembly);
        BasicConfigurator.Configure(repository, consoleAppender);
        #endregion

        #pragma warning disable CS0618 // Type or member is obsolete
        #region UseConfig

        LogManager.Use<Log4NetFactory>();

        // Then continue with the endpoint configuration
        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.Log4NetCustom");

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