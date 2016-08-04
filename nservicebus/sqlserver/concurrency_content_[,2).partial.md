---
title: SQL Server Transport concurrency handling
summary: The design and implementation details of SQL Server Transport concurrency handling
reviewed: 2016-08-03
component: SqlServer
tags:
- SQL Server
- Concurrency
- Transport
---

The SQL Server transport adapts the number of receiving threads (up to `MaximumConcurrencyLevel` [set via `TransportConfig` section](/nservicebus/msmq/transportconfig.md)) to the amount of messages waiting for processing. When idle it maintains only one thread that continuously polls the table queue.


### Version 3

In Versions 3 and above SQL Server transport maintains a dedicated monitoring thread for each input queue. It is responsible for detecting the number of messages waiting for delivery and creating receive [Task](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx)s - one for each pending message.

The maximum number of concurrent tasks will never exceed `MaximumConcurrencyLevel`. The number of tasks does not translate to the number of running threads which is controlled by the TPL scheduling mechanisms.


### Version 2.1

Version 2.1 of SqlTransport uses an adaptive concurrency model. The transport adapts the number of polling threads based on the rate of messages coming in. The key concept in this new model is the *ramp up controller* which controls the ramping up of new threads and decommissioning of unnecessary threads. It uses the following algorithm:

 * if last receive operation yielded a message, it increments the *consecutive successes* counter and resets the *consecutive failures* counter
 * if last receive operation yielded no message, it increments the *consecutive failures* counter and resets the *consecutive successes* counter
 * if *consecutive successes* counter goes over a certain threshold and there is less polling threads than `MaximumConcurrencyLevel`, it starts a new polling thread and resets the *consecutive successes* counter
 * if *consecutive failures* counter goes over a certain threshold and there is more than one polling thread it kills one of the polling threads


### Version 2.0

In 2.0 release support for callbacks has been added. Callbacks are implemented by each endpoint instance having a unique [secondary queue](./#secondary-queues). The receive for the secondary queue does not use the `MaximumConcurrencyLevel` and defaults to 1 thread. This value can be adjusted via the configuration API.


### Prior to version 2.0

Prior to 2.0 each endpoint running SQLServer transport spins up a fixed number of threads (controlled by `MaximumConcurrencyLevel` property of `TransportConfig` section) both for input and satellite queues. Each thread runs in loop, polling the database for messages awaiting processing.

The disadvantage of this simple model is the fact that satellites (e.g. [Delayed Retries](/nservicebus/recoverability/#delayed-retries), Timeout Manager) share the same concurrency settings but usually have much lower throughput requirements. If both Delayed Retries and Timeout Manager are enabled, setting `MaximumConcurrencyLevel` to 10 results in 40 threads in total, each polling the database even if there are no messages to be processed.