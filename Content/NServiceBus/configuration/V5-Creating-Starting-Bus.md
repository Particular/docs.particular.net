---
title: Configuration API Creating and Starting the Bus in V5
summary: Configuration API Creating and Starting the Bus in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

When using the `NServiceBus.Host` the bus creation and startup is done by the hosting engine, when self-hosting the bus we are responsible to create and start the bus.

#### Creation

Given a `BusConfiguration` instance to create the bus it is enough to call the static `Create` method of the `Bus` class, or to create a `SendOnly` bus call the `CreateSendOnly` method.

#### Startup

If the created bus is not a send-only bus it must be started via the `Start()` method of the `IStartableBus` instance.