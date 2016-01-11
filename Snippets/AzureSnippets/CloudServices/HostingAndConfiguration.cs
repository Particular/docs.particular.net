namespace Snippets5.Azure.CloudServices
{
    using System.Web;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NServiceBus;
    using NServiceBus.Hosting.Azure;

    #region HostingInWorkerRole 5

    public class WorkerRole : RoleEntryPoint
    {
        NServiceBusRoleEntrypoint nsb = new NServiceBusRoleEntrypoint();

        public override bool OnStart()
        {
            nsb.Start();

            return base.OnStart();
        }

        public override void OnStop()
        {
            nsb.Stop();

            base.OnStop();
        }
    }

    #endregion

    #region ConfigureEndpoint 5

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
    {
        public void Customize(BusConfiguration builder)
        {
            builder.UseTransport<AzureServiceBusTransport>();
            builder.UsePersistence<AzureStoragePersistence>();
        }
    }

    #endregion

    #region HostingInWebRole 5

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.AzureConfigurationSource();
            busConfiguration.UseTransport<AzureStorageQueueTransport>();
            busConfiguration.UsePersistence<AzureStoragePersistence>();
            IStartableBus startableBus = Bus.Create(busConfiguration);
            IBus bus = startableBus.Start();
        }
    }

    #endregion
}
