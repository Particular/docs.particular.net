---
title: Fair Load Distribution
summary: Implementing fair load distribution for heterogeneous scaled-out endpoints
component: Core
reviewed: 2020-05-25
---

This sample demonstrates how to extend the NServiceBus routing model with a custom distribution strategy. Distribution strategies replace the distributor feature as a scale-out mechanism for MSMQ. The default built-in distribution strategy uses a simple round-robin approach. This sample shows a more sophisticated distribution strategy that keeps the queue length of all load-balanced instances equal, allowing for the effective use of non-heterogeneous worker clusters.

## Prerequisites

Make sure MSMQ is installed and configured as described in the [MSMQ transport - MSMQ configuration](/transports/msmq/#msmq-configuration) section.

## Running the project

 1. Start all the projects by pressing <kbd>F5</kbd>.
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window.
 1. Hold down <kbd>enter</kbd> for a few seconds to send many messages.

### Verifying that the sample works correctly

 1. Notice more messages are being sent to Server.1 than to Server.2
 1. Use an [MSMQ viewing tool](/transports/msmq/viewing-message-content-in-msmq.md) to inspect the queue contents.
 1. Keep pressing <kbd>enter</kbd> and observe the number of messages in the Server.1 and Server.2 queues.
 1. Notice that although Server.2 processes messages 50% slower than Server.1, the number of messages in both queues are almost equal.

## Code walk-through

This sample contains four projects.

### Client

The Client application submits the orders for processing by the server. Client routing is configured to send `PlaceOrder` commands to two instances of the `Server` endpoint:

snippet: Routing

The following code enables fair load distribution (as opposed to the default round-robin algorithm):

snippet: FairDistributionClient

### Server

The Server application processes the `PlaceOrder` commands. On the server-side, there is no need to register the custom distribution strategy:

snippet: FairDistributionServer

NOTE: In real-world scenarios, NServiceBus endpoints are scaled out by deploying multiple physical instances of a single logical endpoint to multiple machines. For simplicity, the scale-out in this sample is simulated by having two separate projects, Server and Server2.

### Shared project

The shared project contains definitions for messages and the custom routing logic.

### Marking messages

All outgoing messages are marked with sequence numbers to keep track of how many messages are in-flight at any given point in time. Separate sequences are maintained for each destination queue. The number of in-flight messages is estimated as the difference between the last sequence number sent and the last sequence number acknowledged.

snippet: MarkMessages

### Acknowledging message delivery

After receiving every `N` messages, the downstream endpoint instance sends back an acknowledgment (ACK) message containing the highest sequence number it has processed so far. The ACK messages are sent separately to each upstream endpoint instance.

snippet: ProcessMarkers

### Processing acknowledgments

When the ACK message is received, the upstream endpoint calculates the number of messages that are currently in-flight between itself and the downstream endpoint.

snippet: ProcessACKs

### Smart routing

The calculated number of in-flight messages is then used to distribute messages in such a way that all instances of the downstream endpoint have roughly the same number of messages in their input queues. That way, the load is adjusted to the capacity of the given instance (e.g., instances running on weaker machines process fewer messages). As a result, no instance gets overwhelmed, and no instance is underutilized when work is available.

Determining an optimal value for `N` (i.e., the number of messages between ACKs) may involve some trial and error. The bigger the `N` value, the bigger the difference between input queue lengths. On the other hand, lower `N` values cause more traffic as more ACKs are being sent upstream.

snippet: GetLeastBusy

## Real-world deployment

For the sake of simplicity, all the endpoints in this sample run on a single machine. In real-world scenarios, it is usually best to run each instance on a separate virtual machine. In this case, the instance mapping file would contain `machine` attributes mapping instances to their machines' hostnames instead of `queue` attributes used to run more than one instance on a single box.