namespace Snippets4.UpgradeGuides._4to5
{
    using System;
    using NServiceBus;
    using NServiceBus.Encryption;
    using NServiceBus.Installation.Environments;
    using Snippets4.Encryption.EncryptionService;

    public class Upgrade
    {
        public void MessageConventions()
        {
            #region 4to5MessageConventions

            // NOTE: When you're self hosting, '.DefiningXXXAs()' has to be before '.UnicastBus()', 
            // otherwise you'll get: 'System.InvalidOperationException: "No destination specified for message(s): MessageTypeName"

            Configure configure = Configure.With()
                .DefaultBuilder()
                .DefiningCommandsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Commands"))
                .DefiningEventsAs(t => t.Namespace == "MyNamespace" && t.Namespace.EndsWith("Events"))
                .DefiningMessagesAs(t => t.Namespace == "Messages")
                .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"))
                .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"))
                .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"))
                .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);

            #endregion
        }

        public void CustomConfigOverrides()
        {
            #region 4to5CustomConfigOverrides

            Configure configure = Configure.With(AllAssemblies.Except("NotThis.dll"))
                .DefaultBuilder();
            configure.DefineEndpointName("MyEndpointName");
            configure.DefiningEventsAs(type => type.Name.EndsWith("Event"));

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

            Configure.With()
                .RavenPersistence("http://localhost:8080", "MyDatabase");

            #endregion

        }

        public void StartupAction()
        {
            #region 4to5StartupAction

            Configure.With().UnicastBus()
                .CreateBus()
                .Start(MyCustomAction);

            #endregion
        }

        public void MyCustomAction()
        {

        }

        public void Installers()
        {
            #region 4to5Installers

            Configure.With()
                .UnicastBus()
                .CreateBus()
                .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

            #endregion
        }


        public void Persistence()
        {

            #region 4to5ConfigurePersistence

            // Configure to use InMemory 
            Configure.With().InMemorySagaPersister();
            Configure.With().UseInMemoryTimeoutPersister();
            Configure.With().InMemorySubscriptionStorage();
            Configure.With().RunGatewayWithInMemoryPersistence();
            Configure.With().UseInMemoryGatewayDeduplication();

            // Configure to use NHibernate
            Configure.With().UseNHibernateSagaPersister();
            Configure.With().UseNHibernateTimeoutPersister();
            Configure.With().UseNHibernateSubscriptionPersister();
            Configure.With().UseNHibernateGatewayPersister();
            Configure.With().UseNHibernateGatewayDeduplication();

            // Configure to use RavenDB for everything
            Configure.With().RavenPersistence();

            // Configure to use RavenDB
            Configure.With().RavenSagaPersister();
            Configure.With().UseRavenTimeoutPersister();
            Configure.With().RavenSubscriptionStorage();
            Configure.With().RunGatewayWithRavenPersistence();
            Configure.With().UseNHibernateGatewayDeduplication();

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

            Configure.With().UnicastBus()
                .RunCustomAction(MyCustomAction)
                .CreateBus()
                .Start();

            #endregion
        }

        public void DefineCriticalErrorAction()
        {

            #region 4to5DefineCriticalErrorAction

            // Configuring how NServicebus handles critical errors
            Configure.With().DefineCriticalErrorAction((message, exception) =>
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

            Configure configure = Configure.With()
                .FileShareDataBus(databusPath);

            #endregion
        }

        public void PurgeOnStartup()
        {
            #region 4to5PurgeOnStartup

            Configure.With()
                .PurgeOnStartup(true);

            #endregion
        }

        public void EncryptionServiceSimple()
        {
            #region 4to5EncryptionServiceSimple

            Configure.With()
                .RijndaelEncryptionService();

            #endregion
        }

        public void FromCustomIEncryptionService()
        {
            #region 4to5EncryptionFromIEncryptionService

            //where EncryptionService implements IEncryptionService 
            Configure.With()
                .Configurer.RegisterSingleton<IEncryptionService>(new EncryptionService());

            #endregion
        }

        public void License()
        {
            #region 4to5License

            Configure.With().LicensePath("PathToLicense");
            //or
            Configure.With().License("YourCustomLicenseText");

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

            Configure.With()
                .EnablePerformanceCounters();

            Configure.With()
                .SetEndpointSLA(TimeSpan.FromMinutes(3));

            #endregion
        }

        public void DoNotCreateQueues()
        {
            #region 4to5DoNotCreateQueues

            Configure.With().DoNotCreateQueues();

            #endregion
        }

        public void EndpointName()
        {
            #region 4to5EndpointName

            Configure.With()
                // If you need to customize the endpoint name via code using the DefineEndpointName method, 
                // it is important to call it first, right after the With() configuration entry point.
                .DefineEndpointName("MyEndpoint");

            #endregion
        }

        public void SendOnly()
        {

            #region 4to5SendOnly

            IBus bus = Configure.With()
                .DefaultBuilder()
                //Other config
                .UnicastBus()
                .SendOnly();

            #endregion
        }

    }
}