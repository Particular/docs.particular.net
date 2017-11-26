using System.Configuration;
using NServiceBus;

class Configure
{
    public void Enable()
    {
        #region NSBHeartbeat_Upgrade_Configure

        var endpointConfiguration = new BusConfiguration();

        var setting = ConfigurationManager.AppSettings["ServiceControl/Queue"];

        endpointConfiguration.SendHeartbeatTo(
            serviceControlQueue: setting);

        #endregion
    }
}