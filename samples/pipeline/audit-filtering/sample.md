---
title: Audit filter pipeline extension
summary: Extending the pipeline to stop certain messages from being audited.
reviewed: 2017-02-21
component: Core
tags:
 - Pipeline
 - Audit
related:
 - samples/audit-filter
 - nservicebus/pipeline
 - nservicebus/operations/auditing
---


## Introduction

This sample shows how to extend the NServiceBus message processing pipeline with custom behaviors to add filters which prevent certain message types from being forwarded to the audit queue.

NOTE: While this sample shows the dynamics of manipulating the Audit pipeline, there is also the [Audit Filter](/nservicebus/audit-filter) feature delivers this functionality in a re-usable and packaged manner.


## Code Walk Through

The solution contains a single endpoint with auditing enabled. The endpoint sends one `AuditThisMessage` and one `DoNotAuditThisMessage` to itself on start up. Both messages are handled by message handlers however only the `AuditThisMessage` will be moved to the audit queue, and the `DoNotAuditThisMessage` is filtered out.

partial: filtering

The filtering logic then needs to be registered in the pipeline:

snippet: addFilterBehaviors


## Running the Code

 * Run the solution.
 * Wait until both messages are handled by their message handlers.
 * Verify the configured audit queue (Samples.AuditFilter.Audit) does not contain the `DoNotAuditThisMessage`.