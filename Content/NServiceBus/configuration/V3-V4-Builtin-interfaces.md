---
title: Configuration API Builtin interfaces in V3 and V4
summary: Configuration API Builtin interfaces in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
- V3
- V4
---

Message contracts are defined using plain C# classes or interfaces, for NServiceBus to find those classes when scanning assemblies, you need to mark them with the special `IMessage` interface, or the `ICommand` or `IEvent` interfaces, both inherit from the `IMessage` one.