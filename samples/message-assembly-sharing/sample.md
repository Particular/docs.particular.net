---
title: Sharing message assemblies
summary: Demonstrate how the NServiceBus.MessageInterfaces package enables message assemblies to be shared across major versions of NServiceBus
reviewed: 2024-04-20
component: Core
---

## Code walk-through

This sample shows how the `NServiceBus.MessageInterfaces` package can be used to create a shared message assembly that can be used by multiple major versions of NServiceBus.

### Shared project

The `Shared` project references the `NServiceBus.MessageInterfaces`, which contains the definitions for `IMessage`, `ICommand`, and `IEvent`. There is a single message type defined, `MyCommand`, which derives from `ICommand`.

### Sending messages

The `Sender` endpoint uses NServiceBus 8 and sends `MyCommand` messages to `Receiver`.

### Receiving messages

The `Receiver` endpoint uses NServiceBus 9 and has a handler for `MyCommand` messages.