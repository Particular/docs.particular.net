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
        var layout = new PatternLayout
        {
            ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n"
        };
        layout.ActivateOptions();
        var appender = new ConsoleAppender
        {
            Threshold = Level.Info,
            Layout = layout
        };
        // Note that no ActivateOptions is required since NSB 4 does this internally
        #endregion

        #region UseConfig
        //Pass the appenders to NServiceBus
        SetLoggingLibrary.Log4Net(null, appender);

        // Then continue with the bus configuration
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.DefineEndpointName("Samples.Logging.Log4NetCustom");
        #endregion

        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}