using System;
using System.Linq;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Transports;
using NServiceBus.Unicast;

#region behavior
class FilterAuditMessageTypeBehavior : IBehavior<IncomingContext>
{

    public IAuditMessages MessageAuditer { get; set; }
    public TimeSpan? TimeToBeReceivedOnForwardedMessages { get; set; }

    public void Invoke(IncomingContext context, Action next)
    {
        next();
        SendOptions sendOptions = new SendOptions("audit")
        {
            TimeToBeReceived = TimeToBeReceivedOnForwardedMessages
        };

        //set audit related headers
        context.PhysicalMessage.Headers[Headers.ProcessingStarted] = DateTimeExtensions.ToWireFormattedString(context.Get<DateTime>("IncomingMessage.ProcessingStarted"));
        context.PhysicalMessage.Headers[Headers.ProcessingEnded] = DateTimeExtensions.ToWireFormattedString(context.Get<DateTime>("IncomingMessage.ProcessingEnded"));

        // Do not audit messages that have a SkipAuditAttribute. 
        Type messageType = context.IncomingLogicalMessage.MessageType;
        bool containsSkipAuditAttribute = messageType.GetCustomAttributes(typeof(SkipAuditAttribute), true).Any();
        if (!containsSkipAuditAttribute)
        {
            MessageAuditer.Audit(sendOptions, context.PhysicalMessage);
        }
    }
}
#endregion