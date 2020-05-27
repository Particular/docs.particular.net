---
title: Command routing
summary: Basic NServiceBus message routing
component: Core
reviewed: 2019-10-09
---

The sample demonstrates basic command routing between endpoints.


## Running the project

1. Start all the projects by hitting F5.
1. In the Sender's console window send some orders by pressing S
1. In the Sender's console window cancel some orders by pressing C
1. Both messages are sent to the Receiver which logs a message


## Code walk-through

Whenever the Sender sends a `CancelOrder` command it must specify the destination endpoint.

snippet: send-command-without-configured-route

Whenever the Sender sends a `PlaceOrder` command it does not need to specify a destination.

snippet: send-command-with-configured-route

This is enabled by configuring a message route for the `PlaceOrder` command in the endpoint configuration.

snippet: configure-command-route