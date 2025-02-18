---
title: Simple MSMQ Transport usage
reviewed: 2025-01-20
component: MsmqTransport
related:
- transports/msmq
---

## Prerequisites

Ensure that [MSMQ has been installed](/transports/msmq#msmq-configuration).

## Code walk-through

This sample shows basic usage of MSMQ as a transport for NServiceBus. The application sends an empty message to itself and writes to the console when the message is received.

### Configuration

snippet: ConfigureMsmqEndpoint

Other settings such as usage of dead-letter queues, journaling, etc., can be configured using the [configuration API](/transports/msmq/transportconfig.md).
