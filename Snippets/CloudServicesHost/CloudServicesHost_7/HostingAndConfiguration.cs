namespace Snippets5.Azure.CloudServices
{
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NServiceBus;
    using NServiceBus.Hosting.Azure;

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
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            endpointConfiguration.UsePersistence<AzureStoragePersistence>();
        }
    }

    #endregion

    #region HostingInWebRole

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            StrartBus().GetAwaiter().GetResult();
        }

        static async Task StrartBus()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("EndpointName");
            endpointConfiguration.AzureConfigurationSource();
            endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
            endpointConfiguration.UsePersistence<AzureStoragePersistence>();

            IEndpointInstance endpointInstance = await Endpoint.Start(endpointConfiguration);
        }
    }

    #endregion
}
