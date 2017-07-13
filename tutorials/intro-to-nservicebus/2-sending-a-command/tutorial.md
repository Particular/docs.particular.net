---
title: "Introduction to NServiceBus: Sending a command"
reviewed: 2017-01-26
summary: In this 15-20 minute tutorial, you'll learn how to define NServiceBus messages and handlers, and send and receive a message.
redirects:
- tutorials/nservicebus-101/lesson-2
extensions:
- !!tutorial
  nextText: "Next Lesson: Multiple endpoints"
  nextUrl: tutorials/intro-to-nservicebus/3-multiple-endpoints
---

Sending and receiving messages is a central characteristic of any NServiceBus system. Durable messages passed between processes allow reliable communication between those processes, even if one of them is temporarily unavailable. In this lesson we'll show how to send and process a message.

In the next 15-20 minutes, you will learn how to define messages and message handlers, send and receive a message locally, and use the built-in logging capabilities.


## What is a message

A [**message**](/nservicebus/messaging/messages-events-commands.md) is a collection of data sent via one-way communication between two endpoints. In NServiceBus, we define message via simple classes.

In this lesson, we'll focus on [commands](/nservicebus/messaging/messages-events-commands.md#command). In [Lesson 4: Publishing events](../4-publishing-events/) we'll expand to look at events as well.

To define a command, create a class and mark it with the `ICommand` marker interface.

snippet: Command

The marker interface has no implementation, but lets NServiceBus know that the class is a command so that it can build up some metadata about the message type when the endpoint starts up. Any properties you create within the message constitute the message data.

