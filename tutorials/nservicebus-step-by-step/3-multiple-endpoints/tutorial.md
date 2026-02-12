---
title: "NServiceBus Step-by-step: Multiple Endpoints"
reviewed: 2026-02-10
summary: In this 15-20-minute tutorial, you'll learn how to send messages between multiple endpoints and control the logical routing of messages between endpoints.
redirects:
- tutorials/intro-to-nservicebus/3-multiple-endpoints
- tutorials/nservicebus-101/lesson-3
extensions:
- !!tutorial
  nextText: "Next: Publishing events"
  nextUrl: tutorials/nservicebus-step-by-step/4-publishing-events
---

Up until this point, we have constrained our activities to a single endpoint, but this is not how real systems behave. The strength of a messaging system is the ability to run code in multiple processes, on multiple servers, which can all collaborate by exchanging messages.

In this lesson, we'll move our message handler to a different endpoint, and discuss the concepts that go along with running more than one endpoint.

In the next 15-20 minutes, you will learn how to send messages between multiple endpoints and how to control the logical routing of messages between endpoints.

## Sending messages

We've already shown how an endpoint can "send a message to itself" using the `SendLocal()` method, which is available via the `IMessageSession` that we used in the endpoint to create a UI, and also via the `IMessageHandlerContext` that can be access while handling a message.

snippet: SendLocal

Sending a message to another endpoint is exactly the same, we just need to drop the word **Local** from the method name.

snippet: Send

The main difference is that with `SendLocal()`, the destination for the message is already known. So when we call `Send()`, how does NServiceBus know where to send the message?

## Logical routing

We could specify where we want the message to go directly in code. There is actually an overload of the `Send()` method that allows us to do this:

snippet: SendDestination

However, in most cases, this isn't a good idea. It requires that we remember where each message is supposed to go and type it in every time we send that message.

Instead, NServiceBus should be made aware of the routing configuration, so that whenever a message is sent, the framework will know exactly where it should be delivered.

This concept is called **logical routing**, the mapping of specific message types to logical endpoints that can process those messages. Each command message should have one logical endpoint that owns that message and can process it.

We say *logical routing* because this is at a logical layer only, which isn't necessarily the same as *physical routing*. Within one *logical* endpoint, there may be many *physical* endpoint instances deployed to multiple servers.

