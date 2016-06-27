using System;
using System.ServiceProcess;
using Autofac;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;
using NServiceBus.Log4Net;
using NServiceBus.Logging;
using NServiceBus.Persistence;

class ProgramService : ServiceBase
{
    IBus bus;
    static ILog log = LogManager.GetLogger("ProgramService");

    static void Main()
    {
        Console.Title = "Samples.FirstEndpoint";
        using (var service = new ProgramService())
        {
            if (Environment.UserInteractive)
            {
                service.OnStart(null);

                Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
                Console.ReadKey();

                service.OnStop();

                return;
            }
            Run(service);
        }
    }

    protected override void OnStart(string[] args)
    {
        #region logging
        var layout = new PatternLayout
        {
            ConversionPattern = "%d %-5p %c - %m%n"
        };
        layout.ActivateOptions();
        var appender = new ConsoleAppender
        {
            Layout = layout,
            Threshold = Level.Info
        };
        appender.ActivateOptions();

        BasicConfigurator.Configure(appender);

        LogManager.Use<Log4NetFactory>();
        #endregion

        #region create-config
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.FirstEndpoint");
        #endregion

        #region container
        var builder = new ContainerBuilder();
        //configure custom services
        //builder.RegisterInstance(new MyService());
        var container = builder.Build();
        busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));
        #endregion

        #region serialization
        busConfiguration.UseSerialization<JsonSerializer>();
        #endregion

        #region transport
        busConfiguration.UseTransport<MsmqTransport>();
        #endregion

        #region persistence
        busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Sagas>();
        busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();
        busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();
        #endregion

        #region critical-errors
        busConfiguration.DefineCriticalErrorAction((errorMessage, exception) =>
        {
            // Log the critical error
            log.Fatal($"CRITICAL: {errorMessage}", exception);

            // Kill the process on a critical error
            var output = $"The following critical error was encountered by NServiceBus:\n{errorMessage}\nNServiceBus is shutting down.";
            Environment.FailFast(output, exception);
        });
        #endregion

        #region start-bus
        busConfiguration.EnableInstallers();
        bus = Bus.Create(busConfiguration).Start();
        #endregion


        var myMessage = new MyMessage();
        bus.SendLocal(myMessage);
    }


    protected override void OnStop()
    {
        #region stop-endpoint

        bus?.Dispose();

        #endregion
    }
}