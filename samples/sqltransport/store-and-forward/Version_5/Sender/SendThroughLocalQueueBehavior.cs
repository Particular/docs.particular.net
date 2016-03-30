using System;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Unicast;

public class SendThroughLocalQueueBehavior : IBehavior<OutgoingContext>
{
    Configure configure;

    public SendThroughLocalQueueBehavior(Configure configure)
    {
        this.configure = configure;
    }

    public void Invoke(OutgoingContext context, Action next)
    {
        #region SendThroughLocalQueueBehavior
        if (context.IncomingMessage != null) //If we are processing an incoming message (in a handler), we skip this behavior
        {
            next();
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
            : base("SendThroughLocalQueue", typeof(SendThroughLocalQueueBehavior), "Put the outgoing message into this endpoint's input queue")
        {
            InsertBefore(WellKnownStep.MutateOutgoingMessages);
            InsertAfter(WellKnownStep.EnforceBestPractices);
        }
    }
}