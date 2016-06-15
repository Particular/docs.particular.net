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
    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        string ackString;
        string instanceString;
        string endpoint;
        string controlAddress;
        string sessionId;

        var headers = context.MessageHeaders;
        if (!headers.TryGetValue("NServiceBus.FlowControl.Marker", out ackString)
            || !headers.TryGetValue("NServiceBus.FlowControl.Endpoint", out endpoint)
            || !headers.TryGetValue("NServiceBus.FlowControl.Instance", out instanceString)
            || !headers.TryGetValue("NServiceBus.FlowControl.ControlAddress", out controlAddress)
            || !headers.TryGetValue("NServiceBus.FlowControl.SessionId", out sessionId))
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
    #endregion

    FlowManager flowManager;
    string currentSessionId;
}