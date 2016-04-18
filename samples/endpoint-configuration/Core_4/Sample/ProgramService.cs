using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation.Environments;
using System.ServiceProcess;
using Autofac;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus.Logging;

class ProgramService : ServiceBase
{
    IBus bus;
    static ILog logger = LogManager.GetLogger("ProgramService");

    static void Main()
    {
        Console.Title = "Samples.FirstEndpoint";
        using (ProgramService service = new ProgramService())
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
        PatternLayout layout = new PatternLayout
        {
            ConversionPattern = "%d %-5p %c - %m%n"
        };
        layout.ActivateOptions();
        ConsoleAppender appender = new ConsoleAppender
        {
            Layout = layout,
            Threshold = Level.Info
        };
        appender.ActivateOptions();

        BasicConfigurator.Configure(appender);

        SetLoggingLibrary.Log4Net();
        #endregion

        #region create-config
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.FirstEndpoint");
        #endregion

        #region container
        ContainerBuilder builder = new ContainerBuilder();
        //configure custom services
        //builder.RegisterInstance(new MyService());
        IContainer container = builder.Build();
        configure.AutofacBuilder(container);
        #endregion

        #region serialization
        Configure.Serialization.Json();
        #endregion

        #region transport
        configure.UseTransport<Msmq>();
        #endregion

        #region sagas
        Configure.Features.Enable<Sagas>();
        #endregion

        #region persistence
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        #endregion

        #region critical-errors
        Configure.Instance.DefineCriticalErrorAction((errorMessage, exception) =>
        {
            // Log the critical error
            logger.Fatal(string.Format("CRITICAL: {0}", errorMessage), exception);

            // Kill the process on a critical error
            string output = string.Format("The following critical error was encountered by NServiceBus:\n{0}\nNServiceBus is shutting down.", errorMessage);
            Environment.FailFast(output, exception);
        });
        #endregion

        #region start-bus
        bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => configure.ForInstallationOn<Windows>().Install());
        #endregion

        bus.SendLocal(new MyMessage());
    }


    protected override void OnStop()
    {
        #region stop-endpoint
        if (bus != null)
        {
            IDisposable disposable = (IDisposable) bus;
            disposable.Dispose();
        }
        #endregion
    }

}