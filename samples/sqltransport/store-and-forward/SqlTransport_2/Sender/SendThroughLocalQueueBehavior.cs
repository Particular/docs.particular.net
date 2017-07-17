using System;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Unicast;

#region SendThroughLocalQueueBehavior

public class SendThroughLocalQueueBehavior :
    IBehavior<OutgoingContext>
{
    Configure configure;

    public SendThroughLocalQueueBehavior(Configure configure)
    {
        this.configure = configure;
    }

    public void Invoke(OutgoingContext context, Action next)
    {
        // If processing an incoming message (in a handler), skip this behavior
        if (context.IncomingMessage != null)
        {
            next();
            return;
        }

        var deliveryOptions = context.DeliveryOptions;
        var sendOptions = deliveryOptions as SendOptions;
        var outgoingHeaders = context.OutgoingLogicalMessage.Headers;
        if (sendOptions == null)
        {
            var publishOptions = deliveryOptions as PublishOptions;
            if (publishOptions == null)
            {
                // Should never get here as is makes no sense to reply from outside of a handler
                throw new Exception($"Not supported delivery option: {deliveryOptions.GetType().Name}");
            }
            // Technically it is not necessary to store the actual type,
            // just a marker that this is a Publish operation
            outgoingHeaders["$.store-and-forward.eventtype"] =
                publishOptions.EventType.AssemblyQualifiedName;
        }
        else
        {
            outgoingHeaders["$.store-and-forward.destination"] =
                sendOptions.Destination.ToString();
            sendOptions.Destination = configure.LocalAddress;
            // Could as well store other properties of the SendOptions
            // to handle things like delayed delivery
        }
        context.Set<DeliveryOptions>(new SendOptions(configure.LocalAddress));
        next();
    }

    public class Registration :
        RegisterStep
    {
        public Registration()
            : base(
                stepId: "SendThroughLocalQueue",
                behavior: typeof(SendThroughLocalQueueBehavior),
                description: "Put the outgoing message into this endpoint's input queue")
        {
            InsertBefore(WellKnownStep.MutateOutgoingMessages);
            InsertAfter(WellKnownStep.EnforceBestPractices);
        }
    }
}

#endregion