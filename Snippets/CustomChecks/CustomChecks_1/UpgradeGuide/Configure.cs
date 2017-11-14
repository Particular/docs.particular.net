using System.Configuration;
using NServiceBus;

class Configure
{
    public void Enable()
    {
        #region NSBCustomChecks_Upgrade_Configure

        var endpointConfiguration = new BusConfiguration();

        var setting = ConfigurationManager.AppSettings["ServiceControl/Queue"];

        endpointConfiguration.ReportCustomChecksTo(
            serviceControlQueue: setting);

        #endregion
    }
}