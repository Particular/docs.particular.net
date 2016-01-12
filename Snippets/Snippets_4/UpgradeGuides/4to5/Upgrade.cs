namespace Snippets4.UpgradeGuides._4to5
{
    using System;
    using NServiceBus;
    using NServiceBus.Encryption;
    using NServiceBus.Installation.Environments;
    using Snippets4.Encryption.EncryptionService;

    public class Upgrade
    {
        public void ImpersonateSender()
        {
            #region 4to5RunHandlersUnderIncomingPrincipal
            Configure configure = Configure.With();
            configure.UnicastBus()
                .RunHandlersUnderIncomingPrincipal(true);
            #endregion
        }

        public void MessageConventions()
        {
            #region 4to5MessageConventions

            // NOTE: When you're self hosting, '.DefiningXXXAs()' has to be before '.UnicastBus()', 
            // otherwise you'll get: 'System.InvalidOperationException: "No destination specified for message(s): MessageTypeName"
            Configure configure = Configure.With();
            configure.DefaultBuilder();
            configure.DefiningCommandsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Commands"));
            configure.DefiningEventsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Events"));
            configure.DefiningMessagesAs(t => t.Namespace == "Messages");
            configure.DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
            configure.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
            configure.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
            configure.DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);

            #endregion
        }

        public void CustomConfigOverrides()
        {
            #region 4to5CustomConfigOverrides

            Configure configure = Configure.With(AllAssemblies.Except("NotThis.dll"));
            configure.DefaultBuilder();
            configure.DefineEndpointName("MyEndpointName");
            configure.DefiningEventsAs(type => type.Name.EndsWith("Event"));

            #endregion
        }

        public void UseTransport()
        {
            Configure configure = Configure.With();
            #region 4to5UseTransport
            //Choose one of the following

            configure.UseTransport<Msmq>();

            configure.UseTransport<RabbitMQ>();

            configure.UseTransport<SqlServer>();

            configure.UseTransport<AzureStorageQueue>();

            configure.UseTransport<AzureServiceBus>();

            #endregion
        }

        public void InterfaceMessageCreation()
        {
            IBus Bus = null;

            #region 4to5InterfaceMessageCreation

            MyInterfaceMessage message = Bus.CreateInstance<MyInterfaceMessage>(o =>
            {
                o.OrderNumber = 1234;
            });
            Bus.Publish(message);

            Bus.Publish<MyInterfaceMessage>(o =>
            {
                o.OrderNumber = 1234;
            });

            #endregion
        }

        public interface MyInterfaceMessage
        {
            int OrderNumber { get; set; }
        }


        public void CustomRavenConfig()
        {
            #region 4to5CustomRavenConfig

            Configure configure = Configure.With();
            configure.RavenPersistence("http://localhost:8080", "MyDatabase");

            #endregion
        }

        public void StartupAction()
        {
            #region 4to5StartupAction

            Configure configure = Configure.With();
            configure.UnicastBus();
            IStartableBus startableBus = configure.CreateBus();
            startableBus.Start(MyCustomAction);

            #endregion
        }

        public void MyCustomAction()
        {

        }

        public void Installers()
        {
            #region 4to5Installers

            Configure configure = Configure.With();
            configure.UnicastBus();
            IStartableBus startableBus = configure.CreateBus();
            startableBus.Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

            #endregion
        }


        public void Persistence()
        {

            Configure configure = Configure.With();
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

        public class MyHandler : IHandleMessages<MyMessage>
        {
            public void Handle(MyMessage message)
            {
                this.Bus().Reply(new OtherMessage());
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

            Configure configure = Configure.With();
            configure.UnicastBus();
            configure.RunCustomAction(MyCustomAction);
            IStartableBus startableBus = configure.CreateBus();
            startableBus.Start();

            #endregion
        }

        public void DefineCriticalErrorAction()
        {
            #region 4to5DefineCriticalErrorAction

            // Configuring how NServicebus handles critical errors
            Configure configure = Configure.With();
            configure.DefineCriticalErrorAction((message, exception) =>
            {
                string output = string.Format("We got a critical exception: '{0}'\r\n{1}", message, exception);
                Console.WriteLine(output);
                // Perhaps end the process??
            });

            #endregion
        }

        public void FileShareDataBus()
        {
            string databusPath = null;

            #region 4to5FileShareDataBus

            Configure configure = Configure.With();
            configure.FileShareDataBus(databusPath);

            #endregion
        }

        public void PurgeOnStartup()
        {
            #region 4to5PurgeOnStartup

            Configure configure = Configure.With();
            configure.PurgeOnStartup(true);

            #endregion
        }

        public void EncryptionServiceSimple()
        {
            #region 4to5EncryptionServiceSimple

            Configure configure = Configure.With();
            configure.RijndaelEncryptionService();

            #endregion
        }

        public void FromCustomIEncryptionService()
        {
            #region 4to5EncryptionFromIEncryptionService

            //where EncryptionService implements IEncryptionService 
            Configure configure = Configure.With();
            configure.Configurer.RegisterSingleton<IEncryptionService>(new EncryptionService());

            #endregion
        }

        public void License()
        {
            Configure configure = Configure.With();
            #region 4to5License

            configure.LicensePath("PathToLicense");
            //or
            configure.License("YourCustomLicenseText");

            #endregion
        }

        public void TransactionConfig()
        {
            #region 4to5TransactionConfig

            //Enable
            Configure.Transactions.Enable();

            // Disable
            Configure.Transactions.Disable();

            #endregion
        }

        public void StaticConfigureEndpoint()
        {
            #region 4to5StaticConfigureEndpoint

            Configure.Endpoint.AsSendOnly();
            Configure.Endpoint.AsVolatile();
            Configure.Endpoint.Advanced(settings => settings.DisableDurableMessages());
            Configure.Endpoint.Advanced(settings => settings.EnableDurableMessages());

            #endregion
        }

        public void PerformanceMonitoring()
        {
            #region 4to5PerformanceMonitoring

            Configure configure = Configure.With();
            configure.EnablePerformanceCounters();
            configure.SetEndpointSLA(TimeSpan.FromMinutes(3));

            #endregion
        }

        public void DoNotCreateQueues()
        {
            #region 4to5DoNotCreateQueues

            Configure configure = Configure.With();
            configure.DoNotCreateQueues();

            #endregion
        }

        public void EndpointName()
        {
            #region 4to5EndpointName

            Configure configure = Configure.With();
            // If you need to customize the endpoint name via code using the DefineEndpointName method, 
            // it is important to call it first, right after the With() configuration entry point.
            configure.DefineEndpointName("MyEndpoint");

            #endregion
        }

        public void SendOnly()
        {
            #region 4to5SendOnly

            Configure configure = Configure.With();
            configure.DefaultBuilder();
            //Other config
            configure.UnicastBus();
            IBus bus = configure.SendOnly();

            #endregion
        }
    }
}