---
title: MSMQ Subscription Persistence usage
reviewed:
component: MsmqPersistence
related:
- persistence/msmq
---


## Prerequisites

Ensure that MSMQ has been installed.


## Code walk-through

This sample shows basic usage of MSMQ as storage for subscriptions. The application publish an empty message to itself, via the MSMQ transport, and writes to the console when the message is received. 

Note that we're overriding the default queue name, `NServiceBus.Subscriptions`, with `{EndpointName}.Subscriptions` to avoid sharing the subscription storage queue with other endpoints on the same machine.

### Configuration

snippet: ConfigureMsmqPersistenceEndpoint