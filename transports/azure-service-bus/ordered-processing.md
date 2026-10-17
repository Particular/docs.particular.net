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
 - samples/azure-service-bus-netstandard/ordered-delivery-sample
 - samples/showcase/loan-broker-showcase
 - architecture/azure
reviewed: 2026-07-15
---

The Azure Service Bus transport supports ordered processing of messages using [Azure Service Bus Sessions](https://learn.microsoft.com/en-us/azure/service-bus-messaging/message-sessions).

The ordered processing of messages feature in Azure Service Bus transport is supported for send-receive and publish/subscribe.

## Supported Transaction Modes

Azure Service Bus transport with ordered processing supports `ReceiveOnly` and `SendsAtomicWithReceive` transaction modes. The transaction mode `None` is unsupported.

## Enabling ordered processing for Azure Service Bus Transport

Use the `EnableSessions` property of the `AzureServiceBusTransport` to enable ordered processing on the endpoint.

```
 var transport = new AzureServiceBusTransport(connectionString, topology) { Ena
bleSessions = true };
```

An endpoint that is sessions-enabled can only accept messages with a `SessionId` across all of its handlers.

## Send-Receive

To enable ordered processing of messages, sessions must be enabled on the receiving endpoint.

Messages that must be processed in order must also have a `SessionId` specified. The `SessionId` can be set using the `SetSessionId` extension method on `SendOptions`.

```
var sendOptions = new SendOptions();
sendOptions.SetSessionId("unique-session-id");
```

The `SessionId` must be a unique identifier set by the sender. A set of messages that should be processed in order must have the same `SessionId` specified.

It is not required that the sender must be sessions-enabled to send messages to another endpoint for ordered processing.

A sender that does not process messages in order can send messages to an endpoint that process message in an ordered fashion. In such a case, there is no need to set `EnableSessions` to `true` on the sending endpoint. However, any messages that needs ordered processing should have a `SessionId`.

## Publish/Subscribe

To enable ordered processing of messages, sessions must be enabled on a subscribing endpoint.

```
 var transport = new AzureServiceBusTransport(connectionString, topology) { EnableSessions = true };
```

Events that must be processed in order must also have a `SessionId` specified. The `SessionId` can be set using the `SetSessionId` extension method on `PublishOptions`.

```
var publishOptions = new PublishOptions();
publishOptions.SetSessionId("unique-session-id");
```

The `SessionId` must be a unique identifier set by the publisher.  A set of events that should be processed in order must have the same `SessionId` specified.

A publisher that does not process messages in order can publish events to be consumed in an ordered fashion. In such a case, there is no need to set `EnableSessions` to `true` on the publishing endpoint. However, any events that needs ordered processing should have a `SessionId`.

### Endpoint set up

With `EnableSessions` to `true` on an endpoint, it is expected that all the messages processed by the endpoint are to be processed in order. It is therefore recommended to use separate endpoints for messages that require ordered processing and messages that do not require ordered processing.

A message with a `SessionId` can be processed by an endpoint that is not sessions-enabled, but without the guarantee of ordered processing. In a case where the same message needs to be processed by an ordered and an unordered endpoint, the message should be send/published with a `SessionId`. But there should be two endpoints - one for ordered processing and another for processing without the ordering guarantee. 

## Retries

Immediate retries are supported and does not break processing order. 

Delayed retries can break the processing order. So sessions-enabled endpoint must disable delayed retries.

```
endpointConfiguration.Recoverability().Delayed(d => d.NumberOfRetries(0));
```

## Error messages

When immediate retries are exhausted, messages are moved into the error queue. This can break the processing order. Retrying a message from the error queue or the DLQ does not guarantee that the message will be enqueued in its original position before it was moved to the error queue.

### SessionState API