---
title: Endpoint Managed Entity Queue Permissions
summary: Explains queue-scoped permissions needed by an endpoint running in Azure Service Bus
component: ASBS
reviewed: 2025-10-14
---

It is common practice to limit [Azure Service Bus connection permissions at the queue level when using Managed Entities](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-managed-service-identity#resource-scope). 

The following list maps endpoint features to fine-grained permissions and when they are needed for an endpoint when using queue-scoped permissions:

- `Azure Service Bus Data Receiver` to the endpoint's queue is required to process messages.
- `Azure Service Bus Data Sender` to the endpoint's queue is required for:
  - [Delayed retries](/nservicebus/recoverability/#delayed-retries)
  - [Saga Timeouts](/nservicebus/sagas/timeouts.md)
  - [Transactional Session](/nservicebus/transactional-session/)
  - [`.SendLocal()`](/nservicebus/messaging/send-a-message.md#sending-to-self)
- `Azure Service Bus Data Sender` to every [queue the endpoint sends a command to](/nservicebus/messaging/routing.md#command-routing).
- `Azure Service Bus Data Sender` to every [queue the endpoint replies to](/nservicebus/messaging/reply-to-a-message.md).
- `Azure Service Bus Data Sender` to every [topic the endpoint publishes an event to](/transports/azure-service-bus/topology.md).
#if-version [,3)
- `Microsoft.ServiceBus/namespaces/topics/subscriptions/write` is required for [every topic](/transports/azure-service-bus/topology.md) the endpoint [handles events](/nservicebus/messaging/publish-subscribe/publish-handle-event.md#handling-an-event) from when using [automatic subscriptions (default)](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md).
#end-if
- `Azure Service Bus Data Sender` to the [transactional session remote processor](/nservicebus/transactional-session/#remote-processor) when configured.
- `Azure Service Bus Data Sender` to the [error queue](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address-using-code).
- `Azure Service Bus Data Sender` to the audit queue when [auditing](/nservicebus/operations/auditing.md#configuring-auditing) is enabled.
- `Azure Service Bus Data Sender` to the metrics queue when ServiceControl [metrics](/monitoring/metrics/install-plugin.md#configuration) are enabled.
- `Azure Service Bus Data Sender` to the ServiceControl queue when [heartbeats](/monitoring/heartbeats/install-plugin.md) or [custom checks](/monitoring/custom-checks/install-plugin.md) are being used.
- `Azure Service Bus Data Sender` to any queue the endpoint [forwards to](/nservicebus/messaging/forwarding.md).
- `Azure Service Bus Data Receiver` is required for every [satellite queue](/nservicebus/satellites/) created.

include: managed-access-rights
