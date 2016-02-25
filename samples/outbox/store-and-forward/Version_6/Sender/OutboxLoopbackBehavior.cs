using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Transports;
using NServiceBus.Unicast;

public class OutboxLoopbackSendBehavior : Behavior<IOutgoingLogicalMessageContext>
{
    public override async Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        #region OutboxLoopbackSendBehavior
        IncomingMessage incomingPhysicalMessage;
        context.TryGetIncomingPhysicalMessage(out incomingPhysicalMessage);
        if (incomingPhysicalMessage != null) //If we are processing an incoming message (in a handler), we skip this behavior
        {
            await next().ConfigureAwait(false);
            return;
        }

        SendOptions sendOptions = context.DeliveryOptions as SendOptions;
        if (sendOptions != null)
        {
            context.OutgoingLogicalMessage.Headers["$.store-and-forward.destination"] =
                sendOptions.Destination.ToString();
            sendOptions.Destination = configure.LocalAddress;
            //We could as well store other properties of the SendOptions to handle things like delayed delivery
        }
        else
        {
            PublishOptions publishOptions = context.DeliveryOptions as PublishOptions;
            if (publishOptions != null)
            {
                //Technically we don't need to store tha actual type, just a marker that this is a Publish operation
                context.OutgoingLogicalMessage.Headers["$.store-and-forward.eventtype"] =
                    publishOptions.EventType.AssemblyQualifiedName;
            }
            else
            {
                //We should never get here as is makes no sense to reply from outside of a handler
                throw new NotSupportedException("Not supported delivery option: " + context.DeliveryOptions.GetType().Name);
            }
        }
        context.Set<DeliveryOptions>(new SendOptions(configure.LocalAddress));
        next();
        #endregion
    }

    public class Registration : RegisterStep
    {
        public Registration()
            : base("OutboxLoopbackSend", typeof(OutboxLoopbackSendBehavior), "OutboxLoopbackSendRegistration")
        {
            InsertBefore(WellKnownStep.MutateOutgoingMessages);
            InsertAfter(WellKnownStep.EnforceBestPractices);
        }
    }
}