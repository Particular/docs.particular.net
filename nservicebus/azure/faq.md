---
title: Azure FAQ
summary: Frequently Asked Questions related to the Azure transports and Azure hosting options.
tags:
- Cloud
- Azure
- Transports
---

This document combines frequently asked questions related to the Azure transports and Azure hosting options.


## Azure Storage Queues


### Do Azure Storage Queues provide the same consistency model as I'm used to with Msmq?

The short answer is no! The longer answer is: Queues are remote, instead of local, and this has several implications.

* A message has to cross the network boundaries before it is persisted, this implies that it is subject to all kinds network related issues like latency, timeouts, connection loss, network partitioning etc.
* Remote queues do not play along in transactions, as transactions are very brittle because of the possible network issues mentioned in the previous point, but also because they would require server side locks to function properly and allowing anyone to take unbound locks on a service is a very good way to get yourself in a denial of service situation. Hence Azure services typically don't allow transactions.


### Do Azure Storage Queues provide an exactly once deliver model?

No, it's at least once delivery. To overcome the lack of transactions, these queues work with a so called 'queue-peek-lock' model. When a worker pulls a message from a queue, it will actually become invisible instead of removed from the queue. The worker has to process the messages in a well defined timeframe and delete it explicitly when done. If the worker fails to do this, because it died, an exception was thrown or it was simply to slow, then the message will reappear on the queue and another instance of the worker can pick it up. This also implies that multiple workers may be working on the same message at the same time (esp. problematic when operations take to long)


## Azure ServiceBus


### Does Azure ServiceBus provide the same consistency model as I'm used to with Msmq?

The short answer is no! For the long answer, see 'Do Azure Storage Queues provide the same consistency model as I'm used to with Msmq?'


### Does Azure ServiceBus provide an exactly once deliver model?

By default it does not, it's an at least once delivery model. But you can enable a feature called Duplicate Detection, which will make sure you get a message exactly once, but this comes at the expense of throughput and is also limited by several conditions (time constrained, not available for partitioned entities).


## Cloud Services


### My MVC webrole 'hangs' when hosted in the compute emulator

There is a known issue when enabling NServiceBus in a website that is hosted in the Microsoft Azure compute emulator and when performance counters have been installed (either via the installer or via PowerShell). There are 2 possible workarounds:

* Do not host the website in the compute emulator, but outside of it
* Remove the performance counters using PowerShell `([Diagnostics.PerformanceCounterCategory]::Delete( "NServiceBus" ))`


### My role instance stays in 'Busy' status, or an infinite reboot loop, after deploying my project to the cloud

This is almost always related to an exception happening at startup of the roleentrypoint. Typically this is a [TypeLoadException](https://msdn.microsoft.com/en-us/library/system.typeloadexception.aspx) coming from a missing assembly or a bad connection string, one that is still pointing to development storage for example.


### Exceptions occurring at startup are not visible in the logs

When using the diagnostics service in cloud services, this service starts in parallel with the startup code. If an exception occurs at this point in time, the code may not be able to call the diagnostics service yet and the exception information may get lost. Use intellitrace and historical debugging instead to learn more about the cause of the exception.
