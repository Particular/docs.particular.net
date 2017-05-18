---
title: "NServiceBus Quick Start"
reviewed: 2017-05-05
summary: TODO
---

In this tutorial, you'll see firsthand how a software system built on asynchronous messaging using NServiceBus is superior to integrating with HTTP-based web services. You'll discover how using NServiceBus gives you the advantages of reliability and extensibility you just can't achieve building services as REST endpoints.

This tutorial will skip over some of the concepts and implementation details. If you'd prefer to dig in and really start to learn how to use NServiceBus, check out the [Introduction to NServiceBus](/tutorials/intro-to-nservicebus/). It will teach you the NServiceBus API as well as the important concepts of message-based systems you'll want to learn in order to build successful software systems.

To get started, download the solution above, extract the archive, and then open the **Before/RetailDemo.sln** file with Visual Studio 2015 or later.


## Project structure

The solution contains four projects. The **ClientUI**, **Sales**, and **Billing** projects are [messaging endpoints](/nservicebus/endpoints/), or processes that communicate with each other using NServiceBus messages. Each endpoint references the **Messages** assembly, which contains the definitions of messages as simple class files.

As shown in this diagram, the **ClientUI** endpoint, which serves as a stand-in for a web application in a more realistic example, will send a **PlaceOrder** command to the **Sales** endpoint. As a result, the **Sales** endpoint will publish an **OrderPlaced** event using the publish/subscribe pattern, which will be received by the **Billing** endpoint.

![Initial Solution](before.svg)

The solution mimics a real-life retail system, where [the command to place an order](/nservicebus/messaging/messages-events-commands.md#command) is sent as a result of a customer interaction, and the actual processing occurs in the background. By publishing an [event](/nservicebus/messaging/messages-events-commands.md#event), the code to bill the credit card is isolated from the code to place the order, reducing coupling and making the system easier to maintain over the long term. Later in this tutorial, we'll see how to add a second subscriber in a **Shipping** endpoint which would begin the process of shipping the order.


## Running the solution

The solution is configured to have [multiple startup projects](https://msdn.microsoft.com/en-us/library/ms165413.aspx), so when you run the solution, it should open three console applications, one for each messaging endpoint.

In the **ClientUI** application, press `P` to place an order, and watch how the other windows react. It may happen too quickly for you to see, but **PlaceOrder** will be sent to **Sales** resulting in:

    INFO  Sales.PlaceOrderHandler Received PlaceOrder, OrderId = 9b16a5ce-e6ae-4447-a911-b7d6e265a1f0

As a result, the **OrderPlaced** event will be published, and the **Billing** window you will see:

    INFO  Billing.OrderPlacedHandler Billing has received OrderPlaced, OrderId = 9b16a5ce-e6ae-4447-a911-b7d6e265a1f0

Try pressing `P` repeatedly in the **ClientUI** window and watch the messages flow between endpoints.


## Chaos Monkey

One of the most powerful features of asynchronous messaging is that failures in one part of a system don't propagate and bring the whole system down. Let's give that a try.

1. Run the solution in Visual Studio and ensure all three console windows are active.
1. Close the **Billing** window.
1. Send several messages by pressing `P` in the **ClientUI** window.
1. Notice how messages are still flowing from **ClientUI** to **Sales**. **Sales** is still publishing messages too, you just can't see them being delivered right now.
1. Restart the **Billing** application by right-clicking the **Billing** project in Visual Studio's Solution Explorer, then selecting **Debug** > **Start new instance**.

When the **Billing** endpoint starts back up, you'll see it quickly catch up on the backlog of messages that was waiting for it, completing the process for all of the orders that were waiting to be billed.

Let's consider more carefully what's going on here. First, we have inter-process communication occurring with very little ceremony. More importantly, it didn't break down when the **Billing** service was offline. If we had implemented **Billing** as a REST endpoint, the **Sales** service would have thrown an HTTP exception when it was unable to communicate with the server, and *the data in that message would have been lost forever*. By using NServiceBus, we get a new expectation of reliability, with a guarantee that even if message processing endpoints are temporarily unavailable, every message will get delivered and processed eventually.


## Protection from transient errors

Have you ever had a business process get interrupted by a transient error like a database deadlock? Usually this leaves a system in an inconsistent state, like the order persisted to the database but not yet submitted to the payment processor. As a developer, you might have to dive directly into the database like a forensic analysis, trying to figure out where the process went wrong, and how to manually jump-start it so that the process can complete.

With NServiceBus, this doesn't have to happen. Each message handler will automatically retry processing messages if an exception is thrown, so transient failures like database deadlocks are handled gracefully, without manual intervention by a developer.

Let's introduce a transient failure into the **Sales** endpoint and see this in action.

1. In the **Sales** endpoint, locate and open the **PlaceOrderHandler.cs** file.
1. Uncomment the code inside the **ThrowTransientException** region shown here. This will cause an exception to be thrown 20% of the time a message is processed:

snippet: ThrowTransientException

3. Start the solution without debugging (Ctrl+F5), or alternatively, start the solution and then select **Detach All** in the **Debug** menu. This will make it easier to observe exceptions occurring without being interrupted by Visual Studio's Exception Assistant.
3. In the **ClientUI** window, slowly start sending one message at a time by pressing `P`, and be sure to watch the **Sales** window.

As you watch the **Sales** window, 80% of the messages will go through as normal, but when an exception occurs, the output will be different:

    INFO  NServiceBus.RecoverabilityExecutor Immediate Retry is going to retry message '43400b29-c235-471f-ab4f-a7760145ea88' because of an exception:
    System.Exception: Oops
       at <long stack trace>
    INFO  Sales.PlaceOrderHandler Received PlaceOrder, OrderId = e1d86cb9-c393-475b-9be0-5407e9e529e0

By using automatic retries, you can avoid losing data or having your system left in an inconsistent state on account of a stray database deadlock. No more spelunking through the database trying to fix business processes gone wrong!


## Easy to extend

As mentioned previously, publishing events using the Publish/Subscribe pattern reduces coupling and makes maintaining a system easier over the long term. Let's look at how you can add an additional subscriber without needing to modify any of the existing code.


### Creating a new endpoint

Let's create a new messaging endpoint called **Shipping** that will also subscribe to the `OrderPlaced` event. First we'll create the project and set up its dependencies:

1. In the solution, create a new **Console Application** project named **Shipping**.
1. In the **Shipping** project, add the NServiceBus NuGet package, which is already present in the other projects in the solution:
    ```
    Install-Package NServiceBus -ProjectName Shipping
    ```
1. In the **Shipping** project, add a reference to the **Messages** project, so that we have access to the `OrderPlaced` event.

Now that we have a project for the Shipping endpoint, we need to add some code to configure and start an NServiceBus endpoint:

snippet: ShippingProgram

You'll want the Shipping endpoint to run when you debug the solution, so use Visual Studio's [multiple startup projects](https://msdn.microsoft.com/en-us/library/ms165413.aspx) to configure the Shipping endpoint to start along with ClientUI, Sales, and Billing.


### Creating a new message handler

Continuing here...


## Summary

Summarize, link to CTA

 -- idea from Weronika - link to "All my errors are severe"?