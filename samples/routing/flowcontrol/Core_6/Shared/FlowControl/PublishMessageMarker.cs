using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class PublishMessageMarker : Behavior<IOutgoingPublishContext>
{
    public PublishMessageMarker(string controlAddress, string sessionId)
    {
        this.controlAddress = controlAddress;
        this.sessionId = sessionId;
    }

    public override Task Invoke(IOutgoingPublishContext context, Func<Task> next)
    {
        context.Headers["NServiceBus.FlowControl.ControlAddress"] = controlAddress;
        context.Headers["NServiceBus.FlowControl.SessionId"] = sessionId;
        return next();
    }

    string controlAddress;
    string sessionId;
}