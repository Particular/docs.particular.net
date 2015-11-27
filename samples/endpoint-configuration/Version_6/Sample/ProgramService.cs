using System;
using System.ServiceProcess;
using System.Threading.Tasks;
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
    IEndpointInstance endpoint;
    static ILog logger = LogManager.GetLogger("ProgramService");

    static void Main()
    {
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
        AsyncOnStart().GetAwaiter().GetResult();
    }

    async Task AsyncOnStart()
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

        LogManager.Use<Log4NetFactory>();

        #endregion

        #region create-config

        BusConfiguration busConfiguration = new BusConfiguration();

        #endregion

        #region endpoint-name

        busConfiguration.EndpointName("Samples.FirstEndpoint");

        #endregion

        #region container

        ContainerBuilder builder = new ContainerBuilder();
        //configure your custom services
        //builder.RegisterInstance(new MyService());
        IContainer container = builder.Build();
        busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));

        #endregion

        #region serialization

        busConfiguration.UseSerialization<JsonSerializer>();

        #endregion
        #region error
        busConfiguration.SendFailedMessagesTo("error");
        #endregion

        #region transport

        busConfiguration.UseTransport<MsmqTransport>();

        #endregion

        #region sagas 

        //Not required since Sagas are enabled by default in Version 5

        #endregion

        #region persistence

        busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Sagas>();
        busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();
        busConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();

        #endregion

        #region critical-errors
        busConfiguration.DefineCriticalErrorAction((endpointInstance, errorMessage, exception) =>
        {
            // Log the critical error
            logger.Fatal(string.Format("CRITICAL: {0}", errorMessage), exception);

            // Kill the process on a critical error
            string output = string.Format("The following critical error was encountered by NServiceBus:\n{0}\nNServiceBus is shutting down.", errorMessage);
            Environment.FailFast(output, exception);
            return Task.FromResult(0);
        });

        #endregion

        #region start-bus

        busConfiguration.EnableInstallers();
        endpoint = await Endpoint.Start(busConfiguration);
        #endregion
    }


    protected override void OnStop()
    {
        #region stop-endpoint
        if (endpoint != null)
        {
            endpoint.Stop().GetAwaiter().GetResult();
        }
        #endregion
    }

}