---
title: Azure Service Bus DLQ Forwarding
versions: '[7,)'
reviewed: 2016-08-29
tags:
- Azure
- Cloud
- Error Handling
---

When Azure Service Bus service detects that an endpoint cannot process messages, or when it has been configured not to discard expired or filtered messages, then it will move those messages to a dead letter queue associated with the entity, originally contained those messages.

From a management perspective, this is less than ideal when operations staff has to monitor a multitude of entities for dead lettered or discarded messages.

A better approach might be to consolidate all those messages into a single centralized queue by enabling forwarding on the dead letter queue of each entity, except for the destination entity itself.

The Azure Service Bus transport supports this capability; it allows to configure forwarding for dead letter queues when it creates entities on behalf of the application.

### Configuration

To set up dead letter queue forwarding, the API provides a configuration method called `ForwardDeadLetteredMessagesTo(string)` on both the `Queues` and `Subscriptions` extension points.

When dead letter forwarding is enabled, it will be enabled for all dead letter queues associated with the queues created by the transport. The API allows making exceptions by using an overload on the configuration API. It is advised to set up forwarding for all entities except for the error queue, audit queue and centralized dead letter queue.

Snippet: asb-configure-dead-letter-queue-forwarding

