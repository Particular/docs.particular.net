---
title: Distributor error handling
summary: Error handling with and configuration with the distributor
component: Distributor
reviewed: 2016-08-30
tags:
 - Error Handling
 - Exceptions
 - Retry
redirects:
 - nservicebus/errors/distributor-errorhandling
 - nservicebus/scalability-and-ha/error-handling
 - nservicebus/msmq/distributor/error-handling
related:
 - samples/scaleout
 - transports/msmq/distributor
---

NServiceBus provides error handling and has [Immediate Retries](/nservicebus/recoverability/#immediate-retries) and [Delayed Retries](/nservicebus/recoverability/#delayed-retries).

When using the distributor the behavior of retries and the related message flow is different between major versions.


## Immediate Retries

Immediate Retries are always performed on the worker.


## Delayed Retries

The behavior of Delayed Retries is different between major versions.

Delayed Retries has two configurable items TimeIncrease and NumberOfRetries.

The message will be retried on *any* available worker. Message processing is not sticky to the worker.


partial: retries


## Best practice

Due to the behavioral differences between major versions it is advised to have the *SecondLevelRetriesConfig* exactly the same on both the distributor and the workers even though it could be that settings are ignored.

It is assumed that *NumberOfRetries* and *TimeIncrease* are the same on the distributor and the workers.

NOTE: If settings are managed in XML configuration a comment stating that changes should be updated on the distributor and worker configuration files.