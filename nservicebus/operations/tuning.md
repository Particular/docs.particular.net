---
title: Tuning endpoint message processing
summary: Optimize message processing by configuring concurrency
component: Core
related:
 - nservicebus/operations
redirects:
 - nservicebus/how-to-reduce-throughput-of-an-endpoint
 - nservicebus/operations/reducing-throughput
 - nservicebus/operations/throughput
tags:
 - performance
 - throttling
 - concurrency
reviewed: 2016-11-15
---

NServiceBus uses defaults that ensure good performance in most common cases. While this is usually the preferred mode of operation there are situations where tuning needs to be applied.


## Tuning concurrency


partial: defaults


Define a maximum concurrency setting that will make sure that no more messages than the specified value is ever being processed at the same time. Set this value to `1` to process messages sequentially. If not specified the transport will choose an optimal value.

Examples where concurrency tuning is relevant are

 * Non thread safe code that needs to run sequentially
 * Databases that might deadlock when getting too many concurrent requests
 
 NOTE: The endpoint needs to be restarted for concurrency changes to take effect.


## Throttling

partial: throttling


## Configuration

partial: configuration



partial: runtime
