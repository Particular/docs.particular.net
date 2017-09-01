---
title: MSMQ Dead Letter Queue Custom Check
reviewed: 2017-09-01
component: MsmqTransport
related:
- transports/msmq
- transports/msmq/dead-letter-queues
- servicecontrol/plugins/custom-checks
---


## Prerequisites

- Ensure that MSMQ has been installed.
- Ensure that ServiceControl and ServicePulse have been installed.


## Code walk-through

This sample shows using a custom check to monitor the MSMQ Dead Letter Queue. The custom check runs once every minute. If there are no messages in the MSMQ Dead Letter Queue then the custom check reports success. Otherwise, the custom check reports failure and includes the number of messages in the MSMQ Dead Letter Queue.


### Configuration

Configure the custom checks plugin with the location of the ServiceControl input queue.

snippet: configure-custom-checks

### Check Dead Letter Queue Length

snippet: check-dead-letter-queue

