#pragma warning disable 618
namespace Core_7.UpgradeGuides._6to7.Audit
{
    using System;
    using System.Configuration;
    using NServiceBus;

    public class ConfigurationChanges
    {

        public void AuditProvideConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region 6to7AuditProvideConfiguration

            endpointConfiguration.AuditProcessedMessagesTo(
                auditQueue: "targetAuditQueue",
                timeToBeReceived: TimeSpan.FromMinutes(10));
            #endregion
        }

        public void AppConfig(EndpointConfiguration endpointConfiguration)
        {
            #region 6to7configureAuditUsingXmlReadAppSettings
            var appSettings = ConfigurationManager.AppSettings;
            var auditQueue = appSettings.Get("auditQueue");
            var timeToBeReceived = TimeSpan.Parse(appSettings.Get("timeToBeReceived"));
            endpointConfiguration.AuditProcessedMessagesTo(
                auditQueue: auditQueue,
                timeToBeReceived: timeToBeReceived);
            #endregion
        }
    }
}
