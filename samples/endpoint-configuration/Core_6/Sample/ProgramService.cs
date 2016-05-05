using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using Autofac;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence;

class ProgramService : ServiceBase
{
    IEndpointInstance endpoint;
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

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.FirstEndpoint");

        #endregion

        #region container

        ContainerBuilder builder = new ContainerBuilder();
        //configure custom services
        //builder.RegisterInstance(new MyService());
        IContainer container = builder.Build();
        endpointConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));

        #endregion

        #region serialization

        endpointConfiguration.UseSerialization<JsonSerializer>();

        #endregion
        #region error
        endpointConfiguration.SendFailedMessagesTo("error");
        #endregion

        #region transport

        endpointConfiguration.UseTransport<MsmqTransport>();

        #endregion

        #region persistence

        endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Sagas>();
        endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Subscriptions>();
        endpointConfiguration.UsePersistence<InMemoryPersistence, StorageType.Timeouts>();

        #endregion

        #region critical-errors
        endpointConfiguration.DefineCriticalErrorAction(async context =>
        {
            // Log the critical error
            logger.Fatal($"CRITICAL: {context.Error}", context.Exception);

            await context.Stop();

            // Kill the process on a critical error
            string output = $"The following critical error was encountered by NServiceBus:\n{context.Error}\nNServiceBus is shutting down.";
            Environment.FailFast(output, context.Exception);
        });

        #endregion

        #region start-bus

        endpointConfiguration.EnableInstallers();
        endpoint = await Endpoint.Start(endpointConfiguration);
        #endregion


        await endpoint.SendLocal(new MyMessage());
    }


    protected override void OnStop()
    {
        #region stop-endpoint

        endpoint?.Stop().GetAwaiter().GetResult();

        #endregion
    }
}