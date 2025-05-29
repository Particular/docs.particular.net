using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transport;

namespace AuditViaASQ;

public class AuditTransportBehavior :
    Behavior<IAuditContext>
{
    public override async Task Invoke(IAuditContext context, Func<Task> next)
    {
        var sendOptions = new SendOptions();
        sendOptions.SetDestination(context.AuditAddress);

        foreach (var item in context.AuditMetadata)
        {
            //if(context.Message.Headers)            
            //context.Message.Headers.Add(item.Key, item.Value);
            context.Message.Headers[item.Key] = item.Value;
        }

        var transportOperations = CreateTransportOperations(context.Message, context.AuditAddress);

        // TODO: Can we get this from anywhere?
        TransportTransaction? transportTransaction = null;

        await AuditViaASQFeatureStartup.AsqDispatcher!.Dispatch(transportOperations, transportTransaction, context.CancellationToken);
    }

    private static TransportOperations CreateTransportOperations(OutgoingMessage message, string auditQueueAddress)
    {
        return new TransportOperations(new TransportOperation(message, new UnicastAddressTag(auditQueueAddress)));
    }

    #region behaviorRegistration
    public class Registration : RegisterStep
    {
        public Registration() : base("AuditTransport", typeof(AuditTransportBehavior), "Sends the audit message to ASQ")
        {
            InsertAfterIfExists("AuditHostInformation");
            InsertAfterIfExists("LicenseReminder");
            InsertAfterIfExists("AuditProcessingStatistics");
            InsertAfterIfExists("MarkAsAcknowledgedBehavior");
        }
    }
    #endregion
}