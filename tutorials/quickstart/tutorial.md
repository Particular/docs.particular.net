---
title: "NServiceBus Quick Start"
reviewed: 2018-03-20
summary: See why software systems built on asynchronous messaging using NServiceBus are superior to traditional synchronous HTTP-based web services.
extensions:
- !!tutorial
  nextText: "Next: NServiceBus from the ground up"
  nextUrl: tutorials/nservicebus-step-by-step/1-getting-started
---

include: quickstart-tutorial-intro-paragraph

This tutorial skips over some concepts and implementation details in order to get up and running quickly. If you'd prefer to go more in-depth, check out our [NServiceBus step-by-step tutorial](/tutorials/nservicebus-step-by-step/). It will teach you the NServiceBus API and important concepts you need to learn to build successful message-based software systems.

## Download solution

To get started, download the solution appropriate for your Visual Studio version, extract the archive, and then open the **RetailDemo.sln** file.

<div class="text-center inline-download hidden-xs">
  <a href="https://liveparticularwebstr.blob.core.windows.net/media/tutorials-quickstart-vs2015.zip" class="btn btn-info btn-lg" onclick="return fireGAEvent('TutorialDownloaded', 'quickstart-tutorials-quickstart-vs2015.zip')"> <span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span> Download for Visual Studio 2015</a>
  <a href="https://liveparticularwebstr.blob.core.windows.net/media/tutorials-quickstart.zip" class="btn btn-primary btn-lg" onclick="return fireGAEvent('TutorialDownloaded', 'quickstart-tutorials-quickstart.zip')"> <span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span> Download for Visual Studio 2017</a>
</div>
<style type="text/css">
  /* Hide native tutorial download button for VS2015/2017 test */
  .tutorial-actions .btn-default { display: none; }
  /* Remove borders on images as they all have appropriate borders */
  img.center { border-style: none !important; }
</style>


## Project structure

The solution contains four projects. The **ClientUI**, **Sales**, and **Billing** projects are [endpoints](/nservicebus/endpoints/) that communicate with each other using NServiceBus messages. The **ClientUI** endpoint is implemented as a web application and is an entry point in our system. The **Sales** and **Billing** endpoints, implemented as console applications, contain business logic related to processing and fulfilling orders. Each endpoint references the **Messages** assembly, which contains the definitions of messages as POCO class files.

![Solution Explorer view](solution-explorer.png "width=240")

As shown in the diagram below, the **ClientUI** endpoint sends a **PlaceOrder** command to the **Sales** endpoint. As a result, the **Sales** endpoint will publish an **OrderPlaced** event using the publish/subscribe pattern, which will be received by the **Billing** endpoint.

![Initial Solution](before.svg "width=680")

The solution mimics a real-life retail system, where [the command](/nservicebus/messaging/messages-events-commands.md) to place an order is sent as a result of a customer interaction, and the processing occurs in the background. Publishing [an event](/nservicebus/messaging/messages-events-commands.md) allows us to isolate the code to bill the credit card from the code to place the order, reducing coupling and making the system easier to maintain over the long term. Later in this tutorial, we'll see how to add a second subscriber in a new **Shipping** endpoint which would begin the process of shipping the order.


## Running the solution

