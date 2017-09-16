using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.AuditFilter";
        var endpointConfiguration = new EndpointConfiguration("Samples.AuditFilter");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region addFilterBehaviors

        endpointConfiguration.AuditProcessedMessagesTo("audit");

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(
            stepId: "AuditFilter.Filter",
            behavior: typeof(AuditFilterBehavior),
            description: "prevents marked messages from being forwarded to the audit queue");
        pipeline.Register(
            stepId: "AuditFilter.Rules",
            behavior: typeof(AuditRulesBehavior),
            description: "checks whether a message should be forwarded to the audit queue");
        pipeline.Register(
            stepId: "AuditFilter.Context",
            behavior: typeof(AuditFilterContextBehavior),
            description: "adds a shared state for the rules and filter behaviors");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #endregion

        var auditThisMessage = new AuditThisMessage
        {
            Content = "See you in the audit queue!"
        };
        await endpointInstance.SendLocal(auditThisMessage)
            .ConfigureAwait(false);

        var doNotAuditThisMessage = new DoNotAuditThisMessage
        {
            Content = "Don't look for me!"
        };
        await endpointInstance.SendLocal(doNotAuditThisMessage)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}