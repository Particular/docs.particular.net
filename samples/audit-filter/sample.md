---
title: AuditFilter
reviewed: 2018-12-11
component: AuditFilter
---

## Introduction

Uses the [AuditFilter](/nservicebus/audit-filter/) component to control what messages are sent to the audit queue.


## Code Walk-through

This sample uses the [Learning Transport](/transports/learning/) and the resultant messages can be viewed in the [Storage Directory](/transports/learning/#usage-storage-directory).


### Decorate messages with attributes

snippet: MessageToExcludeFromAudit

snippet: MessageToIncludeAudit


### Add to EndpointConfiguration

snippet: Enable
