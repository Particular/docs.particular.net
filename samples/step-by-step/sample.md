---
title: Step by Step Guide
summary: Get started with NServiceBus
reviewed: 2016-03-21
component: Core
redirects:
- nservicebus/nservicebus-step-by-step-guide
---

This sample illustrates most of the basic NServiceBus concepts in a step-by-step fashion. You can either download the code from the link above and run it, or you can build it yourself following the instructions here.

## What you'll learn

When complete, this sample will show a very simple ordering system that:

 * sends a command from a client to a server
 * that server handles the command and publishes a new event about the success
 * a subscriber listens to, and handles, the event

By completing these steps, you will have received an introduction to many important NServiceBus concepts:

* Basic project setup
* Self-hosting NServiceBus within a console application
* Defining and sending commands
* Handling messages
* Defining and publishing events
* Logging

## Project structure

The finished solution will contain four projects. Create a new Visual Studio solution and create the following:

* A Console Application named `Client`: The client's job will be to send `PlaceOrder` commands to the Server.
* A Console Application named `Server`: The server's job will be to handle the `PlaceOrder` command, and then publish an `OrderPlaced` event to any interested subscribers.
* A Console Application named `Subscriber`: The subscriber will subscribe to 
* A Class Library named `Shared`.

The client's job will be to send `PlaceOrder` commands to the server. The server will process the `PlaceOrder` commands, and then publish an `OrderPlaced` event to any interested subscribers. The subscriber will subscribe to `OrderPlaced` events published from the server, and process those events when they arrive.

The shared project will be referenced by all of the applications and serve as a central location to store message definition classes and common pieces of configuration. This is not a best practice to combine all these things in one place, but serves to illustrate how things work for this simple example.

### Setting dependencies

Before beginning, set up the necessary dependencies.

* Add a reference to the `Shared` project to the `Client`, `Server`, and `Subscriber` projects.
* In each project, add a reference to the `NServiceBus` NuGet package, either using the Package Manager Console or the NuGet Package Manager visual tool. This sample contains examples covering all supported major versions, so take a moment to note what version you are using and ensure you refer to the correct code snippets going forward.


## Defining messages

First we need to define the messages that will be used by the system. The client will send a `PlaceOrder` command to the server, and in response, the server will publish an `OrderPlaced` event.

There is a difference between commands and events. A **command** is a request to perform a task, which is sent to one specific location. In contrast, an **event** is an announcement that something has happened, which is published from one location and can be consumed by multiple subscribers.

The easiest way to differentiate commands and events is to use the `ICommand` and `IEvent` marker interfaces.

Let's define the `PlaceOrder` command by creating a class that implements `ICommand` in the Shared project called `PlaceOrder`:

snippet:PlaceOrder

Next, define the `OrderPlaced` event by creating a class that implements `IEvent` in the Shared project called `OrderPlaced`:

snippet:OrderPlaced


## The Client

Next we need to get the Client application ready to send messages with NServiceBus. In your Client application, add the following code to the program's Main method. Don't worry about the `SendOrder` method, we will get to that soon.

snippet:ClientInit

With a `PlaceOrder` command defined, and NServiceBus initialized, we can now create a loop to send a new command message every time we press the Enter key.

In the Client application, add this code to the Program class:

snippet:SendOrder

In order to actually run, each endpoint needs to specify an error queue. This defines where messages are sent when they cannot be processed due to repetitive exceptions during message processing.

While it is possible to [configure the error queue in an App.config file](nservicebus/errors), we can use the `IProvideConfiguration` interface to [override app.config settings](nservicebus/hosting/custom-configuration-providers), which has the benefit able to be shared between all our endpoints.

In the Shared project, create a class named `ConfigErrorQueue`:

snippet:ConfigErrorQueue

The Client application is now complete, which we could run. However, doing so would throw an exception when trying to send the message.

From looking at the code, you may notice that it is sending the `PlaceOrder` command to an endpoint named `Samples.StepByStep.Server`, which will not exist yet. We need to create the server to handle that command.


## The Server

Like our client, the Server application is going to need to be configured as an NServiceBus endpoint as well.

In the Server application, add the following code to the program's Main method:

snippet:ServerInit

In the previous snippet, notice the change in endpoint name, which differentiates this endpoint from the Client.

Next, we need to create a handler for the `PlaceOrder` command being sent by the Client.

Create a new class in the Server project named `PlaceOrderHandler` using the following code:

snippet:PlaceOrderHandler

The handler class is automatically discovered by NServiceBus because it implements the `IHandleMessages<T>` interface. The dependency injection system (which supports constructor or property injection) injects the `IBus` instance into the handler to access messaging operations.

When a `PlaceOrder` command is received, NServiceBus will create a new instance of the `PlaceOrderHandler` class and invoke the `Handle(PlaceOrder message)` method. Normally a handler is where we would take the information from the message and save it to a database, or call a web service, or some other business function, but in this example, we simply log that the message was received.

Next, the handler publishes a new `OrderPlaced` message. This won't actually send any messages, because we don't have any subscribers yet. The next step is to create a subscriber for this message.


## The Subscriber

Like the client and the server, the Subscriber application also needs to be configured as an NServiceBus endpoint.

In the Subscriber application, add the following code to the program's Main method:

snippet:SubscriberConfig

This is almost identical to the Server configuration, except for the different endpoint name.

Next we create a message handler for the `OrderPlaced` event. Note that whether handling a command or an event, the syntax for a message handler remains the same.

Create a new class in the Subscriber project named `OrderCreatedHandler` using the following code:

snippet:OrderCreatedHandler

Here we are only logging the fact that the message was received. In a real system, a subscriber to `OrderPlaced` could charge a credit card for the order, start to prepare the order for shipment, or even update information in a customer loyalty database to track when the customer is eligible for different purchase-driven incentives. The fact that these disparate tasks can be accomplished in completely separate message handlers instead of one monolithic process is one of the many strengths of the Publish/Subscribe messaging pattern.

Next, we need to let the message publisher know that we are interested in being notified when `OrderPlaced` events are published. To do that we need to [configure subscriptions in XML](nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed).

In the Subscriber application, create an App.config file and add the following XML configuration:

snippet:subscriptionConfig

The important part is in the `MessageEndpointMappings` section. The `add` directive says that for messages of type `OrderPlaced` within the `Shared` assembly, subscription requests should be sent to the `Samples.StepByStep.Server` endpoint, which is the endpoint that is responsible for publishing `OrderPlaced` events.

When the Subscriber endpoint initializes, it will read this configuration, and knowing that it contains a message handler for `OrderPlaced`, it will send a special subscription message to the `Samples.StepByStep.Server` endpoint. When that endpoint receives the subscription request, it will store it locally (in this sample, using in-memory storage, but in a production system a database would be used) so that when it publishes a message, it can send a copy to every subscriber that expressed interest.


## Running the solution

If you have built the solution yourself, you need to configure it so that multiple projects will start when you debug.

1. Right-click the solution file, and select **Properties**.
2. In the Property Pages dialog, select **Common Properties** > **Startup Project**.
3. Select the **Multiple startup projects** radio button.
4. Set the value in the **Action** column to **Start** for the Client, Server, and Subscriber projects.

If you downloaded the solution, this will be set up for you.

Run the solution, and three console windows will appear, and NServiceBus will initialize in each one.


### Client Output

The output will be

    Press enter to send a message
    Press any key to exit

Hit enter to send a message, or any other key to exit.

    Sent a new PlaceOrder message with id: 5e906f84397e4205ae486f0aa79935e2


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
