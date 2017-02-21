using NServiceBus;
using NServiceBus.Pipeline;

class InitializeNewAuditBehavior : INeedInitialization
{
    public void Customize(BusConfiguration configuration)
    {
        // Replace the existing auditing step with our new one in the Pipeline
        configuration.Pipeline.Replace(WellKnownStep.AuditProcessedMessage, typeof(AuditRulesBehavior), "Replaces existing audit behavior with the new behavior that filters certain messages");
    }
}
