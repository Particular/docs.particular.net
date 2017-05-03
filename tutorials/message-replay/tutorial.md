---
title: Message replay tutorial
reviewed: 2017-06-09
summary: In this tutorial, you'll learn how to replay a failed message using the Particular Service Platform tools.
extensions:
- !!tutorial
  downloadAtTop: true
---

One of the most powerful features of NServiceBus is the ability to replay a message that has failed. By the time a message reaches the error queue, it will have already progressed through multiple retries through the [immediate retries](/nservicebus/recoverability/#immediate-retries) and [delayed retries](/nservicebus/recoverability/#delayed-retries) process, so you can be sure that the exception is systemic.

Often, this type of failure can be introduced by a bug that isn't found until the code is deployed. When this happens, many errors can flood into the error queue all at once. At these times, it's incredibly valuable to be able to roll back to the old version of the endpoint, and then replay the failed messages through proven code. Then you can take the time to properly troubleshoot and fix the issue before attempting a new deployment.

In this tutorial, which is based on the code developed in the [Introduction to NServiceBus tutorial](/tutorials/intro-to-nservicebus/), we'll see how to use ServiceControl to monitor an NServiceBus system, and ServicePulse to replay a failed message.

To get started, download the solution above, extract the archive, and then open the **RetailDemo.sln** file with Visual Studio 2015 or later.


## Project Structure

The solution is similar to the one built in the [Introduction to NServiceBus tutorial](/tutorials/intro-to-nservicebus/), containing five projects. The **ClientUI**, **Sales**, **Billing**, and **Shipping** projects are [endpoints](/nservicebus/endpoints/) that communicate with each other using NServiceBus messages.

The **ClientUI** endpoint mimics a web application and is an entry point in our system. The **Sales**, and **Billing**, and **Shipping** endpoints contain business logic related to processing and fulfilling orders. Each endpoint references the **Messages** assembly, which contains the definitions of messages as POCO class files.

As shown in the diagram, the **ClientUI** endpoint sends a **PlaceOrder** command to the **Sales** endpoint. As a result, the **Sales** endpoint will publish an `OrderPlaced` event using the publish/subscribe pattern, which will be received by the **Billing** and **Shipping** endpoints. Additionally, the **Billing** endpoint endpoint will publish an `OrderBilled` endpoint that will also be received by the **Shipping** endpoint.

![Project Diagram](/tutorials/intro-to-nservicebus/4-publishing-events/diagram.svg)

INFO: In a real system, the **Shipping** endpoint should be able to take some action once it receives both an `OrderPlaced` and `OrderBilled` event for the same order. That is a use case for a [**Saga**](/nservicebus/sagas/) and is outside of the scope of this tutorial.


## Production-ready message transport

The [Introduction to NServiceBus tutorial](/tutorials/intro-to-nservicebus/) on which the code is based uses the [Learning Transport](/nservicebus/learning-transport/) as its underlying [message transport](/nservicebus/transports/) to move messages around. It's great for learning the NServiceBus API and for demos, but since it does not support the tools in the [Particular Service Platform](/platform/), we need to upgrade to a production-ready transport.

This tutorial uses the [MSMQ Transport](/nservicebus/msmq/) that moves messages around using Microsoft Message Queuing.

If you completed the [Introduction to NServiceBus tutorial](/tutorials/intro-to-nservicebus/), you may want to note a few things in the code that have changed.

In the **Program.cs** file of every endpoint, the `LearningTransport` has been replaced by the `MsmqTransport`, including additional configuration that is required by MSMQ:

snippet: MsmqConfig

In this snippet we can see that:

* `UseTransport<LearningTransport>()` has been replaced by `UseTransport<MsmqTransport>()`.
* In-memory persistence has been selected. This is because MSMQ, unlike the learning transport, does not natively support Publish/Subscribe, so the message subscription information will be stored in memory instead.
* An error queue (`error`) has been specified, which was not a required setting in the learning transport. This is important because most MSMQ systems are distributed, and a centralized error queue in the form of `error@MACHINENAME` is used for the whole system.
* `EnableInstallers()` is called, which instructs MSMQ to create the message queues required by the endpoints.

In addition, there is additional routing configuration in the **Program.cs** file in both **Billing**:

snippet: BillingPubSubConfig

And in **Shipping**:

snippet: ShippingPubSubConfig

This is, again, because MSMQ does not support native Publish/Subscribe, so subscribers need to know which [logical endpoint](/nservicebus/endpoints/) to listen for events.

The last change in the solution is [disabling delayed retries](/nservicebus/recoverability/configure-delayed-retries) in the **Sales** endpoint's **Program.cs** file:

snippet: NoDelayedRetries

Since we are going to be causing a lot of messages to fail in this exercise, we'd prefer not to wait around for several rounds of delayed retries to complete.

## Setting up the platform tools

To complete this tutorial and run the solution, we will be using the [Particular Platform Installer](/platform/installer/). It will ensure that MSMQ is set up correctly, and also install two tools we need for the tutorial:

 * [ServiceControl](/servicecontrol/) is like a watchdog monitoring your system, sucking in information and making that available to other tools via a REST API. One of its functions is to monitor your error queue so that you can act on the poison messages that arrive there.
 * [ServicePulse](/servicepulse/) is a web application aimed to be an operational dashboard for your NServiceBus system. It allows you to see failed messages, including the exception details, and provides a UI to either replay or archive failed messages.

 To install the Service Platform:

 1. Download the [Platform Installer](https://particular.net/start-platform-download).
 1. Launch the **ParticularPlatform.exe** you downloaded, and use it to install the Particular Service Platform [according to the instructions](/platform/installer/).

Once that completes, we need to install an instance of ServiceControl. It's possible to install multiple instances of ServiceControl for different transports, so we need to configure a ServiceControl instance specifically for MSMQ.

You can launch the **ServiceControl Management** application in one of two ways:

 * From the **Start ServiceControl Management** button on the last screen of the Platform Installer
 * By locating **ServiceControl Management** in the Windows Start menu

Next, in the **ServiceControl Management** window, click the **Add new instance** button. There are a few customizations we will need to make here to configure ServiceControl.

First and foremost, under the **General** heading, take note of the host name and port (`localhost:33333` by default) as you will need these later.

Last, scroll down to the **Queues Configuration** heading:

1. Change the **Audit Forwarding** dropdown value to **Off**.

{{NOTE:
This setting may seem esoteric, but serves an important purpose. [Forwarding queues](/servicecontrol/errorlog-auditlog-behavior.md) settings control what happens to messages after being processed by ServiceControl. If audit forwarding is on, then copies of all messages processed will accumulate in a queue, but not get processed, eventually consuming all available disk space. On the other hand, if you wanted to do something with those messages but turned audit forwarding off, ServiceControl would consume those messages but then effectively delete them.

Because we're just getting started with NServiceBus development, we don't need to keep copies of these messages around, so we can safely set Audit Forwarding to Off.
}}

Now, we're ready to create and start the service:

 1. Click the **Add** button to install the ServiceControl instance as a Windows service.
 1. When complete, the **ServiceControl Management** tool will display the high-level details of the ServiceControl instance, but the instance will be in the **Stopped** state.
 1. In the upper-right corner of the ServiceControl instance details, click the **Start** button (the button with the *Play* icon) to start the service. You can also start the service from the Windows Services manager.

To check that everything is working properly, you can click on the link shown under **URL**, which will return a JSON response if ServiceControl is working properly. This is the API that is used to serve information to the [ServicePulse](/servicepulse/) and [ServiceInsight](/serviceinsight/) tools.

Later in the exercise, we will be using ServicePulse to replay a failed message, so we should also check to make sure it is working. ServicePulse is installed as a Windows service named **Particular ServicePulse** and has a web-based UI, which can be accessed at `http://localhost:9090` [when default settings are used](/servicepulse/host-config.md). You can check to see if it is running from the Windows Services control panel. ServicePulse must to be able to connect to the ServiceControl API, which [can be configured](/servicepulse/host-config.md#changing-the-servicecontrol-url) if a non-default ServiceControl URL is used.


## Running the solution

The solution is configured to have [multiple startup projects](https://msdn.microsoft.com/en-us/library/ms165413.aspx), so when you run the solution it should open four console applications, one for each messaging endpoint.

In the **ClientUI** application, press `P` to place an order, and watch what happens in other windows.

It may happen too quickly to see, but the `PlaceOrder` command will be sent to the **Sales** endpoint, which will publish events to **Billing** and **Shipping**. The **Billing** endpoint will then publish an event to **Shipping**. The entire process concludes with these two log messages displayed by the **Shipping** endpoint, hinting at the need for a [Saga](/nservicebus/sagas/):

```
INFO  Shipping.OrderPlacedHandler Received OrderPlaced, OrderId = 96dfd084-2bb0-46c3-b939-046e3b911102 - Should we ship now?
INFO  Shipping.OrderBilledHandler Received OrderBilled, OrderId = 96dfd084-2bb0-46c3-b939-046e3b911102 - Should we ship now?
```

## Throwing an exception

Now, let's throw an exception that will make its way to the error queue. For the purposes of this exercise, we'll create a specific bug in the Sales endpoint and watch what happens when we run the endpoint.

1. In the **Sales** endpoint, locate the **PlaceOrderHandler**.
1. Uncomment the line that throws the exception. The code in the project contains a `#pragma` directive to prevent Visual Studio from interpreting the unreachable code after the `throw` statement as a build error.

Now, run the solution.

1. In Visual Studio's **Debug** menu, select **Detach All** so that the system keeps running, but does not break into the debugger when we throw our exception.
1. In the **ClientUI** window, place an order by pressing `P`. 

In the **Sales** window, you will see a wall of text culminating in a red error trace. This is where NServiceBus gives up on the message and sends it to the error queue.

```
INFO  Sales.PlaceOrderHandler Received PlaceOrder, OrderId = e927667c-b949-47ee-8ea2-f29523909784
ERROR NServiceBus.RecoverabilityExecutor Moving message '53ac6836-48ef-49dd-aabb-a67c0104a2a5' to the error queue 'error' because processing failed due to an exception:
System.Exception: BOOM
   at < stack trace>
```

### Replay a message

Because we installed ServiceControl and ServicePulse earlier, we can attempt to replay a message:

 1. Fix the **Sales** endpoint by commenting the `throw` statement.
 1. Run the solution.
 1. Open ServicePulse at `http://localhost:9090` and navigate to the **Failed Messages** page. Note how similar messages are grouped together for easier handling.
    ![Failed Message Groups](failed-message-groups.png)
 1. Click the **View Messages** link to see details on each failed message.
    ![Failed Message Details](failed-message-details.png)
 1. Take a look at the different options for a failed message, including viewing the stack trace for the exception and viewing the message's headers and body. This information is quite useful for finding where the exception is occurring in code, and viewing the data from the message that causes it to occur.
 1. Try the various methods of replaying messages (**Retry selected** vs. **Retry all**) and watch what happens in the console windows. Note that ServiceControl executes message retry batches on a 30-second timer, so *be patient*. Eventually, the messages will be returned to their appropriate endpoints.

When the message is replayed in **Sales**, each endpoint picks up right where it left off. You should be able to see how useful this capability will be when failures happen in your real-life systems.

{{NOTE:
Our solution currently uses [in-memory persistence](/nservicebus/persistence/in-memory.md) to store subscription information. Because of this, if you restart only the Sales endpoint while the others continue to run, it will not know where to publish messages and the system will not work as intended. When using a durable persistence, this will not be an issue. When testing with in-memory persistence, restart the entire system so that subscription messages are resent as each endpoint starts up.

For more details see [Persistence in NServiceBus](/nservicebus/persistence/).
}}

## Summary

In this tutorial, we saw how to set up the Particular Service Platform tools ServiceControl and ServicePulse to replay a failed message. With this ability, we can see the details of failed messages in ServicePulse and begin to troubleshoot what went wrong.

Perhaps the message had a previously unexpected input value which caused the bug to go undetected until the code entered production. With this knowledge in hand, we can go fix the code to validate these inputs or take some other sort of corrective action. Once the new code is deployed with the fix, we can replay the message and everything will flow through the system as if the error had never happened.

If you haven't yet, you might want to check out the [Introduction to NServiceBus]() tutorial, where you'll learn how to build the solution this tutorial is based on from scratch, while learning the messaging concepts you'll need to know to build even more complex software systems with NServiceBus. 