using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class AcknowledgementProcessor :
    Behavior<IIncomingPhysicalMessageContext>
{
    public AcknowledgementProcessor(FlowManager flowManager, string currentSessionId)
    {
        this.flowManager = flowManager;
        this.currentSessionId = currentSessionId;
    }

    #region ProcessACKs
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        var headers = context.MessageHeaders;
        if (!headers.TryGetValue("NServiceBus.FlowControl.ACK", out var ackString)
            || !headers.TryGetValue("NServiceBus.FlowControl.Endpoint", out var endpoint)
            || !headers.TryGetValue("NServiceBus.FlowControl.Address", out var address)
            || !headers.TryGetValue("NServiceBus.FlowControl.SessionId", out var sessionId))
        {
            return next();
        }
        if (sessionId != currentSessionId)
        {
            return Task.CompletedTask;
        }
        var ack = long.Parse(ackString);
        flowManager.Acknowledge(address, ack);
        return Task.CompletedTask;
    }
    #endregion

    FlowManager flowManager;
    string currentSessionId;
}