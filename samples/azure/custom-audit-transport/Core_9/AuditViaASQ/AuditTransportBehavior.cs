using System;
using System.Threading;
using System.Threading.Tasks;
using CustomAuditTransport;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transport;

public class AuditTransportBehavior :
    Behavior<IAuditContext>
{
    public override async Task Invoke(IAuditContext context, Func<Task> next)
    {
        var sendOptions = new SendOptions();
        sendOptions.SetDestination(context.AuditAddress);

        foreach (var item in context.AuditMetadata)
        {
            context.Message.Headers.Add(item.Key, item.Value);
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
        //public Registration(Func<IServiceProvider, AuditTransportBehavior> factory) : base("AuditTransport", typeof(AuditTransportBehavior), "Sends the audit message to ASQ", b => factory(b))
        public Registration() : base("AuditTransport", typeof(AuditTransportBehavior), "Sends the audit message to ASQ")
        {
            InsertAfterIfExists("AuditHostInformation");
            InsertAfterIfExists("LicenseReminder");
            InsertAfterIfExists("AuditProcessingStatistics");
            //InsertAfter("MarkAsAcknowledgedBehavior");
        }
    }
    #endregion
}