namespace Core4.UpgradeGuides._4to5
{
    using System;
    using NServiceBus;
    using NServiceBus.Installation.Environments;
    using NServiceBus.Logging;
    using NServiceBus.Transports;

    class Upgrade
    {
        void RemovePrincipalHack(Configure configure)
        {
            #region 4to5RemovePrincipalHack

            var unicastBus = configure.UnicastBus();
            unicastBus.RunHandlersUnderIncomingPrincipal(true);

            #endregion
        }

        void MessageConventions(Configure configure)
        {
            #region 4to5MessageConventions

            // NOTE: When self hosting, '.DefiningXXXAs()' has to be before '.UnicastBus()',
            // otherwise it will result in:
            // 'InvalidOperationException: "No destination specified for message(s): MessageTypeName"
            configure.DefaultBuilder();
            configure.DefiningCommandsAs(
                type =>
                {
                    return type.Namespace == "MyNamespace" &&
                           type.Namespace.EndsWith("Commands");
                });
            configure.DefiningEventsAs(
                type =>
                {
                    return type.Namespace == "MyNamespace" &&
                           type.Namespace.EndsWith("Events");
                });
            configure.DefiningMessagesAs(
                type =>
                {
                    return type.Namespace == "Messages";
                });
            configure.DefiningDataBusPropertiesAs(
                property =>
                {
                    return property.Name.EndsWith("DataBus");
                });
            configure.DefiningExpressMessagesAs(
                type =>
                {
                    return type.Name.EndsWith("Express");
                });
            configure.DefiningTimeToBeReceivedAs(
                type =>
                {
                    if (type.Name.EndsWith("Expires"))
                    {
                        return TimeSpan.FromSeconds(30);
                    }
                    return TimeSpan.MaxValue;
                });

            #endregion
        }

        void CustomConfigOverrides()
        {
            #region 4to5CustomConfigOverrides

            var configure = Configure.With(AllAssemblies.Except("NotThis.dll"));
            configure.DefaultBuilder();
            configure.DefineEndpointName("MyEndpointName");
            configure.DefiningEventsAs(
                type =>
                {
                    return type.Name.EndsWith("Event");
                });

            #endregion
        }

        void UseTransport(Configure configure)
        {
            #region 4to5UseTransport

            // Choose one of the following

            configure.UseTransport<Msmq>();

            configure.UseTransport<RabbitMQ>();

            configure.UseTransport<SqlServer>();

            configure.UseTransport<AzureStorageQueue>();

            configure.UseTransport<AzureServiceBus>();

            #endregion
        }

        class AzureStorageQueue :
            TransportDefinition
        {
        }

        class AzureServiceBus :
            TransportDefinition
        {
        }

        class RabbitMQ :
            TransportDefinition
        {
        }

        class SqlServer :
            TransportDefinition
        {
        }

        void InterfaceMessageCreation(IBus Bus)
        {
            #region 4to5InterfaceMessageCreation

            var message = Bus.CreateInstance<MyInterfaceMessage>(
                action: interfaceMessage =>
                {
                    interfaceMessage.OrderNumber = 1234;
                });
            Bus.Publish(message);

            Bus.Publish<MyInterfaceMessage>(
                messageConstructor: interfaceMessage =>
                {
                    interfaceMessage.OrderNumber = 1234;
                });

            #endregion
        }

        public interface MyInterfaceMessage
        {
            int OrderNumber { get; set; }
        }


        void CustomRavenConfig(Configure configure)
        {
            #region 4to5CustomRavenConfig

            configure.RavenPersistence("http://localhost:8080", "MyDatabase");

            #endregion
        }

        void StartupAction(Configure configure)
        {
            #region 4to5StartupAction

            configure.UnicastBus();
            var startableBus = configure.CreateBus();
            startableBus.Start(
                startupAction: () =>
                {
                    MyCustomAction();
                });

            #endregion
        }

        public void MyCustomAction()
        {

        }

        public void Installers()
        {
            #region 4to5Installers

            var configure = Configure.With();
            configure.UnicastBus();
            var startableBus = configure.CreateBus();
            startableBus.Start(
                startupAction: () =>
                {
                    configure.ForInstallationOn<Windows>().Install();
                });

            #endregion
        }


