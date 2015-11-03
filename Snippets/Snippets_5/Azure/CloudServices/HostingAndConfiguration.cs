namespace Snippets5.Azure.CloudServices
{
    using System.Web;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NServiceBus;

    #region Replacement for using NServiceBus.Hosting.Azure - can't mix nsb.host and azure host in the same project

    class NServiceBusRoleEntrypoint
    {
        public void Start()
        {
        }

        public void Stop()
        {
        }
    }

    public interface AsA_Worker
    {
    }

    static class X
    {
        public static BusConfiguration AzureConfigurationSource(this BusConfiguration config)
        {
            return null;
        }
    }

    #endregion

    #region HostingInWorkerRole

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

    #region ConfigureEndpoint

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
    {
        public void Customize(BusConfiguration builder)
        {
            builder.UseTransport<AzureServiceBusTransport>();
            builder.UsePersistence<AzureStoragePersistence>();
        }
    }

    #endregion

    #region HostingInWebRole

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.AzureConfigurationSource();
            busConfiguration.UseTransport<AzureStorageQueueTransport>();
            busConfiguration.UsePersistence<AzureStoragePersistence>();
            IStartableBus startableBus = Bus.Create(busConfiguration);
            IBus bus = startableBus.Start();
        }
    }

    #endregion
}
