
using NServiceBus;

public static class AuditConfigurationHelper
{

    #region repalce-audit

    public static void ReplaceAuditBehaviour(this Configure configuration)
    {
        configuration.pipPipeline.Replace(
            WellKnownStep.AuditProcessedMessage,
            typeof(FilterAuditMessageTypeBehavior),
            "Replaces existing audit behavior with the new behavior that filters certain messages");
    }

    #endregion
}
