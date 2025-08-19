---
title: MSMQ Subscription Persistence usage
reviewed: 2024-06-18
component: MsmqPersistence
related:
- persistence/msmq
- transports/msmq/delayed-delivery
- nservicebus/messaging/delayed-delivery
---

This sample shows basic usage of MSMQ as storage for subscriptions.


## Prerequisites

Ensure that MSMQ has been installed.


## Code walk-through

The application publishes an empty event to itself and writes to the console when the event is received.


### Configuration

snippet: ConfigureMsmqPersistenceEndpoint


> [!NOTE]
> MSMQ Persistence [only supports subscriptions](/persistence/msmq/). A [different persistence mechanism is required for delayed delivery](/transports/msmq/delayed-delivery.md) since MSMQ doesn't have native delayed delivery support.
