---
title: Delayed Delivery
reviewed: 2016-03-21
component: Core
tags:
- Defer
related:
- nservicebus/messaging/delayed-delivery
---

In this sample shows a very simple ordering system that:

 * Sends a command from a client to a server.
 * That server handles the command.

While sending command user can choose to defer handling or delivery of the message.


## The Shared Project

The `Shared` project is for shared classes including message definitions. This project will be shared between the client and server so both sides agree on the typed message descriptions. It is referenced by all projects in the solution.


### PlaceOrder Command

Used to place an order, used in Defer Message Handling scenario

snippet: PlaceOrder


### PlaceDelayedOrder Command

Used to place an order, used in Defer Message Delivery scenario

snippet: PlaceDelayedOrder


## Defer message handling

This flow is shown when user chose 1 on Client Console. In this case message is deferred after receiving on the `Server`.


### The Client

The `Client` is the initiate for the ordering process. The sending code in the client is as follows.

snippet: SendOrder


### The Server

The `Server` project processes an Order. It receives `PlaceOrder` sent from `Client` and for the first time defers it's handling for some time.

snippet: PlaceOrderHandler


## Defer message delivery

This flow is shown when user chose 2 on Client Console. In this case message is deferred before being send on the `Client`.


### The Client

The `Client` is the initiate for the ordering process. The 'Client' defers sending of a message as can be seen below.

snippet: DeferOrder


partial: HandleDefer


### The Server

The `Server` project processes an Order. It receives `PlaceDelayedOrder` sent from `Client` and process it.

snippet: PlaceDelayedOrderHandler