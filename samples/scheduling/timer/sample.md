---
title: Scheduling with Timers
summary: An example of scheduling using .NET Timers
reviewed: 2020-10-27
component: Core
related:
 - nservicebus/scheduling
---

This sample illustrates the use of a [.NET Timer](https://docs.microsoft.com/en-us/dotnet/api/system.threading.timer) to trigger scheduled tasks. To leverage the benefits of [NServiceBus retries](/nservicebus/recoverability/) and [consistency of outgoing messages with the transport transaction](/transports/transactions.md), the tasks are implemented as regular message handlers. This also gives full traceability of the invoked tasks in platform tools like [ServicePulse](/servicepulse/) and [ServiceInsight](/serviceinsight/).

snippet: ScheduleUsingTimer
