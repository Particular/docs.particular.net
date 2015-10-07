---
title: Distributor error handling
summary: Error handling with and configuration with the distributor
tags:
- Distributor
- Error Handling
- Exceptions
- Automatic Retries
related:
- samples/scaleout
- nservicebus/scalability-and-ha/distributor
---

- SLR TimeIncrease is managed by the distributor, changing this value on the workers does not have any effect.
- SLR NumberOfRetries is strange, it seems to be applied on both the distributor and the worker
- NumberOfRetries distributor<worker, then distributor forwards message to error queue
- NumberOrRetries distributor>worker, then worker forward message to the error queue
- NumberOfRetries distributor=worker, then worker forward message to the error queue

[13:23] ramon: Is this behavior intended? Reason I ask is that a customer wants to know how it needs to configure SLR with the distributor. Based on this behavior I would say, distributor and workers need to have the same SLR config. Workers use the NumberOfRetries attribute and the distributor uses the TimeIncrease attribute.

[13:24] ramon: @janovesk @andreas @seanfarmar : Please take note of this behavior based on our conversations of yesterday.

[13:26] ramon: So basically the question is: who is responsible for forwarding a message to the error queue when message failed too many times. Is it the worker or the distributor?





NServiceBus provides error handling and has [First Level Retries (FLR)]() and [Secondary  Level Retries (SLR)](). How these features behave is different between major versions.


## First Level Retries (FLR)

[First Level Retries (FLR)](http://docs.particular.net/nservicebus/errors/automatic-retries#first-level-retries) are always performed on the worker.


## Second Level Retries (SLR)

The behavior of [Second Level Retries (SLR)](http://docs.particular.net/nservicebus/errors/automatic-retries#second-level-retries) is different between major versions.

SLR has two configurable items TimeIncrease and NumberOfRetries.

The message will be retried on *any* available worker. Message processing is not sticky to the worker.


### NServiceBus v5.x SLR behavior

The SLR policy *NumberOfRetries* setting its is applied on *both* the distributor and workers, and the *TimeIncrease* setting is applied on the workers.

When an error occurs the SLR policy is invoked immediately by the fault manager. The message will not be forwarded to the retries queue which is was the previous behavior.

When the retry limit is reached the message is  forwarded immediately to the error queue or else forwarded to the **.retries** queue and scheduled for retry. If the SLR policy output is that it needs to be retried then the message is forwarded to the **.retries** queue.


### NServiceBus v4.x SLR behavior

The SLR policy *NumberOfRetries* setting its is applied on the distributor and the *TimeIncrease* setting is applied on the workers.

The distributor has a **.retries** queue where a message is forwarded to in case of an error. Then the distributor processes this message, when the retry limit has been reached the message will be forwarded to the error queue or else scheduled for retry by the distributor.


### NServiceBus v3.x SLR behavior

The SLR policy is applied *only* on the workers for both *TimeIncrease* and *NumberOfRetries* settings. If a SLR configuration is available on the distributor then these settings are ignored.

When an error occurs the worker schedules the retry according to the SLR TimeIncrease setting and when the corresponding timeout is triggered the message is forwarded to the incoming queue of the distributor which will then forward the message to an available worker.


## Best practise

Due to the behavioral differences between major versions it is advised to have the *SecondLevelRetriesConfig* exactly the same on both the distributor and the workers even though it could be that settings are ignored.

It is assumed that *NumberOfRetries* and *TimeIncrease* are the same on the distributor and the workers.

NOTE: If settings are managed in XML configuration a comment stating that changes should be updated on the distributor and worker configuration files.

