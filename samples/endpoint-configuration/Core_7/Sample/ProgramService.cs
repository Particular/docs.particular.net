using System;
using System.ComponentModel;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;
using NServiceBus.Logging;

[DesignerCategory("Code")]
class ProgramService :
    ServiceBase
{
    IEndpointInstance endpointInstance;
    static readonly ILog log = LogManager.GetLogger("ProgramService");

    static void Main()
    {
        using (var service = new ProgramService())
        {
            if (ServiceHelper.IsService())
            {
                Run(service);
                return;
            }
            Console.Title = "Samples.FirstEndpoint";
            service.OnStart(null);

            Console.WriteLine("\r\nEndpoint created and configured; press any key to stop program\r\n");
            Console.ReadKey();

            service.OnStop();
        }
    }

    protected override void OnStart(string[] args)
    {
        AsyncOnStart().GetAwaiter().GetResult();
    }

    async Task AsyncOnStart()
    {
        #pragma warning disable CS0618 // Type or member is obsolete
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

        var executingAssembly = Assembly.GetExecutingAssembly();
        var repository = log4net.LogManager.GetRepository(executingAssembly);
        BasicConfigurator.Configure(repository, appender);

        LogManager.Use<Log4NetFactory>();

        #endregion
        #pragma warning restore CS0618 // Type or member is obsolete

        #region create-config

        var endpointConfiguration = new EndpointConfiguration("Samples.FirstEndpoint");

        #endregion

        #region container

        endpointConfiguration.UseContainer(new AutofacServiceProviderFactory(builder =>
        {
            // configure custom services
            // builder.RegisterInstance(new MyService());
        }));

        #endregion

        #region serialization

        endpointConfiguration.UseSerialization<XmlSerializer>();

        #endregion

        #region error

        endpointConfiguration.SendFailedMessagesTo("error");

        #endregion

        #region audit

        endpointConfiguration.AuditProcessedMessagesTo("audit");

        #endregion

        #region transport

        endpointConfiguration.UseTransport<LearningTransport>();

        #endregion

        #region persistence

        endpointConfiguration.UsePersistence<LearningPersistence>();

        #endregion

        #region critical-errors

        endpointConfiguration.DefineCriticalErrorAction(
            onCriticalError: async context =>
            {
                // Log the critical error
                log.Fatal($"CRITICAL: {context.Error}", context.Exception);

                await context.Stop()
                    .ConfigureAwait(false);

                // Kill the process on a critical error
                var output = $"NServiceBus critical error:\n{context.Error}\nShutting down.";
                Environment.FailFast(output, context.Exception);
            });

        #endregion

        #region start-bus

        endpointConfiguration.EnableInstallers();
        endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #endregion

        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
    }


    protected override void OnStop()
    {
        #region stop-endpoint

        endpointInstance?.Stop().GetAwaiter().GetResult();

        #endregion
    }
}