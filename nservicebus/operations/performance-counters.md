---
title: Performance Counters
summary: Monitoring through the use of performance counters.
reviewed: 2016-11-01
component: Core
tags:
 - Performance Counters
redirects:
 - nservicebus/monitoring-nservicebus-endpoints
 - nservicebus/operations/monitoring-endpoints
 - nservicebus/operations/fail-or-hang-during-performance-counter-setup
related:
 - servicecontrol
 - servicepulse
 - nservicebus/recoverability
 - nservicebus/operations/auditing
 - nservicebus/operations/management-using-powershell
 - samples/performance-counters
---

When a system is broken down into multiple processes, each with its own queue, it allows identifying which process is the bottleneck by examining how many messages (on average) are in each queue. The only issue is that without knowing the rate of messages coming into each queue, and the rate at which messages are being processed from each queue, it is not possible to know how long messages are waiting in each queue, which is the primary indicator of a bottleneck.

Despite the many performance counters Microsoft provides for MSMQ (including messages in queues, machine-wide incoming and outgoing messages per second, and the total messages in all queues), there is no built-in performance counter for the time it takes a message to get through each queue.

NServiceBus includes several performance counters. They are installed in the `NServiceBus` category.

Since all performance counters in Windows are exposed via Windows Management Instrumentation (WMI), it is very straightforward to pull this information into the existing monitoring infrastructure.


## Critical Time

**Counter Name:** `Critical Time`

**Added in:** Version 3

Critical Time is the time from a message being sent until successfully processed. This metric is useful for monitoring message Service-level agreement. For example "All orders should be processed within X seconds/minutes/hours". Define an SLA for each endpoint and use the `CriticalTime` counter to ensure it is adhered to.

**Versions 6 and above:** The value recorded in the TimeSent header is the time when the call to send the message is executed, not the actual time when the message was dispatched to the transport infrastructure. Since the outgoing messages in handlers are sent as a [batched](/nservicebus/messaging/batched-dispatch.md) operation, depending on how long the message handler takes to complete, the actual dispatch may happen later than the time recorded in the TimeSent header. For operations outside of handlers the recorded sent time is accurate.


### Configuration

This counter can be enabled using the the following code:

snippet:enable-criticaltime

In the NServiceBus Host this counter is enabled by default.


## SLA violation countdown

**Counter Name:** `SLA violation countdown`

**Added in:** Version 3

Acts as a early warning system to inform on the number of seconds left until the SLA for the endpoint is breached. This gives a system-wide counter that can be monitored without putting the SLA into the monitoring software. Just set that alarm to trigger when the counter goes below X, which is the time that the operations team needs to be able to take actions to prevent the SLA from being breached.


### Configuration

This counter can be enabled using the the following code:

snippet: enable-sla

See also [Performance Counters in the NServiceBus Host](/nservicebus/hosting/nservicebus-host/#performance-counters).


## Successful Message Processing Rate

**Counter Name:** `# of msgs successfully processed / sec`

**Added in:** Version 4

The current number of messages processed successfully by the transport per second.


### Configuration

Enabled by default and will only write to the counter if it exists.


## Queue Message Receive Rate

**Counter Name:** `# of msgs pulled from the input queue /sec`

**Added in:** Version 4

The current number of messages pulled from the input queue by the transport per second.


### Configuration

Enabled by default and will only write to the counter if it exists.


## Failed Message Processing Rate

**Counter Name:** `# of msgs failures / sec`

**Added in:** Version 4

The current number of failed processed messages by the transport per second.


### Configuration

Enabled by default and will only write to the counter if it exists.


## Installing Counters

The NServiceBus Performance counters can be installed using the [NServiceBus PowerShell tools](management-using-powershell.md).

```ps
Import-Module NServiceBus.PowerShell
Install-NServiceBusPerformanceCounters
```

To list the installed counters use

```ps
Get-Counter -ListSet NServiceBus | Select-Object -ExpandProperty Counter
```

NOTE: After installing the performance counters, all endpoints must be restarted in order to start collecting the new data.


## Performance Monitor Users local security group

When [running installers](installers.md) the service account will be automatically added to the local Performance Monitor Users group if executed with elevated privileges.


## System.InvalidOperationException

If the endpoint instance throws one of the  following exceptions at startup, then [the performance counters need to be reinstalled](#installing-counters)

 * `InvalidOperationException: The requested Performance Counter is not a custom counter, it has to be initialized as ReadOnly.`
 * `InvalidOperationException: NServiceBus performance counter for 'Critical Time' is not set up correctly.`


## Corrupted Counters

Corrupted performance counters can cause the endpoint to either hang completely during startup or fail with the following error:

`NServiceBus performance counter for '{counterName}' is not set up correctly`

Should this happen try rebuilding the performance counter library using the following steps:

 1. Open an elevated command prompt
 1. Execute the following command to rebuild the performance counter library: `lodctr /r`


### More information

 * [KB2554336: How to manually rebuild Performance Counters for Windows Server 2008 64bit or Windows Server 2008 R2 systems](https://support.microsoft.com/en-us/kb/2554336)
 * [KB300956: How to manually rebuild Performance Counter Library values](https://support.microsoft.com/kb/300956)
 * [LODCTR at TechNet](https://technet.microsoft.com/en-us/library/bb490926.aspx)
