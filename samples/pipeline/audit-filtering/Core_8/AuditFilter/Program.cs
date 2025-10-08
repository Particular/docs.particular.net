Console.Title = "AuditFilter";

var endpointConfiguration = new EndpointConfiguration("Samples.AuditFilter");

endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

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

#endregion

var endpointInstance = await Endpoint.Start(endpointConfiguration);

var auditThisMessage = new AuditThisMessage
{
    Content = "See you in the audit queue!"
};

await endpointInstance.SendLocal(auditThisMessage);

var doNotAuditThisMessage = new DoNotAuditThisMessage
{
    Content = "Don't look for me!"
};

await endpointInstance.SendLocal(doNotAuditThisMessage);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();
