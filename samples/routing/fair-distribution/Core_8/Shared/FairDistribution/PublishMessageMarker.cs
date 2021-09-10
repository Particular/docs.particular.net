using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class PublishMessageMarker :
    Behavior<IOutgoingPublishContext>
{
    public PublishMessageMarker(string controlAddress, string sessionId)
    {
        this.controlAddress = controlAddress;
        this.sessionId = sessionId;
    }

    public override Task Invoke(IOutgoingPublishContext context, Func<Task> next)
    {
        var headers = context.Headers;
        headers["NServiceBus.FlowControl.ControlAddress"] = controlAddress;
        headers["NServiceBus.FlowControl.SessionId"] = sessionId;
        return next();
    }

    string controlAddress;
    string sessionId;
}