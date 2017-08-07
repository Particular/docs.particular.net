using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;
using NServiceBus;
using NServiceBus.Hosting.Azure;

#region HostingInWorkerRole

public class WorkerRole :
    RoleEntryPoint
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

#region AzureServiceBusTransportWithAzureHost

public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Worker
{
    public void Customize(BusConfiguration busConfiguration)
    {
        var transport = busConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");
        busConfiguration.UsePersistence<AzureStoragePersistence>();
    }
}


#endregion

#region HostingInWebRole

public class MvcApplication :
    HttpApplication
{
    protected void Application_Start()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.AzureConfigurationSource();
        busConfiguration.UseTransport<AzureStorageQueueTransport>();
        busConfiguration.UsePersistence<AzureStoragePersistence>();
        var startableBus = Bus.Create(busConfiguration);
        var bus = startableBus.Start();
    }
}
#endregion

#region AsAHost

public class EndpointHostConfig :
    IConfigureThisEndpoint,
    AsA_Host
{
    public void Customize(BusConfiguration configuration)
    {
    }
}

#endregion