---
title: Audit filter pipeline extension
summary: Extending the pipeline to stop certain messages from being audited.
reviewed: 2017-02-21
component: Core
tags:
 - Pipeline
 - Audit
related:
 - nservicebus/pipeline
 - nservicebus/operations/auditing
---


## Introduction

This sample shows how to extend the pipeline with custom behaviors to add filters which prevent messages from being forwarded to the audit queue.


## Code Walk Through

The solution contains a single endpoint with auditing enabled. The endpoint sends one `AuditThisMessage` and one `DoNotAuditThisMessage` to itself on startup. Both messages are handled by message handlers but `DoNotAuditThisMessage` should not be moved to the audit queue.

partial:filtering

The filtering logic then needs to be registered in the pipeline:

snippet:addFilterBehaviors


## Running the Code

 * Run the solution.
 * Wait until both messages are handled by their message handlers.
 * Verify the configured audit queue (Samples.AuditFilter.Audit) does not contain the `DoNotAuditThisMessage`.