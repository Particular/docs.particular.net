using System.Configuration;
using NServiceBus;

class Configure
{
    public void Enable()
    {
        #region NSBSagaAudit_Upgrade_Configure

        var endpointConfiguration = new BusConfiguration();

        var setting = ConfigurationManager.AppSettings["ServiceControl/Queue"];

        endpointConfiguration.AuditSagaStateChanges(
            serviceControlQueue: setting);

        #endregion
    }
}