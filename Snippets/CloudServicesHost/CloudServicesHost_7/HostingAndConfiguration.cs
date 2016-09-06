using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;
using NServiceBus;

#region HostingInWorkerRole

public class WorkerRole :
    RoleEntryPoint
{
    NServiceBusRoleEntrypoint roleEntrypoint = new NServiceBusRoleEntrypoint();

    public override bool OnStart()
    {
        roleEntrypoint.Start();
        return base.OnStart();
    }

    public override void OnStop()
    {
        roleEntrypoint.Stop();
        base.OnStop();
    }
}

#endregion

#region AzureServiceBusTransportWithAzureHost

public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Worker
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");
        endpointConfiguration.UsePersistence<AzureStoragePersistence>();
    }
}

#endregion

#region HostingInWebRole

public class MvcApplication :
    HttpApplication
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

#region AsAHost

public class EndpointHostConfig :
    IConfigureThisHost
{
    public HostingSettings Configure()
    {
        return new HostingSettings("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
    }
}

#endregion