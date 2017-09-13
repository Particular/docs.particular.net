using Microsoft.WindowsAzure.ServiceRuntime;
using NServiceBus;

#region AzureSharedHosting_HostConfiguration

public class EndpointConfig :
#pragma warning disable 618
    IConfigureThisHost
#pragma warning restore 618
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
    }

    public HostingSettings Configure()
    {
        var connectionString = RoleEnvironment.GetConfigurationSettingValue("HostWorker.ConnectionString");
        var autoUpdate = bool.Parse(RoleEnvironment.GetConfigurationSettingValue("HostWorker.AutoUpdate"));
        var updateInterval = int.Parse(RoleEnvironment.GetConfigurationSettingValue("HostWorker.UpdateInterval"));
        var container = RoleEnvironment.GetConfigurationSettingValue("HostWorker.Container");

        return new HostingSettings(connectionString)
        {
            AutoUpdate = autoUpdate,
            UpdateInterval = updateInterval,
            Container = container
        };
    }
}

#endregion