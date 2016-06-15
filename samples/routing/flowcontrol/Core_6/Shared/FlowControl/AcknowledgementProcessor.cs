using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class AcknowledgementProcessor : Behavior<IIncomingPhysicalMessageContext>
{
    public AcknowledgementProcessor(FlowManager flowManager, string currentSessionId)
    {
        this.flowManager = flowManager;
        this.currentSessionId = currentSessionId;
    }

    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        string ackString;
        string instanceString;
        string endpoint;
        string controlAddress;
        string sessionId;
        if (!context.MessageHeaders.TryGetValue("NServiceBus.FlowControl.Marker", out ackString)
            || !context.MessageHeaders.TryGetValue("NServiceBus.FlowControl.Endpoint", out endpoint)
            || !context.MessageHeaders.TryGetValue("NServiceBus.FlowControl.Instance", out instanceString)
            || !context.MessageHeaders.TryGetValue("NServiceBus.FlowControl.ControlAddress", out controlAddress)
            || !context.MessageHeaders.TryGetValue("NServiceBus.FlowControl.SessionId", out sessionId))
        {
            await next().ConfigureAwait(false);
            return;
        }
        if (sessionId != currentSessionId)
        {
            return;
        }
        var instanceHash = instanceString.GetHashCode();
        var ack = long.Parse(ackString);
        flowManager.Acknowledge(controlAddress, endpoint, instanceHash, ack);
    }

    FlowManager flowManager;
    string currentSessionId;
}