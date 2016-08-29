---
title: Azure Service Bus Error Handling
reviewed: 2016-08-29
tags:
- Azure
- Cloud
- Error Handling
---

## Native Error Handling

Next to the [error handling capabilities offered by NServiceBus](/nservicebus/recoverability/configure-error-handling.md), the Azure Service Bus also provides error handling capabilities of it's own. This document describes the native error handling capabilities and how it can be responded to.

### Azure Service Bus SDK

In a cloud environment exceptions are not really exceptional at all. Given the size and complexity of a cloud environment, connectivity and service side capacity problems are to be expected. For a large part these problems are covered by leveraging the retry behavior on transient exceptions of the Azure Service Bus SDK, the so called `RetryPolicy`.

The following document describes a list of [common exceptions](https://azure.microsoft.com/en-us/documentation/articles/service-bus-messaging-exceptions/) that may be logged by NServiceBus. Usually these exceptions are transient in nature and will be automatically resolved by the retry policy of the Azure Service Bus SDK, or they are resolved at the transport level by reestablishing the connection to the service.

If the problems persist they will eventually trigger the transport's circuit breaker which results in crashing the process with a fatal exception.

#### Broker side exceptions

A subset of these exceptions originate from within the ASB service itself. These exceptions can be recognized by a value in the exception message called `TrackingId`. Usually these exceptions are also transient in nature and will disappear after a few minutes. 

Note: If broker side exceptions persist the `TrackingId` value can be used to contact Azure Support, so they can figure out what is going wrong inside the broker.

#### Message Size problems

A peculiar behavior of the ASB SDK is how it reports on actual message size. It can only do so accurately after a message has been sent. Before sending the reported message size only covers the body section and not the final size that would include header and serialization overhead. This can obviously lead to unexpected results when trying to send a message.

If the application is intended to send large messages, it should always leverage the [NServiceBus Databus](/nservicebus/messaging/databus/) in it's design. But if the application sends regular messages that flirt with the maximum message size than extra precaution may be necessary.

The transport deals with this problem for a large part by performing an [estimated batch size calculation](batching.md#batching-messages-sent-from-a-handler-padding-and-estimated-batch-size-calculation) that includes both body and headers as well as a percentage for padding due to serialization. 

But even with this calculation in place, there is no guarantee that a message will eventually fit the limits after serialization. And if it doesn't then the ASB SDK will throw a `MessageSizeExceededException`. The transport will catch this exception and invoke an instance of `IHandleOversizedBrokeredMessages`, which has a default implementation that throws a `MessageTooLargeException` suggesting to use the databus instead. If the application wants to [implement custom handling of oversized sends](oversized-sends.md) it can provide it's own implementation of this interface.

### Dead Letter Queue

Next to the client side error handling concerns described above, the broker itself also has several error handling capabilities:
* When the client fails to process a message in a timely fashion.
* When message expire (optional)
* When subscription filters do not match (optional)

It will respond to any of these conditions by moving the message to a dead letter queue, which is a sub queue below a messaging entity. Each regular queue or subscription has such a dead letter queue.

From a management perspective this behavior is not ideal though, as it requires operations to check multiple dead letter queue instances for failed messages. One solution could be to set up [dead letter queue forwarding](dlq-forwarding.md) to a centralized error or dead letter queue.