The solution is configured to have [multiple startup projects](https://msdn.microsoft.com/en-us/library/ms165413.aspx), so when you run the solution it should open the web application and two console applications, one window for each messaging endpoint.

![ClientUI Web Application](webapp-start.png)
![2 console applications, one for endpoint implemented as a console app](2-console-windows.png)

In the **ClientUI** web application, click the **Place order** button to place an order, and watch what happens in other windows. 

It may happen too quickly to see, but the **PlaceOrder** command will be sent to the **Sales** endpoint. In the **Sales** endpoint window you should see:

```
INFO  Sales.PlaceOrderHandler Received PlaceOrder, OrderId = 9b16a5ce
INFO  Sales.PlaceOrderHandler Publishing OrderPlaced, OrderId = 9b16a5ce
```

As shown in the log, the **Sales** endpoint then publishes an **OrderPlaced** event, which will be received by the **Billing** endpoint. In the **Billing** endpoint window you should see:

```
INFO  Billing.OrderPlacedHandler Billing has received OrderPlaced, OrderId = 9b16a5ce
```

In the **ClientUI** web application, go back and send more messages, watching the messages flow between endpoints.

![Messages flowing between endpoints](messages-flowing.png)


## Reliability

One of the most powerful advantages of asynchronous messaging is reliability. Failures in one part of a system aren't propagated and don't bring the whole system down. 

See how that is achieved by following these steps:

1. Stop the solution (if you haven't already) and then in Visual Studio's **Debug** menu, select **Start Without Debugging** or use <kbd>Ctrl</kbd>+<kbd>F5</kbd>. This will allow us to stop one endpoint without Visual Studio closing all three.
2. Close the **Billing** window.
3. Send several messages using the button in the **ClientUI** window.
4. Notice how messages are flowing from **ClientUI** to **Sales**. **Sales** is still publishing messages, even though **Billing** can't process them at the moment.

![ClientUI and Sales processing messages while Billing is shut down](billing-shut-down.png)

5. Restart the **Billing** application by right-clicking the **Billing** project in Visual Studio's Solution Explorer, then selecting **Debug** > **Start new instance**.

When the **Billing** endpoint starts, it will pick up messages published earlier by **Sales** and will complete the process for orders that were waiting to be billed.

![Billing endpoint processing through backlog](billing-processing-backlog.png)

Let's consider more carefully what happened. First, we had two processes communicating with each other with very little ceremony. The communication didn't break down even when the **Billing** service was unavailable. If we had implemented **Billing** as a REST endpoint, the **Sales** service would have thrown an HTTP exception when it was unable to communicate with it and *that request would have been lost*. By using NServiceBus we get a guarantee that even if message processing endpoints are temporarily unavailable, every message will eventually get delivered and processed.


## Transient failures

Have you ever had business processes get interrupted by transient errors like database deadlocks? Transient errors often leave a system in an inconsistent state. For example, the order could be persisted in the database but not yet submitted to the payment processor. In such a situation you might have to investigate the database like a forensic analyst, trying to figure out where the process went wrong, and how to manually jump-start it so that the process can complete.

With NServiceBus you don't need manual interventions. If an exception is thrown, then the message handler will automatically retry processing it. That addresses transient failures like database deadlocks, connection issues across machines, file write access conflicts, etc.

Let's simulate a transient failure in the **Sales** endpoint and see retries in action:

1. In the **Sales** endpoint, locate and open the **PlaceOrderHandler.cs** file.
1. Uncomment the code inside the **ThrowTransientException** region shown here. This will cause an exception to be thrown 20% of the time a message is processed:

snippet: ThrowTransientException

3. Start the solution without debugging (<kbd>Ctrl</kbd>+<kbd>F5</kbd>). This will make it easier to observe exceptions occurring without being interrupted by Visual Studio's Exception Assistant.
4. In the **ClientUI** window, send one message at a time, and watch the **Sales** window.

![Transient exceptions](transient-exceptions.png)

As you can see in the **Sales** window, 80% of the messages will go through as normal, but when an exception occurs, the output will be different. The first attempt of `PlaceOrderHandler` will throw and log an exception, but then in the very next log entry, processing will be retried and likely succeed.

```
INFO  NServiceBus.RecoverabilityExecutor Immediate Retry is going to retry message '5154b012-4180-4b56-9952-a90a01325bfc' because of an exception:
System.Exception: Oops
    at <long stack trace>
INFO  Sales.PlaceOrderHandler Received PlaceOrder, OrderId = e1d86cb9
```

NOTE: If you forgot to detach the debugger, you'll need to click the **Continue** button in the Exception Assitant dialog before the message will be printed in the **Sales** window.

5. Comment the code inside the **ThrowTransientException** region, so no exceptions are thrown in the future.

Automatic retries allow us to avoid losing data or having our system left in an inconsistent state because of a random transient exception. We won't need to manually dig through the database to fix things anymore!

Of course, there are other exceptions that may be harder to recover from than simple database deadlocks. NServiceBus contains more [recoverability tools](/nservicebus/recoverability/) to handle various types of failures and ensure that no message is ever lost.


## Extending the system

As mentioned previously, publishing events using the [Publish-Subscribe pattern](/nservicebus/messaging/publish-subscribe/) reduces coupling and makes maintaining a system easier in the long run. Let's look at how we can add an additional subscriber without needing to modify any existing code.

As shown in the diagram, we'll be adding a new messaging endpoint called **Shipping** that will also subscribe to the `OrderPlaced` event.

![Completed Solution](after.svg "width=680")


### Create a new endpoint

First we'll create the **Shipping** project and set up its dependencies.

To start, in the **Solution Explorer** window, right-click the **RetailDemo** solution and select **Add** > **New Project**.

![New Project Dialog](new-project.png "width=680")

1. In the **Add New Project** dialog, be sure to select at least **.NET Framework 4.6.1** in the dropdown menu at the top of the window for access to the `Task.CompletedTask` API.
1. Select a new **Console App (.NET Framework)** project (or just **Console Application**).
1. Name the project **Shipping**.
1. Click **OK** to create the project and add it to the solution.

{{NOTE:
**ProTip:** The existing projects in this solution are using the newer, leaner, .NET Core style project files, but the current Visual Studio tooling doesn't make it very easy to do the same for the **Shipping** project. If you like, you can create a project of type **Console App (.NET Core)** and then manually edit the **Shipping.csproj** file to change the `TargetFramework` value from `netcoreapp2.0` to `net461`.

Creating a **Console App (.NET Framework)** project which uses the older-style `*.csproj` file will work just fine, but will just look slightly different in Visual Studio, with nested **Properties**, **References**, and **packages.config** items instead of **Dependencies**.
}}

By default, Visual Studio will create the project using C# 7.0. Let's change it to C# 7.1 so that we can use nice features like [an async Main method](https://blogs.msdn.microsoft.com/mazhou/2017/05/30/c-7-series-part-2-async-main/):

1. In the **Solution Explorer**, right-click on the **Shipping** project and choose **Properties**.
1. Switch to the **Build** tab.
1. Under the **Output** heading, click the **Advanced…** button in the far lower-right corner.
1. Change **Language version** to **C# latest minor version (latest)**.
1. Click **OK**.
1. Save and close the **Shipping** properties page.

![Change Language Version](change-language-version.png "width=680")

Now, we need to add references to the NServiceBus package and Messages project.

1. In the newly created **Shipping** project, add the `NServiceBus` NuGet package, which is already present in the other projects in the solution. In the Package Manager Console window type:
    ```
    Install-Package NServiceBus -ProjectName Shipping
    ```
1. In the **Shipping** project, add a reference to the **Messages** project, so that we have access to the `OrderPlaced` event.

Now that we have a project for the Shipping endpoint, we need to add some code to configure and start an `NServiceBus` endpoint. In the **Shipping** project, find the auto-generated **Program.cs** file and replace its contents with:

snippet: ShippingProgram

You'll want the **Shipping** endpoint to run when you debug the solution, so use Visual Studio's [multiple startup projects](https://msdn.microsoft.com/en-us/library/ms165413.aspx) feature to configure the **Shipping** endpoint to start along with **ClientUI**, **Sales**, and **Billing**.


### Create a new message handler

Next, we need a message handler to process the `OrderPlaced` event. When NServiceBus starts, it will detect the message handler and handle subscribing to the event automatically.

To create the message handler:

1. In the **Shipping** project, create a new class named `OrderPlacedHandler`.
1. Mark the handler class as public, and implement the `IHandleMessages<OrderPlaced>` interface.
1. Add a logger instance, which will allow us to take advantage of the logging system used by NServiceBus. This has an important advantage over `Console.WriteLine()`: the entries written with the logger will appear in the log file in addition to the console. Use this code to add the logger instance to the handler class:
    ```cs
    static ILog log = LogManager.GetLogger<OrderPlacedHandler>();
    ```
1. Within the `Handle` method, use the logger to record when the `OrderPlaced` message is received, including the value of the `OrderId` message property:
    ```cs
    log.Info($"Shipping has received OrderPlaced, OrderId = {message.OrderId}");
    ```
1. Since everything we have done in this handler method is synchronous, return `Task.CompletedTask`.

When complete, the `OrderPlacedHandler` class should look like this:

snippet: OrderPlacedHandler


### Run the updated solution

Now run the solution, and assuming you remembered to [update the startup projects](https://msdn.microsoft.com/en-us/library/ms165413.aspx), a window for the **Shipping** endpoint will open in addition to the other two.

![Addition of Shipping endpoint](add-shipping-endpoint.png)

As you place orders by clicking the button in the **ClientUI** window, you will see the **Shipping** endpoint reacting to `OrderPlaced` events:

```
INFO Shipping.OrderPlacedHandler Shipping has received OrderPlaced, OrderId = 25c5ba63
```

**Shipping** is now receiving events published by **Sales** without having to change the code in the **Sales** endpoint. Additional subscribers could be added, for example, to email a receipt to the customer, notify a fulfillment agency via a web service, update a wish list or gift registry, or update data on items that are frequently bought together. Each business activity would occur in its own isolated message handler and doesn't depend on what happens in other parts of the system.


## Summary

In this tutorial, we explored the basics of how a messaging system using NServiceBus works.

We learned that asynchronous messaging failures in one part of a system can be isolated and prevent the entire system failure. That level of resilience and reliability is not easy to achieve with traditional REST-based web services.

We saw how automatic retries provide protection from transient failures like database deadlocks. If we implement a multi-step process as a series of message handlers, then each step will be executed independently and can be automatically retried in case of failures. This means that a stray exception won't abort an entire process, leaving the system in an inconsistent state.

We also implemented an additional event subscriber, showing how to decouple independent bits of business logic from each other. The ability to publish one event and then implement resulting steps in separate message handlers makes the system much easier to maintain and evolve.

SUCCESS: Now that you've seen what NServiceBus can do, take the next step and learn how to build a system like this one from the ground up. In the next tutorial, find out how to build the same solution starting from **File** > **New Project**.
