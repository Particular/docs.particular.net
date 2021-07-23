---
title: Build message-driven business applications with NServiceBus
summary: Build message-driven business applications with NServiceBus and Azure Service Bus
reviewed: 2021-07-23
component: ASBS
related:
- transports/azure-service-bus
---


## Prerequisites

include: asb-connectionstring-xplat


## Code walk-through

This sample shows an end-to-end implementation of NServiceBus with Azure Service Bus for Microsoft documentation article available at [TBD](https://azure.microsoft.com/???/articles/service-bus-messaging/solve-complex-problems-with-nservicebus.md).

 * `Sender` sends a `Ping` message to `Receiver`.
 * `Receiver` replies to `Sender` with a `Ping` message.