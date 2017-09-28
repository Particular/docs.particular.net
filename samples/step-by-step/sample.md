---
title: Step by Step Sample
reviewed: 2016-07-13
component: Core
---

WARNING: The completed sample code can be downloaded above but following this guide is strongly encouraged.


## Introduction

This guide illustrates essential NServiceBus concepts by showing how to build a simple system that uses messaging to:

 * Send a command from a client to a server
 * Receive that message and process it on the server
 * From the server handler, publish an event
 * Add a new subscriber that can receive published events

Completing these steps will serve as an introduction to many important NServiceBus concepts:

 * Basic project setup
 * [Self-hosting NServiceBus](/nservicebus/hosting/) within a console application
 * [Defining commands and events](/nservicebus/messaging/messages-events-commands.md)
 * [Sending commands](/nservicebus/messaging/send-a-message.md)
 * [Handling messages](/nservicebus/handlers/)
 * [Publishing events](/nservicebus/messaging/publish-subscribe/publish-handle-event.md)
 * [Logging](/nservicebus/logging/)


## Prerequisites


### .NET Framework Version 4.6.1

[Download the .NET Framework Version 4.6.1](https://www.microsoft.com/en-us/download/details.aspx?id=49982) and install it.


### Learning Transport

INFO: While this sample uses the [Learning Transport](/transports/learning/) as the queuing transport, it is possible to use NServiceBus with a variety of other [transports](/transports/).


## Project structure

The finished solution will contain four projects. Create a new Visual Studio solution containing the following:

Note: Ensure .NET 4.6.1 is selected as the [target framework](https://msdn.microsoft.com/en-us/library/bb398202.aspx).

 * A Console Application named `Client`
 * A Console Application named `Server`
 * A Console Application named `Subscriber`
 * A Class Library named `Shared`

Each of the console applications will be configured to be **messaging endpoints**, meaning they are able to send and receive NServiceBus messages. Commonly they will be referred to simply as [endpoints](/nservicebus/endpoints/).

The client endpoint's job will be to send `PlaceOrder` commands to the server. The server endpoint will process the `PlaceOrder` commands, and then publish an `OrderPlaced` event to any interested subscribers. The subscriber endpoint will subscribe to `OrderPlaced` events published from the server, and process those events when they arrive.

The shared project will be referenced by all of the endpoints and serve as a central location to store message definition classes and common pieces of configuration.

NOTE: Storing all message definitions in a single location is not a best practice, but serves to illustrate how things work for this simple example.


### Setting dependencies


#### Reference the Shared project.

In the `Client`, `Server`, and `Subscriber` projects, add a reference to the `Shared` project.


#### Add the NServiceBus NuGet package

Install the [NServiceBus NuGet package](https://www.nuget.org/packages/NServiceBus) in all projects.


## Defining messages

First, define the messages that will be used by the system. The client will send a `PlaceOrder` command to the server, and in response, the server will publish an `OrderPlaced` event.

There is a [difference between commands and events](/nservicebus/messaging/messages-events-commands.md). A **command** is a request to perform a task, which is sent to one specific location. In contrast, an **event** is an announcement that something has happened, which is published from one location and can be consumed by multiple subscribers.

The easiest way to differentiate commands and events is to use the `ICommand` and `IEvent` marker interfaces.

Define the `PlaceOrder` command by creating a class that implements `ICommand` in the Shared project called `PlaceOrder`:

snippet: PlaceOrder

Next, define the `OrderPlaced` event by creating a class that implements `IEvent` in the Shared project called `OrderPlaced`:

snippet: OrderPlaced


## Error Queue

Each endpoint configures [recoverability](/nservicebus/recoverability/):

```cs
endpointConfiguration.SendFailedMessagesTo("error");
```

This defines where messages are sent when they cannot be processed due to repetitive exceptions during message processing.

NOTE: It is also possible to [recoverability](/nservicebus/recoverability/) in an App.config file or the the `IProvideConfiguration` interface using [override app.config settings](/nservicebus/hosting/custom-configuration-providers.md), which allows sharing the same configuration across all endpoints.


## The Client

Next, the Client application must be ready to send messages with NServiceBus. In the Client application, add the following code to the Program class. Ignore the missing `SendOrder` method, it will be added below.

snippet: ClientInit

With a `PlaceOrder` command defined, and NServiceBus initialized, a loop can be created to send a new command message every time the Enter key is pressed.

In the Client endpoint, add this code to the Program class:

snippet: SendOrder

The Client endpoint is now complete, and could now be executed. However, doing so would throw an exception when trying to send the message. The Client is sending the `PlaceOrder` command to an endpoint named `Samples.StepByStep.Server`, which will not exist yet. A server endpoint must be created to handle that command.


## The Server

Like the client, the Server application needs to be configured as an NServiceBus endpoint.

In the Server application, add the following code to the Program class:

snippet: ServerInit

Notice that the endpoint name is different, which differentiates the Server endpoint from the Client.

Next, create a new class in the Server project named `PlaceOrderHandler` using the following code:

snippet: PlaceOrderHandler

This class is the message handler that processes the `PlaceOrder` command being sent by the Client. A handler is where a message is processed; very often this will involve saving information from the message into a database, calling a web service, or some other business function. In this example, the message is logged, so the fact the message was received will be visible in the Console window. Next, the handler publishes a new `OrderPlaced` event.

The handler class is automatically discovered by NServiceBus because it implements the `IHandleMessages<T>` interface. The [dependency injection system](/nservicebus/dependency-injection/) (which supports constructor or property injection) injects the `IMessageHandlerContext` instance into the handler to access messaging operations. When a `PlaceOrder` command is received, NServiceBus will create a new instance of the `PlaceOrderHandler` class and invoke the `Handle` method.

The next step is to create a subscriber for this event.

NOTE: The solution could be executed at this stage and would run without exceptions. However, there are no subscribers to the `OrderPlaced` event, so the sample is not yet complete. In practice it's perfectly valid to have a publisher with no subscribers; a new subscriber can be added at a later date. In such a situation, no messages are exchanged on the transport level, meaning there's no communication between endpoints when the event is published.


## The Subscriber

Like the client and the server, the Subscriber application also needs to be configured as an NServiceBus endpoint.

In the Subscriber application, add the following code to the Program class:

snippet: SubscriberInit

This is almost identical to the Server configuration, except for the different endpoint name.

Next, create a message handler for the `OrderPlaced` event. Note that whether handling a command or an event, the syntax for a message handler remains the same.

Create a new class in the Subscriber project named `OrderCreatedHandler` using the following code:

snippet: OrderCreatedHandler

The handler only logs the fact that the message was received. In a real system, a subscriber to `OrderPlaced` could charge a credit card for the order, start to prepare the order for shipment, or even update information in a customer loyalty database to track when the customer is eligible for different purchase-driven incentives.

In fact, all of those activities could be handled by different subscribers to the same event. The fact that these disparate tasks can be accomplished in completely separate message handlers instead of one monolithic process is one of the many strengths of the Publish/Subscribe messaging pattern. Each subscriber is focused on one task, and any failure in one doesn't affect the others.

Next, the subscriber needs to inform the publisher that it wants to receive `OrderPlaced` events when they are published. To do that, [subscriptions need to be configured](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md). However since this sample uses the [Learning Transport](/transports/learning) subscriptions are handled automatically.

When the Subscriber endpoint initializes, it will read this configuration. Because the endpoint also contains a message handler for `OrderPlaced`, it will send a special subscription message to the `Samples.StepByStep.Server` endpoint. When that endpoint receives the subscription request, it will store it locally. In this sample, the [Learning Persistence](/persistence/learning/) storage will be used, but in a production system a database would be used instead. When publishing a message, it can consult the subscriber list and send a copy to every subscriber that expressed interest.


## Running the solution

First, set Client, Server, and Subscriber to be [startup projects](https://msdn.microsoft.com/en-us/library/ms165413.aspx). If the solution was downloaded, this will already be set up.

Run the solution, and three console windows will appear, and NServiceBus will initialize in each one.


### Client Output

The output will be

```
Press enter to send a message
Press any key to exit
```

Hit enter to send a message, or any other key to exit.

```
Sent a PlaceOrder message with id: 5e906f84397e4205ae486f0aa79935e2
```


### Server Output

Note that the Server will indicate that it received a subscription request from the Subscriber:

```
INFO  NServiceBus.SubscriptionReceiverBehavior Subscribing Samples.StepByStep.Subscriber@MACHINENAME to message type OrderPlaced, Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
```

The rest of the output shows the results of sending a message from the Client:

```
Press any key to exit
INFO  PlaceOrderHandler Order for Product:New shoes placed with id: 1c7f01eb-6de3-4506-9e56-a914f9486d9d
INFO  PlaceOrderHandler Publishing: OrderPlaced for Order Id: 1c7f01eb-6de3-4506-9e56-a914f9486d9d
```


### Subscriber Output

Note that the Subscriber will indicate it is sending a subscription request to the Server endpoint:

```
INFO  NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions.SubscriptionManager Subscribing to OrderPlaced, Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null at publisher queue Samples.StepByStep.Server@MACHINENAME
```

```
Press any key to exit
INFO  PlaceOrderHandler Publishing: OrderPlaced for Order Id: 1c7f01eb-6de3-4506-9e56-a914f9486d9d
```
