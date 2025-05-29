using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transport;

namespace AuditViaASQ;

#region auditDispatchTerminator
public class AuditDispatchTerminator :
    PipelineTerminator<IAuditContext>
{    
    protected override async Task Terminate(IAuditContext context)
    {
        var sendOptions = new SendOptions();
        sendOptions.SetDestination(context.AuditAddress);

        foreach (var item in context.AuditMetadata)
        {
            context.Message.Headers[item.Key] = item.Value;
        }

        //NOTE the ASQ transport has a message size limit of 64KB, so if the message is larger than that, it will be rejected. Some checks would need to be put in place to handle that scenario.
        var transportOperations = CreateTransportOperations(context.Message, context.AuditAddress);

        //Transport transaction is being set to null since we cannot use the existing ASB transaction here.
        //Each audit message is processed one at a time so there's also no point in creating an ASQ transaction for it.
        await AuditViaASQFeatureStartup.AsqDispatcher!.Dispatch(transportOperations, null, context.CancellationToken);
    }

    private static TransportOperations CreateTransportOperations(OutgoingMessage message, string auditQueueAddress)
    {
        return new TransportOperations(new TransportOperation(message, new UnicastAddressTag(auditQueueAddress)));
    }
}
#endregion