---
title: Configuration API Creating and Starting the Bus in V3 and V4
summary: Configuration API Creating and Starting the Bus in V3 and V4
tags:
- NServiceBus
- BusConfiguration
- V3
- V4
---

Once the endpoint is configured the last step is to define the type of the bus and create it.

* `UnicastBus()`: defines that the bus is a unicast bus, currently the only supported bus type.

#### Creation

* `CreateBus()`: creates a startable bus ready to be started as required.
* `SendOnly()`: creates and starts a send-only bus, suitable for a send-only endpoint that does not receive commands and does not handle events.

#### Startup

If the created bus is not a send-only bus it must be started:

* `Start()`: starts the bus.
* `Start( Action startupAction )`: Starts the bus, invoking the supplied delegate at startup time.
