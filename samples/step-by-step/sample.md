---
title: Step by Step Guide
summary: Get started with NServiceBus
tags: []
redirects:
- nservicebus/nservicebus-step-by-step-guide
---

In this sample shows a very simple ordering system that 

 * sends a command from a client to a server 
 * that server handles the command and publishes a new event about the success
 * a subscriber listens to, and handles, the event


## The Shared Project

The `Shared` project is the container for shared classes including message definitions. This project will be shared between the client and server so both sides agree on the typed message descriptions. It is referenced by all projects in the solution. 


### PlaceOrder Command

Used to initiate an order

snippet:PlaceOrder


### OrderPlaced Event

Used to indicate that an order has been received and processed.

snippet:OrderPlaced


## The Client

The `Client` is the initiate for the ordering process. The sending code in the client is as follows.

snippet:SendOrder

This is a console blocking loop that is called the the console starts.


## The Server 

The `Server` project processes an Order. It receives `PlaceOrder` sent from `Client` and then publishes `OrderPlaced` on success.

snippet:PlaceOrderHandler


## The Subscriber

The `Subscriber` project needs notification of a successful order. Hence it subscribes to `OrderPlaced` events from `Server`.


### Subscription Configuration

The subscription is done in the `app.config` of the project

<!-- import subscriptionConfig--> 


### Handling the event

When the event is received it will be passed to `OrderCreatedHandler` for processing.

snippet:OrderCreatedHandler


## Running the solution

Run the solution.


### Client Output

The output will be 

    Press 'Enter' to send a message. To exit press 'Ctrl + C'

If you hit enter you will see

    Sent a new PlaceOrder message with id: 5e906f84397e4205ae486f0aa79935e2


### Server Output

`Server` will receive that message, process it and then publish it.

```
2015-04-23 11:45:22.088 INFO Subscribing StepByStep.Subscriber to message type OrderPlaced
To exit press 'Ctrl + C'
Order for Product:New shoes placed with id: ef5b77ba-192e-451b-ad4a-94c4acbbd8ed
Publishing: OrderPlaced for Order Id: ef5b77ba-192e-451b-ad4a-94c4acbbd8ed
```

### Subscriber Output

`Subscriber` will receive the message that is published from `Server`

```
To exit press 'Ctrl + C'
2015-04-23 11:45:19.972 INFO Subscribing to OrderPlaced publisher queue StepByStep.Server
Handling: OrderPlaceed for Order Id: ef5b77ba-192e-451b-ad4a-94c4acbbd8ed
```
