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

    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next, Func<IRoutingContext, Task> fork)
    {
        await next().ConfigureAwait(false);

        string markerString;
        string controlAddress;
        if (!context.Message.Headers.TryGetValue("NServiceBus.FlowControl.Marker", out markerString) ||
            !context.Message.Headers.TryGetValue("NServiceBus.FlowControl.ControlAddress", out controlAddress))
        {
            return;
        }

        var marker = long.Parse(markerString);
        long lastAcknowledged;
        if (!acknowledgedMarkers.TryGetValue(controlAddress, out lastAcknowledged) || marker - lastAcknowledged > maxAckBatchSize) //ACK every N-th message
        {
            var lastAcked = acknowledgedMarkers.AddOrUpdate(controlAddress, _ => marker, (_, v) => marker > v ? marker : v);
            await SendAcknowledgement(context, fork, lastAcked, controlAddress).ConfigureAwait(false);
        }
    }

    async Task SendAcknowledgement(ITransportReceiveContext context, Func<IRoutingContext, Task> fork, long lastAcked, string controlAddress)
    {
        var ackHeaders = new Dictionary<string, string>();
        ackHeaders["NServiceBus.FlowControl.Marker"] = lastAcked.ToString();
        ackHeaders["NServiceBus.FlowControl.Endpoint"] = endpointName;
        ackHeaders["NServiceBus.FlowControl.Instance"] = instanceString;
        ackHeaders["NServiceBus.FlowControl.ControlAddress"] = controlAddress;
        var ackMessage = new OutgoingMessage(Guid.NewGuid().ToString(), ackHeaders, new byte[0]);
        var ackContext = this.CreateRoutingContext(ackMessage, controlAddress, context);
        await fork(ackContext).ConfigureAwait(false);
    }

    int maxAckBatchSize;
    string endpointName;
    string instanceString;
    ConcurrentDictionary<string, long> acknowledgedMarkers = new ConcurrentDictionary<string, long>();
}
