using System;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Transports;
using NServiceBus.Unicast;

#region auditRulesBehavior
public class AuditRulesBehavior :
    IBehavior<IncomingContext>
{
    IAuditMessages messageAuditer;

    public AuditRulesBehavior(IAuditMessages messageAuditer)
    {
        this.messageAuditer = messageAuditer;
    }

    public void Invoke(IncomingContext context, Action next)
    {
        next();
        var sendOptions = new SendOptions("audit");

        //set audit related headers
        var headers = context.PhysicalMessage.Headers;

        var processingStarted = context.Get<DateTime>("IncomingMessage.ProcessingStarted");
        headers[Headers.ProcessingStarted] = DateTimeExtensions.ToWireFormattedString(processingStarted);

        var processingEnded = context.Get<DateTime>("IncomingMessage.ProcessingEnded");
        headers[Headers.ProcessingEnded] = DateTimeExtensions.ToWireFormattedString(processingEnded);

        // Don't audit control messages
        if (headers.ContainsKey(Headers.ControlMessageHeader))
        {
            return;
        }

        // Do not audit messages of type DoNotAuditThisMessage.
        if (context.IncomingLogicalMessage.MessageType != typeof(DoNotAuditThisMessage))
        {
            messageAuditer.Audit(sendOptions, context.PhysicalMessage);
        }
    }
}
#endregion