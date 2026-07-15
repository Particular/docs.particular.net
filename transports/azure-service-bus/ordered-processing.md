---
title: Support for Azure Service Bus Sessions
component: ASBS
related:
 - samples/azure-service-bus-netstandard/send-reply
 - samples/azure-service-bus-netstandard/native-integration
 - samples/azure-service-bus-netstandard/native-integration-pub-sub
 - samples/azure-service-bus-netstandard/options
 - samples/azure-service-bus-netstandard/send-receive-with-nservicebus
 - samples/azure-service-bus-netstandard/topology-migration
 - samples/showcase/loan-broker-showcase
 - architecture/azure
reviewed: 2026-07-15
---

The Azure Service Bus transport supports ordered processing of messages using [Azure Service Bus Sessions](https://learn.microsoft.com/en-us/azure/service-bus-messaging/message-sessions). 

The ordered processing of messages feature in Azure Service Bus transport is supported for send-receive and publish/subscribe.

## Supported Transaction Modes

Azure Service Bus transport with ordered processing supports `ReceiveOnly` and `SendsAtomicWithReceive` transaction modes. The transaction mode `None` is unsupported.

## Send-Receive

To enable ordered processing of messages, sessions must be enabled on the receiving endpoint.

```
 var transport = new AzureServiceBusTransport(connectionString, topology) { EnableSessions = true };
```

Commands that must be processed in order must also have a `SessionId` specified. The `SessionId` can be set using the `SetSessionId` extension method on `SendOptions`.

```
var sendOptions = new SendOptions();
sendOptions.SetSessionId("unique-session-id")
```

The `SessionId` must be a unique identifier set by the sender. A set of messages that should be processed in order must have the same `SessionId` specified.

It is possible that a sender does not need to process messages in order. In such a case, there is no need to set `EnableSessions` to `true` on the sender/publisher side. A sender that does not process messages in order can send messages with a `SessionId` to be processed by a different receiver endpoint that processes messages in order.

## Publish/Subscribe

To enable ordered processing of messages, sessions must be enabled on a subscribing endpoint.

```
 var transport = new AzureServiceBusTransport(connectionString, topology) { EnableSessions = true };
```

Events that must be processed in order must also have a `SessionId` specified. The `SessionId` can be set using the `SetSessionId` extension method on `PublishOptions`.

The `SessionId` must be a unique identifier set by the publisher.  A set of events that should be processed in order must have the same `SessionId` specified.

It is possible that a publisher does not process messages in order. In such a case there is no need to set `EnableSessions` to `true` on the publisher side. A publisher that does not process messages in order can publish messages with a `SessionId` to be processed by a different subscriber endpoint that processes messages in order.

### Subscription Forwarder

TODO: Do we need to mention this?

## Endpoint set up

Setting the `EnableSessions` to `true` enables ordered processing on the endpoint. It is expected that all the messages processed by the endpoint are to be processed in order. It is therefore recommended to use separate endpoints for messages that require ordered processing and messages that do not require ordered processing. Ordered processing of messages have an impact on the throughput of the endpoint. Having separate endpoints for messages that requires ordered processing and messages that do not isolates the impact to only endpoints that process in order.

The input queue for such an endpoint when created would have the `Requires Session` flag enabled in Azure Service Bus. All messages routed to the endpoint are expected to have a `SessionId`. Messages to be routed to the endpoint, that does not have a `SessionId` will be rejected by Azure Service Bus.

## Retries