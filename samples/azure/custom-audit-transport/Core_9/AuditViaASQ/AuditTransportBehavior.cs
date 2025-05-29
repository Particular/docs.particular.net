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
            context.Message.Headers[item.Key] = item.Value;
        }

        var transportOperations = CreateTransportOperations(context.Message, context.AuditAddress);
        
        //Transport transaction is being set to null since we cannot use the existing ASB transaction here.
        //Each audit message is processed one at a time so there's also no point in creating an ASQ transaction for it.
        await AuditViaASQFeatureStartup.AsqDispatcher!.Dispatch(transportOperations, null, context.CancellationToken);
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