        public void Persistence()
        {

            var configure = Configure.With();

            #region 4to5ConfigurePersistence

            // Configure to use InMemory
            configure.InMemorySagaPersister();
            configure.UseInMemoryTimeoutPersister();
            configure.InMemorySubscriptionStorage();
            configure.RunGatewayWithInMemoryPersistence();
            configure.UseInMemoryGatewayDeduplication();

            // Configure to use NHibernate
            configure.UseNHibernateSagaPersister();
            configure.UseNHibernateTimeoutPersister();
            configure.UseNHibernateSubscriptionPersister();
            configure.UseNHibernateGatewayPersister();
            configure.UseNHibernateGatewayDeduplication();

            // Configure to use RavenDB for everything
            configure.RavenPersistence();

            // Configure to use RavenDB
            configure.RavenSagaPersister();
            configure.UseRavenTimeoutPersister();
            configure.RavenSubscriptionStorage();
            configure.RunGatewayWithRavenPersistence();
            configure.UseNHibernateGatewayDeduplication();

            #endregion
        }

        #region 4to5BusExtensionMethodForHandler

        public class MyHandler :
            IHandleMessages<MyMessage>
        {
            public void Handle(MyMessage message)
            {
                var otherMessage = new OtherMessage();
                this.Bus().Reply(otherMessage);
            }
        }

        #endregion

        public class MyMessage
        {
        }

        public class OtherMessage
        {
        }

        public void RunCustomAction()
        {
            #region 4to5RunCustomAction

            var configure = Configure.With();
            configure.UnicastBus();
            configure.RunCustomAction(MyCustomAction);
            var startableBus = configure.CreateBus();
            startableBus.Start();

            #endregion
        }

        public void DefineCriticalErrorAction(ILog log)
        {
            #region 4to5DefineCriticalErrorAction

            // Configuring how NServiceBus handles critical errors
            var configure = Configure.With();
            configure.DefineCriticalErrorAction(
                (message, exception) =>
                {
                    var output = $"Critical exception: '{message}'";
                    log.Error(output, exception);
                    // Perhaps end the process?
                });

            #endregion
        }

        public void PurgeOnStartup()
        {
            #region 4to5PurgeOnStartup

            var configure = Configure.With();
            configure.PurgeOnStartup(true);

            #endregion
        }

        public void License()
        {
            var configure = Configure.With();

            #region 4to5License

            configure.LicensePath("PathToLicense");
            // or
            configure.License("YourCustomLicenseText");

            #endregion
        }

        public void TransactionConfig()
        {
            #region 4to5TransactionConfig

            // Enable
            Configure.Transactions.Enable();

            // Disable
            Configure.Transactions.Disable();

            #endregion
        }

        public void StaticConfigureEndpoint()
        {
            #region 4to5StaticConfigureEndpoint

            var endpoint = Configure.Endpoint;
            endpoint.AsSendOnly();
            endpoint.AsVolatile();
            endpoint.Advanced(settings => settings.DisableDurableMessages());
            endpoint.Advanced(settings => settings.EnableDurableMessages());

            #endregion
        }

        public void PerformanceMonitoring()
        {
            #region 4to5PerformanceMonitoring

            var configure = Configure.With();
            configure.EnablePerformanceCounters();
            configure.SetEndpointSLA(TimeSpan.FromMinutes(3));

            #endregion
        }

        public void DoNotCreateQueues()
        {
            #region 4to5DoNotCreateQueues

            var configure = Configure.With();
            configure.DoNotCreateQueues();

            #endregion
        }

        public void EndpointName()
        {
            var configure = Configure.With();

            #region 4to5EndpointName

            // To customize the endpoint name via code using the DefineEndpointName method,
            // it is important to call it first, right after the With() configuration entry point.
            configure.DefineEndpointName("MyEndpoint");

            #endregion
        }

        public void SendOnly()
        {
            var configure = Configure.With();

            #region 4to5SendOnly

            configure.DefaultBuilder();
            // Other config
            configure.UnicastBus();
            var bus = configure.SendOnly();

            #endregion
        }
    }



    public static class FakeExtensions
    {
        public static void UseNHibernateGatewayDeduplication(this Configure configure)
        {
        }
        public static void UseNHibernateSagaPersister(this Configure configure)
        {
        }
        public static void UseNHibernateTimeoutPersister(this Configure configure)
        {
        }
        public static void UseNHibernateSubscriptionPersister(this Configure configure)
        {
        }
        public static void UseNHibernateGatewayPersister(this Configure configure)
        {
        }
    }
}