---
title: Simple MSMQ Transport usage
reviewed: 2017-06-20
component: MsmqTransport
related:
- nservicebus/msmq
---


## Prerequisites

Ensure that MSMQ has been installed.


## Code walk-through

This sample shows basic usage of MSMQ as a transport for NServiceBus. The application sends an empty message to itself, via the MSMQ transport, and writes to the console when the message is received.


### Configuration

snippet: ConfigureMsmqEndpoint

Other settings such as DeadLetterQueue, Journal behavior, etc., can also be configured via the [connection string](/nservicebus/msmq/connection-strings.md).

