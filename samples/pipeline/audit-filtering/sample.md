---
title: Audit Filter Pipeline Extension
summary: Extending the pipeline to stop certain messages from being audited
reviewed: 2020-10-24
component: Core
related:
 - nservicebus/pipeline
 - nservicebus/operations/auditing
---


This sample shows how to extend the NServiceBus message-processing pipeline with custom behaviors to add filters which prevent certain message types from being forwarded to the audit queue.


## Code walk-through

The solution contains a single endpoint with auditing enabled. When it starts, the endpoint sends one `AuditThisMessage` message and one `DoNotAuditThisMessage` message to itself. Both messages are handled by message handlers; however only the `AuditThisMessage` message will be moved to the audit queue, and the `DoNotAuditThisMessage` message is filtered out.

partial: filtering

The filtering logic must be registered in the pipeline:

snippet: addFilterBehaviors


## Running the code

 * Run the solution.
 * Wait until both messages are handled by their message handlers.
 * Verify the configured audit queue (Samples.AuditFilter.Audit) does not contain the `DoNotAuditThisMessage` message.