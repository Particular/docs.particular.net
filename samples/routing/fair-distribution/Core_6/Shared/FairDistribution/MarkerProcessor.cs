using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

class MarkerProcessor :
    ForkConnector<ITransportReceiveContext, IRoutingContext>
{
    public MarkerProcessor(string endpoint, string localAddress, int maxAckBatchSize)
    {
        this.endpoint = endpoint;
        this.localAddress = localAddress;
        this.maxAckBatchSize = maxAckBatchSize;
    }

    #region ProcessMarkers
    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next, Func<IRoutingContext, Task> fork)
    {
        await next()
            .ConfigureAwait(false);

        var headers = context.Message.Headers;
        if (!headers.TryGetValue("NServiceBus.FlowControl.Marker", out var markerString) ||
            !headers.TryGetValue("NServiceBus.FlowControl.ControlAddress", out var controlAddress) ||
            !headers.TryGetValue("NServiceBus.FlowControl.SessionId", out var sessionId))
        {
            return;
        }

        var marker = long.Parse(markerString);
        var tracker = acknowledgedMarkers.AddOrUpdate(
            key: controlAddress,
            addValueFactory: k => new MarkerTracker(sessionId, marker),
            updateValueFactory: (k, v) => v.OnNewMarker(sessionId, marker));
        if (tracker.ShouldAcknowledge(maxAckBatchSize))
        {
            await SendAcknowledgement(context, fork, tracker.Marker, controlAddress, sessionId)
                .ConfigureAwait(false);
        }
    }
    #endregion

    Task SendAcknowledgement(ITransportReceiveContext context, Func<IRoutingContext, Task> fork, long lastAcked, string controlAddress, string sessionId)
    {
        var ackHeaders = new Dictionary<string, string>
        {
            ["NServiceBus.FlowControl.ACK"] = lastAcked.ToString(),
            ["NServiceBus.FlowControl.Endpoint"] = endpoint,
            ["NServiceBus.FlowControl.Address"] = localAddress,
            ["NServiceBus.FlowControl.SessionId"] = sessionId
        };
        var ackMessage = new OutgoingMessage(Guid.NewGuid().ToString(), ackHeaders, Array.Empty<byte>());
        var ackContext = this.CreateRoutingContext(ackMessage, controlAddress, context);
        return fork(ackContext);
    }

    string endpoint;
    int maxAckBatchSize;
    string localAddress;
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
