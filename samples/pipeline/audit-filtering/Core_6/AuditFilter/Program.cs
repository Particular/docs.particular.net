using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncRun().GetAwaiter().GetResult();
    }

    static async Task AsyncRun()
    {
        Console.Title = "Samples.AuditFilter";
        var endpointConfiguration = new EndpointConfiguration("Samples.AuditFilter");

        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

#region addFilterBehaviors
        endpointConfiguration.AuditProcessedMessagesTo("Samples.AuditFilter.Audit");

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register("AuditFilter.Filter", typeof(AuditFilterBehavior), "prevents marked messages from being forwarded to the audit queue");
        pipeline.Register("AuditFilter.Rules", typeof(AuditRulesBehavior), "checks whether a message should be forwarded to the audit queue");
        pipeline.Register("AuditFilter.Context", typeof(AuditFilterContextBehavior), "adds a shared state for the rules and filter behaviors");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
#endregion
        try
        {
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
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
