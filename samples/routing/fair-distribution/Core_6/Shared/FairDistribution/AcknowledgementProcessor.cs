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
        string ackString;
        string address;
        string endpoint;
        string sessionId;
        var headers = context.MessageHeaders;
        if (!headers.TryGetValue("NServiceBus.FlowControl.ACK", out ackString)
            || !headers.TryGetValue("NServiceBus.FlowControl.Endpoint", out endpoint)
            || !headers.TryGetValue("NServiceBus.FlowControl.Address", out address)
            || !headers.TryGetValue("NServiceBus.FlowControl.SessionId", out sessionId))
        {
            return next();
        }
        if (sessionId != currentSessionId)
        {
            return Task.FromResult(0);
        }
        var ack = long.Parse(ackString);
        flowManager.Acknowledge(address, ack);
        return Task.FromResult(0);
    }
    #endregion

    FlowManager flowManager;
    string currentSessionId;
}