---
title: Error Handling
reviewed: 2017-05-05
component: ASB
tags:
- Azure
- Error Handling
redirects:
 - nservicebus/azure-service-bus/error-handling
---

Next to [Recoverability](/nservicebus/recoverability/), the Azure Service Bus also provides error handling capabilities of its own. This document describes the native error handling capabilities and compensating actions should there be a need.


## Azure Service Bus SDK

In a cloud environment, exceptions are common. Given the size and complexity of a cloud environment, connectivity and service side capacity problems are to be expected. For a large part, these problems are covered by leveraging the retry behavior on transient exceptions of the Azure Service Bus SDK, known as [`RetryPolicy`](https://docs.microsoft.com/en-us/azure/architecture/best-practices/retry-service-specific).

The following document describes a list of [common exceptions](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-exceptions), coming from the broker or connectivity to the broker, that may be logged by NServiceBus. Usually, these exceptions are transient in their nature and will be automatically resolved by the retry policy of the Azure Service Bus SDK, or they are resolved at the transport level by reestablishing the connection to the service.

If the exceptions persist, they will eventually trigger the transport's circuit breaker which results in crashing the process with a fatal exception.


### Broker side exceptions

A subset of these exceptions originate from within the Azure Service Bus service itself. These exceptions can be recognized by the `TrackingId` field and its value in the exception message. Usually, these exceptions are also transient in nature and will cease occurring after a few minutes. 

Note: If broker side exceptions persist the `TrackingId` value can be used to contact Azure Support to investigate further with Microsoft and Azure Service Bus teams.


### Message Size problems

A peculiar behavior of the Azure Service Bus SDK is how it reports on message size. It can only do so accurately after a message has been sent. Before sending the reported message size only covers the body section and not the final size that would include header and serialization overhead. This can obviously lead to unexpected results when trying to send a message.

If the application is intended to send large messages, it should leverage the [DataBus](/nservicebus/messaging/databus/) to send large payloads. In the scenario where an application sends regular messages that borderline with the maximum message size than extra precaution may be necessary.

The transport deals with this problem for a large part by performing an [estimated batch size calculation](batching.md#batching-messages-sent-from-a-handler-padding-and-estimated-batch-size-calculation) that includes both body and headers as well as a percentage for padding due to serialization.

Even with this calculation in place, there is no guarantee that a message will eventually fit the limits after serialization. And if it doesn't then the Azure Service Bus client will throw a `MessageSizeExceededException`. The transport will catch this exception and invoke an instance of `IHandleOversizedBrokeredMessages`, which has a default implementation that throws a `MessageTooLargeException` suggesting to use the DataBus feature. A [custom implementation](oversized-sends.md) for handling of oversized sends can be provided.


## Dead Letter Queue

Next to the client-side error handling concerns described above, the broker itself also has several error handling capabilities:

 * When the client fails to process a message and exceeds a maximum number of attempts.
 * When message expires (optional).
 * When subscription filters do not match (optional).

Broker will respond to any of these conditions by moving the message to a dead letter queue, which is a sub-queue below a messaging entity. Each regular queue or subscription has a dead letter queue.

From a management perspective, it is not ideal for an operations staff to monitor a multitude of entities for dead lettered or discarded messages in multiple locations. One solution could be to set up [dead letter queue forwarding](dlq-forwarding.md) to a centralized error or dead letter queue.
