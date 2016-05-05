using System;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;
using NServiceBus.Log4Net;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Logging.Log4NetCustom";
        #region ConfigureLog4Net
        PatternLayout layout = new PatternLayout
        {
            ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n"
        };
        layout.ActivateOptions();
        ConsoleAppender consoleAppender = new ConsoleAppender
        {
            Threshold = Level.Info,
            Layout = layout
        };
        // Note that ActivateOptions is required in NSB 5 and above
        consoleAppender.ActivateOptions();
        BasicConfigurator.Configure(consoleAppender);
        #endregion

        #region UseConfig

        LogManager.Use<Log4NetFactory>();

        // Then continue with the bus configuration
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.Log4NetCustom");

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
