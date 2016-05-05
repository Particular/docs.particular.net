using System;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using NServiceBus;
using NServiceBus.Installation.Environments;

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
        ConsoleAppender appender = new ConsoleAppender
        {
            Threshold = Level.Info,
            Layout = layout
        };
        // Note that no ActivateOptions is required since NSB 3 does this internally
        #endregion

        #region UseConfig
        //Pass the appenders to NServiceBus
        SetLoggingLibrary.Log4Net(null, appender);

        // Then continue with the bus configuration
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.Logging.Log4NetCustom");
        #endregion


        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }
}