> [!NOTE]
> An [**endpoint**](/nservicebus/concepts/glossary.md#endpoint) is a logical concept, defined by an endpoint name and associated implementation, that defines an owner responsible for processing messages.
>
> An [**endpoint instance**](/nservicebus/concepts/glossary.md#endpoint-instance) is a physical instance of the endpoint deployed to a single server. Many endpoint instances may be deployed to many servers in order to scale out the processing of a high-volume message to multiple servers.
>
> The `IMessageSession` API provides basic message operations.

For now, we'll only concern ourselves with logical routing, and leave the rest of it (physical routing, scale-out, etc.) for a later time.

Because logical routing does not cover physical concerns, but only defines logical ownership, this is something that we (as the implementers) should control, and is not an Operations (as in the part of our organization that handles the underlying hardware infrastructure) concern. While Operations may want to be able to move an endpoint to a different server using only configuration files, changing the owner for messages would require code changes and a recompile/redeploy anyway.

Therefore, it makes sense that logical routing is defined in code.

### Defining logical routes

[**Message routing**](/nservicebus/messaging/routing.md) is a function of the message transport, so all routing functionality is accessed from the `RoutingSettings<T>` object returned when we defined the message transport, as shown in this example using the Learning Transport:

snippet: RoutingSettings

`RoutingSettings<T>` is scoped to the transport being used, and routing options are exposed as extension methods on this class. Therefore, only routing options that are viable for the transport in use will appear. Routing configurations only applicable to Microsoft Azure, for example, won't clutter up the API when using the Learning Transport.

In order to define routes, start with the `routing` variable and call the `RouteToEndpoint` method as needed, which comes in three varieties:

snippet: RouteToEndpoint

For now, we will use the first overload, specifying individual message types.

## Exercise

Let's split apart the endpoint we created in the previous lesson. We'll reconfigure our solution so that the **ClientUI** endpoint sends the `PlaceOrder` command to a new endpoint that we'll call **Sales**. Sales will become the true logical owner of the `PlaceOrder` command, and we'll get to see NServiceBus send a message from one endpoint to another. For that reason, we will also rename the **Messages** project to **Sales.Messages** allowing us in future steps to add messages belonging to other services into their own dedicated projects.

![Exercise 3 Diagram](diagram.svg)

### Creating a new endpoint

First, let's create a project for our new endpoint.

 1. Create a new **Console Application** project named **Sales**.
 1. In the same directory as the **Sales** project, add the following NuGet packages using the .NET CLI:
      ```
    dotnet add package NServiceBus
    dotnet add package NServiceBus.Extensions.Hosting
      ```
 1. Rename **Messages** project to **Sales.Messages**
 1. In the **Sales** project, add a reference to the **Sales.Messages** project, so that we have access to the `PlaceOrder` message.

### Configuring an endpoint

Now that we have a project for our **Sales** endpoint, we need to add similar code to configure and start an NServiceBus endpoint:

snippet: SalesConsoleApp

Most of this configuration looks exactly the same as our **ClientUI** endpoint. It's critical for the configuration between endpoints to match (especially message transport and serializer); otherwise, the endpoints would not be able to understand each other.

For example, if the **ClientUI** endpoint used `.UseSerialization<XmlSerializer>()` while the **Sales** endpoint used `.UseSerialization<JsonSerializer>()`, the **Sales** endpoint would not be able to understand the XML-serialized messages it received from **ClientUI** since it would be expecting JSON.

> [!TIP]
> It's also possible to specify [multiple deserializers](/nservicebus/serialization/#specifying-additional-deserializers) to enable receiving messages serialized in different formats, for instance, to enable integration between teams, or to enable the use of a high-performance serializer in a performance-critical subsystem.

While most of the configuration is the same, notice two specific lines that are different:

snippet: EndpointDifferences

The difference, of course, is the name "Sales" in the console title and `EndpointConfiguration` constructor, which defines the endpoint name for the **Sales** endpoint and gives it its own identity.

This means that the **Sales** endpoint will create its own queue named `Sales` where it will listen for messages. We now have two processes that each have their own queues, so now we can send messages between them.

> [!NOTE]
> This is quite repetitive, but remember that this is still an introductory exercise. There are various methods, such as the [INeedInitialization interface](/nservicebus/lifecycle/ineedinitialization.md) which allow for centralizing the repetitive endpoint configuration code.

### Debugging multiple projects

At this point, we could run the **Sales** endpoint, although we wouldn't expect **Sales** to do anything except start up, create its queues, and then wait for messages that would never arrive. This is a good exercise to do, although you can skip it if you're in a hurry.

However, it's common in NServiceBus solutions to run multiple projects (i.e. endpoints) at once. To make this easier, configure both endpoints (**ClientUI** and **Sales**) to run at startup using Visual Studio's [multiple startup projects](https://learn.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?view=visualstudio) feature.

If you run the project now, **ClientUI** will work just as it did before, and **Sales** will start up and wait for messages that will never arrive.

### Moving the handler

Now let's move the handler from **ClientUI** over to **Sales** where it belongs.

 1. In the Solution Explorer, find **PlaceOrderHandler.cs** in the **ClientUI** project, and drag it to the **Sales** project.
 1. Open the new **PlaceOrderHandler.cs** in **Sales** and change the namespace from `ClientUI` to `Sales` to match its new home.
 1. Visual Studio's default action when you drag files between projects is to copy them, so you must delete the old **PlaceOrderHandler.cs** from the **ClientUI** endpoint.

Now that the handler is in the correct endpoint, what would happen if we started the solution? **Sales** now has a message handler, but recall that ClientUI is still calling `endpointInstance.SendLocal(command)` which effectively sends the message to itself. But it doesn't have a handler for this message anymore.

If you attempt to place an order in **ClientUI**, an exception will be thrown because **ClientUI** no longer has a handler for it:

> [!WARNING]
> System.InvalidOperationException: No handlers could be found for message type: Messages.Commands.PlaceOrder

In fact, you will probably get a giant wall of exception text, because the message is tried and retried, and then retried some more after successively longer delays, until finally failing for good sometime later. We'll cover this behavior in detail in [Part 5: Retrying errors](../5-retrying-errors/).

The important takeaway is, if a message is accidentally sent to an endpoint we didn't intend, it won't just fail silently, and the message will not be lost.

### Sending to another endpoint

Now we need to change **ClientUI** so that it is sending `PlaceOrder` to the **Sales** endpoint.

1. In the **ClientUI** endpoint, modify the **Program.cs** file so that `messageSession.SendLocal(command, stoppingToken)` is replaced by `messageSession.Send(command, stoppingToken)`.
1. In the **Program.cs** file, use the `routing` variable to access the routing configuration and specify the logical routing for `PlaceOrder` by adding the following code after the line that configures the Learning Transport:

snippet: AddingRouting

This establishes that commands of type `PlaceOrder` should be sent to the **Sales** endpoint.

> [!IMPORTANT]
> As noted in [configuring an endpoint](#exercise-configuring-an-endpoint), ensure the configuration (e.g. message transport and serializers used) between endpoints match.

### Running the solution

Now when we run the solution, we get two console windows, one for **ClientUI** and one for **Sales**. After moving the windows around so that we can see both, we can try to place an order by pressing <kbd>P</kbd> in the **ClientUI** window.

> [!NOTE]
> You can also keep console windows from showing up in random screen locations each time by right-clicking the console window's title bar, and in the **Layout** tab, unchecking the **Let system position window** checkbox.

In the **ClientUI** window, we see this output:

```
 info: ClientUI.InputLoopService[0]
       Press 'P' to place an order, or 'Q' to quit.
 p
 info: ClientUI.InputLoopService[0]
       Sending PlaceOrder command, OrderId = 0124c1d5-8eb9-43f7-85e4-2c3ef6081464
 info: ClientUI.InputLoopService[0]
       Press 'P' to place an order, or 'Q' to quit.
 p
 info: ClientUI.InputLoopService[0]
       Sending PlaceOrder command, OrderId = 7c833457-a8a3-4c45-bc01-b30f09c11db0
 info: ClientUI.InputLoopService[0]
       Press 'P' to place an order, or 'Q' to quit.
```

Everything is the same, except the command is not processed here.

In the **Sales** window, we see:

```
 Press Enter to exit.
 info: Sales.PlaceOrderHandler[0] Received PlaceOrder, OrderId = 0124c1d5-8eb9-43f7-85e4-2c3ef6081464

 info: Sales.PlaceOrderHandler[0] Received PlaceOrder, OrderId = 7c833457-a8a3-4c45-bc01-b30f09c11db0
```

At this point, we've managed to create two processes and achieve inter-process communication between them. Now let's try something different.

1. In Visual Studio's **Debug** menu, select **Detach All** so that we can close one console window without Visual Studio closing all the other windows as well. Alternatively, you can run the solution using **Debug** > **Start Without Debugging** or <kbd>Ctrl</kbd> + <kbd>F5</kbd>.
1. Close the **Sales** endpoint window so that only **ClientUI** is running.
1. Press <kbd>P</kbd> several times to send several messages to the **Sales** endpoint. Note that it works just fine; messages are sent, and nothing fails because the **Sales** endpoint happens to be offline.
1. Restart the **Sales** endpoint by right-clicking the **Sales** project and selecting **Debug** > **Start new instance**.

After **Sales** starts up, it receives and processes all the messages that were waiting for it in the queue.

The value in this approach is the ability to take a part of your system offline and have the rest of it proceed normally as though nothing is wrong, and then have everything return to normal when the offline piece comes back online.

## Summary

In this lesson, we learned about sending messages between endpoints. We already knew the basics of how to send and handle messages, but we learned how to control the logical message routing so that when we send a message, the system will know where that message should go.

In the next lesson, we'll learn about events, a different kind of message that can be published to multiple subscribers using the Publish/Subscribe pattern. We'll also learn how the decoupling provided by this pattern allows us to structure our distributed systems in a more logical and maintainable way.
