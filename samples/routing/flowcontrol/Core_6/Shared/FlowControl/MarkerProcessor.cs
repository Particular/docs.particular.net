using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Transports;

class MarkerProcessor : ForkConnector<ITransportReceiveContext, IRoutingContext>
{

    public MarkerProcessor(EndpointInstance endpointInstance, int maxAckBatchSize)
    {
        this.maxAckBatchSize = maxAckBatchSize;
        endpointName = endpointInstance.Endpoint.ToString();
        instanceString = endpointInstance.ToString();
    }

    #region ProcessMarkers
    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next, Func<IRoutingContext, Task> fork)
    {
        await next().ConfigureAwait(false);

        string sessionId;
        string markerString;
        string controlAddress;
        string key;
        if (!context.Message.Headers.TryGetValue("NServiceBus.FlowControl.Marker", out markerString) ||
            !context.Message.Headers.TryGetValue("NServiceBus.FlowControl.Key", out key) ||
            !context.Message.Headers.TryGetValue("NServiceBus.FlowControl.ControlAddress", out controlAddress) ||
            !context.Message.Headers.TryGetValue("NServiceBus.FlowControl.SessionId", out sessionId))
        {
            return;
        }

        var marker = long.Parse(markerString);
        long lastAcknowledged;
        if (!acknowledgedMarkers.TryGetValue(controlAddress, out lastAcknowledged) || marker - lastAcknowledged > maxAckBatchSize) //ACK every N messages
        {
            var lastAcked = acknowledgedMarkers.AddOrUpdate(controlAddress, _ => marker, (_, v) => marker > v ? marker : v);
            await SendAcknowledgement(context, fork, lastAcked, controlAddress, sessionId, key).ConfigureAwait(false);
        }
    }
    #endregion

    Task SendAcknowledgement(ITransportReceiveContext context, Func<IRoutingContext, Task> fork, long lastAcked, string controlAddress, string sessionId, string key)
    {
        var ackHeaders = new Dictionary<string, string>
        {
            ["NServiceBus.FlowControl.ACK"] = lastAcked.ToString(),
            ["NServiceBus.FlowControl.Key"] = key,
            ["NServiceBus.FlowControl.Endpoint"] = endpointName,
            ["NServiceBus.FlowControl.Instance"] = instanceString,
            ["NServiceBus.FlowControl.SessionId"] = sessionId
        };
        var ackMessage = new OutgoingMessage(Guid.NewGuid().ToString(), ackHeaders, new byte[0]);
        var ackContext = this.CreateRoutingContext(ackMessage, controlAddress, context);
        ackContext.Extensions.Set(new MessageMarker.SkipMarking());
        return fork(ackContext);
    }

    int maxAckBatchSize;
    string endpointName;
    string instanceString;
    ConcurrentDictionary<string, long> acknowledgedMarkers = new ConcurrentDictionary<string, long>();
}
