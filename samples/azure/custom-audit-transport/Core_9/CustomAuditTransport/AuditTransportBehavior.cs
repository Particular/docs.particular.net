using System;
using System.Threading.Tasks;
using CustomAuditTransport;
using NServiceBus;
using NServiceBus.Pipeline;

public class AuditTransportBehavior :
    Behavior<IAuditContext>
{


    public AuditTransportBehavior()
    {

    }

    public override async Task Invoke(IAuditContext context, Func<Task> next)
    {
        //currently this fails as what is being sent is NServiceBus.Transport.OutgoingMessage - that's not something nsb recognises as a message/command/event
        //perhaps we need to "interrupt" the pipeline when the message has not been converted as yet> But then we would be missing the required audit headers?
        SendOptions sendOptions = new SendOptions();
        sendOptions.SetDestination(context.AuditAddress);
        await AuditViaASQFeatureStartup.auditEndpoint.Send(context.Message, sendOptions, context.CancellationToken);
    }
}