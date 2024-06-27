---
title: PeriodTimer usage
summary: Using PeriodicTimer in a BackgroundJob to send messages from within an NServiceBus endpoint.
reviewed: 2024-06-26
component: Core
related:
  - nservicebus/scheduling
---

This sample illustrates the use of [`PeriodicTimer`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.periodictimer) to send messages from within an NServiceBus endpoint.

`PeriodicTimer` was introduced in .NET 6 and enables waiting asynchronously for timer ticks.

## Running the project

1.  Start both the Scheduler and Receiver projects.
1.  At startup, Scheduler will schedule a message send to Receiver every 3 seconds.
1.  Receiver will handle the message.

## Code Walk-through

### Define the background service

The [`BackgroundService`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.backgroundservice) defines the job to be run when the host starts. It sets up a `PeriodicTimer` which will tick every 3 seconds. Every time it ticks, a message is sent using NServiceBus.

snippet: SendMessageJob

### Configure host

The host is configured with NServiceBus and the background service created above. When the host starts, the background service is run and messages will be sent periodically.

snippet: ConfigureHost
