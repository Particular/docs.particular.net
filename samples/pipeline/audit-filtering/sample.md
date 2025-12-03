---
title: Audit Filter Pipeline Extension
summary: Extending the pipeline to stop certain messages from being audited
reviewed: 2025-10-08
component: Core
related:
  - nservicebus/pipeline
  - nservicebus/operations/auditing
---

This sample demonstrates how to extend the NServiceBus message processing pipeline with custom behaviors to add filters which prevent certain message types from being forwarded to the audit queue.

## Code walk-through

The solution contains a single endpoint with auditing enabled. When it starts, the endpoint sends one `AuditThisMessage` message and one `DoNotAuditThisMessage` message to itself. Both messages are handled by message handlers. However only the `AuditThisMessage` message will be sent to the audit queue. The `DoNotAuditThisMessage` message is filtered out and not sent.

Three behaviors are added to the message processing pipeline to implement the desired filtering logic:

### AuditFilterContextBehavior

This behavior adds a class to the pipeline context's `Extensions` bag. This class can later be accessed by the other behaviors to share state across behaviors. The state must be added early in the pipeline because anything added to `Extensions` after `IIncomingPhysicalMessageContext` is invisible to the `IAuditContext` instance.

snippet: auditFilterContextBehavior

### AuditRulesBehavior

`AuditRulesBehavior` uses the `IIncomingLogicalMessageContext` to inspect the incoming message type and it applies rules to determine whether that message should be audited or not. If the message should not be audited, it retrieves the shared state from the context's `Extensions` and marks the message.

snippet: auditRulesBehavior

### AuditFilterBehavior

This behavior is invoked for every message which is sent to the audit queue. By retrieving the shared state and checking its value, this behavior can stop the auditing pipeline by not invoking the next step.

snippet: auditFilterBehavior

The filtering logic must be registered in the pipeline:

snippet: addFilterBehaviors

## Running the code

- Run the solution.
- Wait until both messages are handled by their message handlers.
- Verify the configured audit queue (named `audit`) does not contain the `DoNotAuditThisMessage` message. For the default learning transport, this can be found in the `.learningtransport` folder.
