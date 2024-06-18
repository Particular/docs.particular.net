---
title: MSMQ Subscription Persistence usage
reviewed: 2024-06-18
component: MsmqPersistence
related:
- persistence/msmq
---

This sample shows basic usage of MSMQ as storage for subscriptions.


## Prerequisites

Ensure that MSMQ has been installed.


## Code walk-through

The application publishes an empty event to itself and writes to the console when the event is received.


### Configuration

snippet: ConfigureMsmqPersistenceEndpoint


> [!NOTE]
> MSMQ Persistence only supports subscription storage. A different storage type is required for Timeouts since MSMQ doesn't have native timeout support.
