---
title: Configuration API Fault Management in V3 and V4
summary: Configuration API Fault Management in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
- V3
- V4
---

NServiceBus has [exception catching and handling logic](/nservicebus/how-do-i-handle-exceptions) of its own which surrounds all calls to user code. When an exception bubbles through to the NServiceBus infrastructure, it rolls back the transaction on a transactional endpoint, causing the message to be returned to the queue, and any messages that the user code tried to send or publish to be undone as well.

At that point, NServiceBus retries to handle that message a configurable number of times (default of 5) and if the message fails on every one of those retries, the message is then moved to the configured error queue.

To activate the fault manager, call the `MessageForwardingInCaseOfFault()` method  of the `Configure` instance.