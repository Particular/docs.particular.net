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
> MSMQ Persistence [only supports subscriptions](https://docs.particular.net/persistence/msmq/). A [different persistence is required for delayed delivery](https://docs.particular.net/transports/msmq/delayed-delivery) (e.g. for [saga timeouts](https://docs.particular.net/nservicebus/sagas/timeouts) or [delayed retries](https://docs.particular.net/nservicebus/recoverability/configure-delayed-retries)) since MSMQ doesn't have native delayed delivery support.
