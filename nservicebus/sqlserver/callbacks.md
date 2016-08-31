---
title: SQL Server Transport Callback support
reviewed: 2016-08-31
tags:
 - SQL Server
---

The settings mentioned below are available in version 2.x of the SQL Server transport. In version 3.x using callbacks requires the new `NServiceBus.Callbacks` NuGet package. Refer to [callbacks](/nservicebus/messaging/handling-responses-on-the-client-side.md) for more details.


### Disable callbacks

Callbacks and callback queues receivers are enabled by default. In order to disable them use the following setting:

snippet:sqlserver-config-disable-secondaries

Secondary queues use the same adaptive concurrency model as the primary queue. Secondary queues (and hence callbacks) are disabled for satellite receivers.


### Callback Receiver Max Concurrency

Changes the number of threads used for the callback receiver. The default is 1 thread.

snippet:sqlserver-CallbackReceiverMaxConcurrency