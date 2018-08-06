---
title: Multiple ServiceControl Instances - Communication
reviewed: 2018-08-06
component: ServiceControl
---

This page describes the communication methods between ServiceControl instances within a cluster.

## Event-driven interactions

The first category of communication patterns is event-driven interaction. The master node subscribes to events published by slave nodes using the transport's publish/subscribe mechanism.

### NewEndpointDetected

A `NewEndpointDetected` event is published when a node's Heartbeat component detects an endpoint it has not previously seen. The master node subscribes to this event in order to show the endpoints that have not enabled the Heartbeat plugin.

### MessageFailureResolvedByRetry

A `MessageFailureResolvedByRetry` event is published when an audit message is detected that contains ServiceControl retry headers. The master node subscribes to this event on order to be able to mark the failed message record as successfully retried. 

## Scatter-gather HTTP interactions

The second category of communication patterns is HTTP-based scatter-gather. In this pattern the master node executes the request locally and in addition to that, fans it out to all registered slave nodes. Then the master node combines all the responses and forwards it back to the client.

### GetKnownEndpointsApi

This API is used by ServiceInsight to show endpoint-based filtering options. 

### GetSagaByIdApi

This API is used by ServiceInsight's saga view.

### ScatterGatherApiMessageView

This category of API calls group all return messages based on certain criteria such as ID, correlation ID or other. It includes the following API calls:

 * `GetAllMessagesApi`
 * `GetAllMessagesForEndpointApi`
 * `MessagesByConversationApi`
 * `SearchApi`
 * `SearchEndpointApi`

Apart from simply aggregating the results from the slave nodes, these calls manipulate certain bits of the response content, namely:
 
 * Rewrite the returned message body URL to include the ID of node that contains a given message record
 * Add an attribute containing the ID of the node that contains a given message record

## Routed HTTP interactions

The third category of communication patterns is based on routing HTTP requests to slave nodes based on node ID present in the query string. The client application (ServiceInsight) always sends the requests to the master node and the master node forwards them to the slaves.

### RetryMessagesApi

When a message is being retried, ServiceInsight includes the node ID in the retry request.

### GetBodyByIdApi

When the ServiceInsight fetches the body of a message it uses the URL provided by ServiceControl which includes the ID of nodes that contains the message record.
