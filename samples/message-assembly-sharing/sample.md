---
title: Sharing message assemblies
summary: Demonstrate how the NServiceBus.MessageInterfaces package enables message assemblies to be shared across major versions of NServiceBus
reviewed: 2026-01-20
component: Core
related:
  - nservicebus/messaging/unobtrusive-mode
---

This sample shows how the `NServiceBus.MessageInterfaces` package can be used to create a shared message assembly that can be used by multiple major versions of NServiceBus, and in projects using different target frameworks, while still relying on the `ICommand` and `IEvent` marker interfaces.

In this sample, an NServiceBus endpoint running on .NET Framework can share a message assembly with another endpoint running a newer version of NServiceBus and .NET without the need to employ [unobtrusive message conventions](/nservicebus/messaging/unobtrusive-mode.md), which is especially useful for projects transitioning from .NET Framework to .NET that were not originally designed with message conventions in mind.

### Shared project

The `Shared` project references the `NServiceBus.MessageInterfaces`, which contains the definitions for `IMessage`, `ICommand`, and `IEvent`. Because this project targets `netstandard2.0` it can be used from both .NET and .NET Framework applications.

snippet: shared-project

There is a single message type defined, `MyCommand`, which derives from `ICommand`.

## Sending messages

The `Sender` endpoint uses NServiceBus 8.2 on .NET Framework and sends `MyCommand` messages to `Receiver`.

## Receiving messages

The `Receiver` endpoint uses NServiceBus 9 on modern .NET and has a handler for `MyCommand` messages.
