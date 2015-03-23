using System;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Pipeline;

public static class AuditConfigurationHelper
{

    #region repalce-audit

    public static void ReplaceAuditBehaviour(this BusConfiguration configuration)
    {
        AuditConfig messageAuditingConfig = configuration.GetSettings().GetConfigSection<AuditConfig>();
        if (messageAuditingConfig != null && messageAuditingConfig.OverrideTimeToBeReceived > TimeSpan.Zero)
        {
            configuration.RegisterComponents(x=>x.ConfigureProperty<FilterAuditMessageTypeBehavior>(y=>y.TimeToBeReceivedOnForwardedMessages, messageAuditingConfig.OverrideTimeToBeReceived));
        }

        configuration.Pipeline.Replace(
            WellKnownStep.AuditProcessedMessage,
            typeof(FilterAuditMessageTypeBehavior),
            "Replaces existing audit behavior with the new behavior that filters certain messages");
    }

    #endregion
}

