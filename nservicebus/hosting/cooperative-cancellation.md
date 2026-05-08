---
title: Cooperative cancellation
summary: To participate in a graceful shutdown initiated by the host
reviewed: 2026-02-26
component: core
related:
- samples/cooperative-cancellation
---

Starting with version 8, NServiceBus supports [cooperative cancellation](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation). This enables NServiceBus to participate in graceful shutdown of its host by exposing a [cancellation token](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) to abort potentially long-running operations both outside and inside the message processing pipeline.

partial: content
