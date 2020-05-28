---
title: Delayed Delivery
summary: A simple ordering system that defers handling or delivery of a message
reviewed: 2019-08-20
component: Core
related:
- nservicebus/messaging/delayed-delivery
---

This sample shows a simple ordering system that:

 * sends a command from a client to a server
 * handles the command on the server

While sending the command, the user can choose to defer handling or delivery of the message.


## The Shared Project

The `Shared` project is for shared classes including message definitions. This project is shared between the client and server so both sides agree on the typed message descriptions. It is referenced by all projects in the solution.


### PlaceOrder Command

This is the command for placing an order; it used in the Defer Message Handling scenario

snippet: PlaceOrder


### PlaceDelayedOrder Command

This command also places an order but is used in the Defer Message Delivery scenario.

snippet: PlaceDelayedOrder


## Defer message handling

Choose option 1 in the client console to defer _handling_ the message after receiving it on the server.


### The Client

The `Client` initiates the ordering process. The sending code in the client is as follows.

snippet: SendOrder


### The Server

The `Server` processes an Order. It receives `PlaceOrder` sent from `Client` and for the first time defers its handling for five seconds.

snippet: PlaceOrderHandler


## Defer message delivery

Choose option 2 in the client console to defer _delivery_ of the message on the client.


### The Client

The `Client` initiates the ordering process. The 'Client' defers sending the message as can be seen below.

snippet: DeferOrder


partial: HandleDefer


### The Server

The `Server` project processes an Order. It receives `PlaceDelayedOrder` sent from `Client` and processes it normally.

snippet: PlaceDelayedOrderHandler