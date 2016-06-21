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
        var tracker = acknowledgedMarkers.AddOrUpdate(controlAddress, k => new MarkerTracker(sessionId, marker), (k, v) => v.OnNewMarker(sessionId, marker));
        if (tracker.ShouldAcknowledge(maxAckBatchSize))
        {
            await SendAcknowledgement(context, fork, tracker.Marker, controlAddress, sessionId, key).ConfigureAwait(false);
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
        return fork(ackContext);
    }

    int maxAckBatchSize;
    string endpointName;
    string instanceString;
    ConcurrentDictionary<string, MarkerTracker> acknowledgedMarkers = new ConcurrentDictionary<string, MarkerTracker>();

    struct MarkerTracker
    {
        string sessionId;
        long marker;
        long previousMarker;

        public MarkerTracker(string sessionId, long marker, long previousMarker = 0)
        {
            this.sessionId = sessionId;
            this.marker = marker;
            this.previousMarker = previousMarker;
        }

        public long Marker => marker;

        public MarkerTracker OnNewMarker(string newSessionId, long newMarker)
        {
            if (newSessionId != sessionId)
            {
                return new MarkerTracker(newSessionId, newMarker);
            }
            if (newMarker <= marker)
            {
                return this;
            }
            return new MarkerTracker(sessionId, newMarker, marker);
        }

        public bool ShouldAcknowledge(long maxAckBatchSize)
        {
            return marker - previousMarker >= maxAckBatchSize;
        }
    }
}
