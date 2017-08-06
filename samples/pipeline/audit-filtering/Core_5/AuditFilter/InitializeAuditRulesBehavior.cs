using NServiceBus;
using NServiceBus.Pipeline;

#region addFilterBehaviors
class InitializeNewAuditBehavior :
    INeedInitialization
{
    public void Customize(BusConfiguration configuration)
    {
        // Replace the existing auditing step with our new one in the Pipeline
        configuration.Pipeline.Replace(
            wellKnownStep: WellKnownStep.AuditProcessedMessage,
            newBehavior: typeof(AuditRulesBehavior),
            description: "Replaces existing audit behavior with the new behavior that filters certain message types.");
    }
}
#endregion