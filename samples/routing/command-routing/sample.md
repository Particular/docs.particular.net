---
title: Command routing
summary: Basic NServiceBus message routing
component: Core
reviewed: 2025-10-13
---

The sample demonstrates basic command routing between endpoints.

## Running the project

1. Start all the projects by hitting F5.
1. In the Sender's console window send some orders by pressing S
1. In the Sender's console window cancel some orders by pressing C
1. Both messages are sent to the Receiver which logs a message

## Code walk-through

### Endpoint configuration command routing

Command routing can be specified in the endpoint configuration, as the sample does for the `PlaceOrder` command.

snippet: configure-command-route

Whenever the Sender sends a `PlaceOrder` command it does not need to specify a destination. This is the preferred method for routing commands.

snippet: send-command-with-configured-route

### Per-message command routing

In special circumstances, a command can be routed when it is sent.

Whenever the Sender sends a `CancelOrder` command it specifies the destination endpoint.

snippet: send-command-without-configured-route

