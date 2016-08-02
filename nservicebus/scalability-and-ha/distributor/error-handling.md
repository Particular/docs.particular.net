---
title: Distributor error handling
summary: Error handling with and configuration with the distributor
tags:
- Distributor
- Error Handling
- Exceptions
- Retry
redirects:
- nservicebus/errors/distributor-errorhandling
related:
- samples/scaleout
- nservicebus/scalability-and-ha/distributor
---

NServiceBus provides error handling and has [immediate retries](/nservicebus/recoverability/#immediate-retries) and [delayed retries](/nservicebus/recoverability/#delayed-retries).

When using the distributor the behavior of retries and the related message flow is different between major versions.


## Immediate Retries

Immediate Retries are always performed on the worker.


## Delayed Retries

The behavior of Delayed Retries is different between major versions.

Delayed Retries has two configurable items TimeIncrease and NumberOfRetries.

The message will be retried on *any* available worker. Message processing is not sticky to the worker.


### NServiceBus Versions 5.x

The Delayed Retries policy *NumberOfRetries* setting its is applied on *both* the distributor and workers, and the *TimeIncrease* setting is applied on the distributor.

When an error occurs the Delayed Retries policy is invoked immediately by the fault manager. The message will not be forwarded to the retries queue which is was the previous behavior.

When the retry limit is reached the message is  forwarded immediately to the error queue or else forwarded to the **.retries** queue and scheduled for retry. If the Delayed Retries policy output is that it needs to be retried then the message is forwarded to the **.retries** queue.


### NServiceBus Versions 4.x

The Delayed Retries policy is only applied on the distributor for both *NumberOfRetries* and *TimeIncrease* settings.

The distributor has a **.retries** queue where a message is forwarded to in case of an error. Then the distributor processes this message, when the retry limit has been reached the message will be forwarded to the error queue or else scheduled for retry by the distributor.


### NServiceBus Versions 3.x

The Delayed Retries policy is applied *only* on the workers for both *TimeIncrease* and *NumberOfRetries* settings. If a Delayed Retries configuration is available on the distributor then these settings are ignored.

When an error occurs the worker schedules the retry according to the SLR TimeIncrease setting and when the corresponding timeout is triggered the message is forwarded to the incoming queue of the distributor which will then forward the message to an available worker.


## Best practice

Due to the behavioral differences between major versions it is advised to have the *SecondLevelRetriesConfig* exactly the same on both the distributor and the workers even though it could be that settings are ignored.

It is assumed that *NumberOfRetries* and *TimeIncrease* are the same on the distributor and the workers.

NOTE: If settings are managed in XML configuration a comment stating that changes should be updated on the distributor and worker configuration files.