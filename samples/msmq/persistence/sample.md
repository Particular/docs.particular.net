---
title: MSMQ Subscription Persistence usage
reviewed: 2017-09-22
component: MsmqPersistence
related:
- persistence/msmq
---

This sample shows basic usage of MSMQ as storage for subscriptions.


## Prerequisites

Ensure that MSMQ has been installed.


## Code walk-through

The application publishes an empty message to itself, via the MSMQ transport, and writes to the console when the message is received. 

Note that the default queue name, `NServiceBus.Subscriptions`, is overridden to avoid sharing the subscription storage queue with other endpoints on the same machine. The queue name is `{EndpointName}.Subscriptions`.


### Configuration

snippet: ConfigureMsmqPersistenceEndpoint


NOTE: MSMQ Persistence only supports subscription storage so another storage is needed for Timeouts since MSMQ doesn't have native timeout support.