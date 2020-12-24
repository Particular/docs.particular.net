---
title: Error Handling
summary: Describes the native error-handling capabilities of Azure Service Bus
reviewed: 2020-12-24
component: ASB
redirects:
 - nservicebus/azure-service-bus/error-handling
 - transports/azure-service-bus/error-handling
---

include: legacy-asb-warning

Next to [recoverability](/nservicebus/recoverability/), the Azure Service Bus also provides error handling capabilities of its own. This document describes the native error-handling capabilities and compensating actions should there be a need.


## Azure Service Bus SDK

In a cloud environment, exceptions are common. Given the size and complexity of a cloud environment, connectivity and service side capacity problems are to be expected. For a large part, these problems are covered by leveraging the retry behavior on transient exceptions of the Azure Service Bus SDK, known as [`RetryPolicy`](https://docs.microsoft.com/en-us/azure/architecture/best-practices/retry-service-specific).

The following document describes a list of [common exceptions](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-exceptions), coming from the broker or connectivity to the broker, that may be logged by NServiceBus. Usually, these exceptions are transient in their nature and will be automatically resolved by the retry policy of the Azure Service Bus SDK, or they are resolved at the transport level by reestablishing the connection to the service.

If the exceptions persist, they will eventually trigger the transport's circuit breaker which results in a [critical error](/nservicebus/hosting/critical-errors.md) which may stop the process with a fatal exception.


### Broker side exceptions

A subset of these exceptions originate from within the Azure Service Bus service itself. These exceptions can be recognized by the `TrackingId` field and its value in the exception message. Usually, these exceptions are also transient in nature and will cease occurring after a few minutes. 

Note: If broker side exceptions persist the `TrackingId` value can be used to contact Azure Support to investigate further with Microsoft and Azure Service Bus teams.


### Message size problems

A peculiar behavior of the Azure Service Bus SDK is how it reports on message size. It can only do so accurately after a message has been sent. Before sending the reported message size only covers the body section and not the final size that would include header and serialization overhead. This can obviously lead to unexpected results when trying to send a message.

If the application is intended to send large messages, it should leverage the [data bus](/nservicebus/messaging/databus/) to send large payloads. In the scenario where an application sends regular messages that borderline with the maximum message size than extra precaution may be necessary.

The transport deals with this problem for a large part by performing an [estimated batch size calculation](batching.md#batching-messages-sent-from-a-handler-padding-and-estimated-batch-size-calculation) that includes both body and headers as well as a percentage for padding due to serialization.

Even with this calculation in place, there is no guarantee that a message will eventually fit the limits after serialization. If it doesn't, then the Azure Service Bus client will throw a `MessageSizeExceededException` similar to the following:

> Microsoft.Azure.ServiceBus.MessageSizeExceededException: The received message (delivery-id:0, size:262626 bytes) exceeds the limit (262144 bytes) currently allowed on the link.

The transport will catch this exception and invoke an instance of `IHandleOversizedBrokeredMessages`, which has a default implementation that throws a `MessageTooLargeException` suggesting to use the data bus feature. A [custom implementation](oversized-sends.md) for handling of oversized sends can be provided.

1. Go to Premium which allows 1MB messages
2. Use the [data bus](/nservicebus/messaging/databus/) feature
3. Transmit smaller message bodies

NOTE: Sometimes this exception can occur when forwarding a messsage to the audit or error queue as the headers are expanded with metadata which can result in the message size limit being exceeded. Ensure that any message will have enough remaining space in the header to allow for this header data expansion.


## Dead letter queue

Next to the client-side error handling concerns described above, the broker itself also has several error handling capabilities:

 * When the client fails to process a message and exceeds a maximum number of attempts.
 * When message expires (optional).
 * When subscription filters do not match (optional).

Broker will respond to any of these conditions by moving the message to a dead letter queue, which is a sub-queue below a messaging entity. Each regular queue or subscription has a dead letter queue.

From a management perspective, it is not ideal for an operations staff to monitor a multitude of entities for dead lettered or discarded messages in multiple locations. One solution could be to set up [dead letter queue forwarding](dlq-forwarding.md) to a centralized error or dead letter queue.
