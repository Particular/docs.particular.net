---
title: Build message-driven business applications with NServiceBus
summary: Build message-driven business applications with NServiceBus and Azure Service Bus
reviewed: 2021-07-26
component: ASBS
related:
- transports/azure-service-bus
---


## Prerequisites

* An existing Azure Service Bus namespace
* A connection string for the Azure Service Bus namespace defined in `appsettings.json` for both the Sender and the Receiver projects

## Code walk-through

This sample shows an end-to-end implementation of NServiceBus with [Azure Service Bus](https://docs.microsoft.com/en-us/azure/service-bus-messaging/). It depicts a sender that sends a command to a receiver which then replies back to the sender.

 * `Sender` sends a `Ping` message to `Receiver`.
 * `Receiver` replies to `Sender` with a `Ping` message.
