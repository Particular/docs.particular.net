using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;
using NServiceBus;

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
        StartBus().GetAwaiter().GetResult();
    }

    static async Task StartBus()
    {
        var endpointConfiguration = new EndpointConfiguration("EndpointName");
        endpointConfiguration.AzureConfigurationSource();
        endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        endpointConfiguration.UsePersistence<AzureStoragePersistence>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
    }
}

#endregion
