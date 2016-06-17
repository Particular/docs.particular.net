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

    #region ProcessACKs
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        string ackString;
        string instanceString;
        string endpoint;
        string sessionId;
        string key;
        var headers = context.MessageHeaders;
        if (!headers.TryGetValue("NServiceBus.FlowControl.ACK", out ackString)
            || !headers.TryGetValue("NServiceBus.FlowControl.Key", out key)
            || !headers.TryGetValue("NServiceBus.FlowControl.Endpoint", out endpoint)
            || !headers.TryGetValue("NServiceBus.FlowControl.Instance", out instanceString)
            || !headers.TryGetValue("NServiceBus.FlowControl.SessionId", out sessionId))
        {
            return next();
        }
        if (sessionId != currentSessionId)
        {
            return Completed;
        }
        var instanceHash = instanceString.GetHashCode();
        var ack = long.Parse(ackString);
        flowManager.Acknowledge(key, endpoint, instanceHash, ack);
        return Completed;
    }
    #endregion

    FlowManager flowManager;
    string currentSessionId;
    static Task<int> Completed = Task.FromResult(0);
}