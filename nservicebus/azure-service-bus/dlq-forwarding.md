---
title: Azure Service Bus DLQ Forwarding
reviewed: 2016-08-29
tags:
- Azure
- Cloud
- Error Handling
---

## Dead Letter Queue Forwarding

When the ASB service detects that an endpoint cannot process messages in a timely fashion, or when it has been configured to not discard expired or filtered messages, then it will move those messages to a dead letter queue associated to the entity that contained those messages.

From a management perspective this is less then ideal as operations people have to monitor a multitude of entities for failed or discarded messages.

A better approach may be to consolidate all those messages into a single centralized queue by enabling forwarding on the dead letter queue of each entity, except for the destination entity itself.

NServiceBus supports this capability, it allows to configure forwarding for dead letter queues when it creates entities on behalf of the application.

### Configuration

In order to set up dead letter queue forwarding, the API provides a configuration method called `ForwardDeadLetteredMessagesTo` on both the `Queues` and `Subscriptions` extension points.

By default the forward happens for the dead letter queue of each queue created, but there is an overload available that allows to exclude entities. Usually it is advised to set up forwarding for all entities except for the error queue, audit queue and centralized dead letter queue.

Snippet: asb-configure-dead-letter-queue-forwarding

