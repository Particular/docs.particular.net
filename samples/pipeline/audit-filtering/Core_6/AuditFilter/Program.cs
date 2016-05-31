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

        endpointConfiguration.Pipeline.Register("AuditFilter.Filter", typeof(AuditFilterBehavior), "prevents marked messages from being forwarded to the audit queue");
        endpointConfiguration.Pipeline.Register("AuditFilter.Rules", typeof(AuditRulesBehavior), "checks whether a message should be forwarded to the audit queue");
        endpointConfiguration.Pipeline.Register("AuditFilter.Context", typeof(AuditFilterContextBehavior), "adds a shared state for the rules and filter behaviors");

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
#endregion
        try
        {
            await endpointInstance.SendLocal(new AuditThisMessage {Content = "See you in the audit queue!"});
            await endpointInstance.SendLocal(new DoNotAuditThisMessage {Content = "Don't look for me!"});

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            await endpointInstance.Stop();
        }
    }
}
