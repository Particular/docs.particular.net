using System;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Transports;
using NServiceBus.Unicast;

#region auditRulesBehavior
public class AuditRulesBehavior : IBehavior<IncomingContext>
{
    public IAuditMessages MessageAuditer { get; set; }
    public TimeSpan? TimeToBeReceivedOnForwardedMessages { get; set; }

    public void Invoke(IncomingContext context, Action next)
    {
        next();
        var sendOptions = new SendOptions("audit")
        {
            TimeToBeReceived = TimeToBeReceivedOnForwardedMessages
        };

        //set audit related headers
        context.PhysicalMessage.Headers[Headers.ProcessingStarted] = DateTimeExtensions.ToWireFormattedString(context.Get<DateTime>("IncomingMessage.ProcessingStarted"));
        context.PhysicalMessage.Headers[Headers.ProcessingEnded] = DateTimeExtensions.ToWireFormattedString(context.Get<DateTime>("IncomingMessage.ProcessingEnded"));

        // Don't audit control messages
        if (context.PhysicalMessage.Headers.ContainsKey(Headers.ControlMessageHeader))
        {
            return;
        }

        // Do not audit messages of type DoNotAuditThisMessage. 
        if (context.IncomingLogicalMessage.MessageType != typeof(DoNotAuditThisMessage))
        {
            MessageAuditer.Audit(sendOptions, context.PhysicalMessage);
        }
    }
}
#endregion