The name of the command class is also important. A command is an order to do something, so it should be named in the [imperative tense](https://en.wikipedia.org/wiki/Imperative_mood). `PlaceOrder` and `ChargeCreditCard` are great names for commands, because they are phrased as a command and are very specific. `PlaceOrder` will place an order and `ChargeCreditCard` will charge money on a credit card. `CustomerMessage`, on the other hand, is not a good example. It is not in the imperative, and it's not very specific either. Another developer should know exactly what a command's purpose is just by reading the name.

Command names should also convey business intent. `UpdateCustomerPropertyXYZ`, while more specific than `CustomerMessage` isn't a good name for an command either, because it's focused only on the data manipulation, rather than the business meaning behind it. `MarkCustomerAsGold`, or something else that is more business-oriented, is probably a better choice.

When sending a message, the endpoint's [serializer](/nservicebus/serialization/) will serialize an instance of the `DoSomething` class and add that to the contents of the outgoing message that goes to the queue. On the other end, the receiving endpoint will deserialize the message back to an instance of the message class so that it can be used in code.

Messages can even contain child objects or collections. (The supported range of structures is dictated by the [choice of serializer](/nservicebus/serialization/#supported-serializers).)

snippet: ComplexCommand

Messages are a contract between two endpoints. Any change to the message will likely involve a change on both the sender and receiver side. The more properties you have on a message, the more reasons it has to change, so keep your messages as slim as possible.

Also, you should not embed logic within your message classes. Each message should contain only automatic properties and not computed properties or methods.It is a good practice to initialize collection properties as shown above, so that you never have to deal with a potentially null collection.

In essence, messages should be carriers for data only. By keeping your messages small and giving them clear purpose, you'll make your code easy to understand and evolve.


## Organizing messages

Messages are data contracts and as such, they are shared between multiple endpoints. Therefore you generally should not put the classes in the same assembly with the endpoints. Instead, they should live in a separate class library.

**Message assemblies** should be entirely self-contained, meaning they should contain only NServiceBus message types, and any supporting types required by the messages themselves. For example, if a message uses an enumeration type for one of its properties, then that enumeration should also live within the same message assembly.

INFO: It is technically possible to embed messages within the endpoint assembly, but then those messages can't be exchanged with other endpoints. Samples will also sometimes embed the messages in order to make the sample easier to understand. In this tutorial, we'll stick to keeping them in dedicated message assemblies.

Additionally, message assemblies should have no dependencies other than libraries included with the .NET Framework, and the NServiceBus core assembly, which is required to reference the `ICommand` interface. 

Following these guidelines will make your message contracts easy to evolve in the future.


## Processing messages

To process a message, we create a [**message handler**](/nservicebus/handlers/), a class that implements `IHandleMessages<T>`, where `T` is a message type. A message handler looks like this:

snippet: EmptyHandler

The implementation of the `IHandleMessages<T>` interface is the `Handle` method, which NServiceBus will invoke when a message of type `T` (in this case `DoSomething`) arrives. The `Handle` method receives the message and an `IMessageHandlerContext` that contains contextual API for working with messages.

Instead of explicitly returning a `Task`, you can add the `async` keyword to a handler method:

snippet: EmptyHandlerAsync

If you want to learn more about working with async methods, check out [Asynchronous Handlers](/nservicebus/handlers/async-handlers.md).

A single class can implement multiple `IHandleMessages<T>` for multiple message types. This allows grouping handlers that are logically related, although a new instance of the class will be created for every message processed.

snippet: MultiHandler

When NServiceBus starts up, it will find all of these message handler classes and automatically wire them up, so that they will be invoked when messages arrive. There's no special setup or configuration needed.

It makes no difference whether handlers are implemented inside one or multiple classes. When NServiceBus starts up, it will find all of these message handlers and automatically wire them up without any special configuration. Use it to group related handlers in order to make your code easier to understand.


## Exercise

Now let's take the solution we started in the last lesson and modify it to send a message. You can also grab the completed solution from the last lesson to use as a starting point.

When we're done, the ClientUI endpoint will be sending a PlaceOrder message to itself, and then processing that message, as depicted in the following diagram:

![Exercise 2 Diagram](diagram.svg)


### Create a messages assembly

To share message between endpoints they need to be self-contained within a separate assembly, so let's create that assembly now.

 1. In the solution, create a new project and select the **Class Library** project type.
 1. Set the name of the project to **Messages**.
 1. Remove the automatically created **Class1.cs** file from the project.
 1. Add the NServiceBus NuGet package to the **Messages** project.
 1. In the **ClientUI** project, add a reference to the **Messages** project.


### Create a message

We'll create our first command in a folder called **Commands**.

 1. In the **Messages** project, create a new class named `PlaceOrder`.
 1. Mark `PlaceOrder` as `public` and implement `ICommand`.
 1. Add a public property of type `string` named `OrderId`.

WARNING: The .NET Framework contains its own interface named `ICommand` in the `System.Windows.Input` namespace. Be sure that if you use tooling to resolve the namespace, you select `NServiceBus.ICommand`. Most of the types you will need will reside in the `NServiceBus` namespace.

When complete, your `PlaceOrder` class should look like the following:

snippet: PlaceOrder


### Create a handler

Now that we've defined a message, we can create a corresponding message handler. For now, let's handle the message locally within the **ClientUI** endpoint.

 1. In the **ClientUI** project, create a new class named `PlaceOrderHandler`.
 1. Mark the handler class as public, and implement the `IHandleMessages<PlaceOrder>` interface.
 1. Add a logger instance, which will allow you to take advantage of the same logging system used by NServiceBus. This has an important advantage over `Console.WriteLine()`: the entries written with the logger will appear in the log file in addition to the console. Use this code to add the logger instance to your handler class:
    ```cs
    static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
    ```
 1. Within the `Handle` method, use the logger to record the receipt of the `PlaceOrder` message, including the value of the `OrderId` message property:
    ```cs
    log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");
    ```
 1. Since everything we have done in this handler method is synchronous, return `Task.CompletedTask`.

When complete, your `PlaceOrderHandler` class should look like this:

snippet: PlaceOrderHandler

INFO: Because `LogManager.GetLogger(..);` is an expensive call, it's important to [always implement loggers as static members](/nservicebus/logging/usage.md).


### Send a message

Now we have a message and a handler to process it. Let's send that message.

In the **ClientUI** project, we are currently stopping the endpoint when we press the Enter key. Instead, let's create a run loop that will allow us to be a little more interactive, so that we can use the keyboard to decide whether to send a message or quit.

Add the following method to the **Program.cs** file:

snippet: RunLoop
 
Let's take a closer look at the case when we want to place an order. In order to create the `PlaceOrder` command simply create an instance of the `PlaceOrder` class and supply a unique value for the `OrderId`. Then, after logging the details, we can send it with the `SendLocal` method.

`SendLocal(object message)` is a method that is available on the `IEndpointInstance` interface, as we are using here, and also on the `IMessageHandlerContext` interface, which we saw when we were defining our message handler. The *Local* part means that we are not sending to an external endpoint (in a different process) so we intend to handle the message in the same endpoint that sent it. Using `SendLocal()`, we don't have to do anything special to tell the message where to go.

NOTE: In this lesson, we're using `SendLocal` (rather than the more commonly used `Send` method) as a bit of a crutch. This way, we can explore how to define, send, and process messages without needing a second endpoint to process them. With `SendLocal`, we also don't need to define routing rules to control where the sent messages go. We'll learn about these concepts [in the next lesson](../3-multiple-endpoints/).

Because `SendLocal()` returns a `Task`, we need to be sure to `await` it properly.

Now let's modify the `AsyncMain` method to call the new `RunLoop` method:

snippet: AddRunLoopToAsyncMain


### Running the solution

Now we can run the solution. Whenever we press `P` on the console, a command message is sent and then processed by a handler class in the same project.

```
INFO  ClientUI.Program Press 'P' to place an order, or 'Q' to quit.
p
INFO  ClientUI.Program Sending PlaceOrder command, OrderId = 1fb61e01-34a3-4562-82b1-85278565b59d
INFO  ClientUI.Program Press 'P' to place an order, or 'Q' to quit.
INFO  ClientUI.PlaceOrderHandler Received PlaceOrder, OrderId = 1fb61e01-34a3-4562-82b1-85278565b59d
p
INFO  ClientUI.Program Sending PlaceOrder command, OrderId = d9e59362-ccf4-4323-8298-4bbc052fb877
INFO  ClientUI.Program Press 'P' to place an order, or 'Q' to quit.
INFO  ClientUI.PlaceOrderHandler Received PlaceOrder, OrderId = d9e59362-ccf4-4323-8298-4bbc052fb877
```

Note how after sending a message, the prompt from `ClientUI.Program` is displayed _before_ the `ClientUI.PlaceOrderHandler` acknowledges receipt of the message. This is because, rather than calling the `Handle` method as a direct method call, the message is sent asynchronously, and then control immediately returns to the `RunLoop`, which repeats the prompt. It isn't until a bit later, when the message is received and processed, that we see the `Received PlaceOrder` notification.


## Summary

In this lesson we learned about messages, message assemblies, and message handlers. We created a message and a handler and we used `SendLocal()` to send the message to the same endpoint.

In the next lesson, we'll create a second messaging endpoint, move our message handler over to it, and then configure the ClientUI to send the message to the new endpoint. We'll also be able to observe what happens when we send messages while the receiver endpoint is offline.
