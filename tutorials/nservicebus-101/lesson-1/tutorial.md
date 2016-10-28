---
title: "NServiceBus 101 Lesson 1: Getting started"
---

In this lesson we will set up a new development machine with NServiceBus and show how the framework is set up within an application.


## Objectives

By the end of this lesson, you will have learned:

 * How to set up a machine for NServiceBus development
 * What a messaging endpoint is and how to create one


## Prerequisites

There are a few things we need in order to build a solution with the latest version of NServiceBus.

* This course uses Visual Studio 2015 and .NET Framework 4.6.1, although [NServiceBus only requires .NET Framework 4.5.2](/nservicebus/operations/dotnet-framework-version-requirements.md).
* NServiceBus requires queuing infrastructure (a message transport) to move messages around. This course uses Microsoft Message Queuing (MSMQ) as a default. The [Particular Platform Installer](/platform/installer/) will install MSMQ and its dependencies for you. [Download and run the Platform Installer](/platform/installer/) to get started, or [follow these instructions to configure MSMQ manually](/nservicebus/msmq/#nservicebus-configuration).



## Exercise

Let's build something simple to give NServiceBus a try.


### Create a solution

First, let's create a basic solution and include the dependencies we need.

 1. In Visual Studio, create a new project and select the **Console Application** project type.
 1. Set the project name to **ClientUI**.
 1. Set the solution name to **RetailDemo**.

Next, we need to add the NServiceBus NuGet package as a dependency. From the [NuGet Package Manager Console](https://docs.nuget.org/ndocs/tools/package-manager-console), type the following:

```no-highlight
Install-Package NServiceBus -ProjectName ClientUI
```

This adds a reference to the NServiceBus.Core assembly to the project. With the proper dependencies in place, we're ready to start writing code.


### Configure an endpoint

Now we're ready to create a [**messaging endpoint**](/nservicebus/endpoints/). A messaging endpoint (or just **endpoint**) is a logical component that's capable of sending and receiving messages. An endpoint is hosted within a process, which in this case is a simple console application, but could be a web application or other .NET process.

Because of the current limitations of console applications, we need to add some boilerplate code to be able to use the `async`/`await` keywords.

In the **Program.cs** file, modify the code to look like the following:

snippet:EmptyProgram

Add the following code to your program first and then let's analyze the importance of each line.

Add this code to your AsyncMain method:

snippet:AsyncMain

Now, let's go line-by-line and find out exactly what each step is doing.


#### Console Title

snippet:ConsoleTitle

When developing NServiceBus systems, we tend to run multiple message endpoints at the same time in separate console windows. If we're not careful, it can become difficult to tell them apart. Giving each one a short, clear name can be helpful to make it easier to find the desired window later on.

When running multiple console apps in the same solution, giving each a name makes them easier to identify. This console app's title uses `ClientUI`. In later lessons, we'll expand this solution to host several more.


#### EndpointConfiguration

snippet:EndpointName

The `EndpointConfiguration` class is where we define all the settings that determine how our endpoint will operate. The single string parameter `ClientUI` is the [**endpoint name**](/nservicebus/endpoints/specify-endpoint-name.md), which serves as the logical identity for our endpoint, and forms a naming convention by which other things will derive their names, such as the **input queue** where the endpoint will listen for messages to process.


#### Message transport

snippet:Transport

This setting defines the [**message transport**](/nservicebus/transports/) that NServiceBus will use to send and receive messages. `MsmqTransport` is the only transport available within the core library. All other transports require additional NuGet packages.

The [**MSMQ transport**](/nservicebus/msmq/) is the default setting, so we technically don't need this line at all. However, if you add a period right before the semicolon (in order to see the IntelliSense) you will see that more settings related to the message transport are available as extension methods. We will explore these in later lessons. For now, it's good to make the choice of transport explicit within our code.


#### Message serializer

snippet:Serializer

When sending messages, an endpoint needs to serialize message objects to a stream, and then deserialize the stream back to a message object on the receiving end. The choice of [**message serializer**](/nservicebus/serialization/) governs what format that will take. Each endpoint in a system needs to use the same serializer in order to be able to understand each other.

Here, we are choosing the `JsonSerializer` because JSON is reasonably compact and efficient, while still being human-readable. When using JSON, it's also easier to integrate with other systems on other platforms due to its ubiquity.


#### Persistence

snippet:Persistence

NServiceBus needs [**persistence**](/nservicebus/persistence/) to store some data in between handling messages. We will explore the reasons for this in future lessons but for now, we'll use an implementation that stores everything in memory. This has the advantage during development of allowing us to iterate quickly by providing us with a clean slate every time we start up. Of course, as everything persisted is lost when the endpoint shuts down, it is not safe for production use, so we will want to replace it with a different persistence option before deployment.


#### Error queue

snippet:ErrorQueue

Processing a message can fail for several reasons. It could be due to a coding bug, a database deadlock, or unanticipated data inside a message. Automatic retries will make dealing with non-deterministic exceptions a non-issue, but for very serious errors, the message could get stuck at the top of the queue and be retried indefinitely. This type of message, known as a **poison message**, would block all other messages behind it. When these occur, NServiceBus needs to be able to set it aside in a different queue to allow other work to get done. This queue is referred to as the **error queue** and is commonly named `error`. We will discuss [**recoverability**](/nservicebus/recoverability/) more in [Lesson 5: Retrying errors](../lesson-5/).


#### Installers

snippet:EnableInstallers

This setting instructs the NServiceBus endpoint to run its [installers](/nservicebus/operations/installers.md) on startup. Installers are used to set up anything the endpoint requires to run. The most common example is creating necessary queues, such as the endpoint's input queue where it will receive messages.


### Starting up

At the end of the `AsyncMain` method, after the configuration code, add the following code which will start up the endpoint, keep it running until we press the Enter key, and then shut it down.

snippet:Startup

When the endpoint starts, the `EndpointConfiguration` information is locked down from further changes, and the settings it contains are used to initialize the endpoint.

When you run the endpoint for the first time, the endpoint will:

 * Display its logging information, which is written to a file as well as the console. NServiceBus also logs to multiple levels, so you can [change the log level](/nservicebus/logging/) from `INFO` to log level `DEBUG` in order to get more information.
 * Display the [status of your license](/nservicebus/licensing/).
 * Attempt to add the current user to the "Performance Monitor Users" group so that it can write [performance counters](/nservicebus/operations/performance-counters.md) to track its health and progress.
 * Warn you that the [queues it created have development-specific permissions that may not be required in production](/nservicebus/msmq/operations-scripting.md#create-queues-default-permissions). Creating the queues with additional permissions makes things easier during development, but in a production scenario these queues should be created with the minimum required privileges, and these warnings will serve as reminders to do that.


## Summary

In this lesson we set up the prerequisites for NServiceBus and created a simple messaging endpoint to make sure it works. In the next lesson, we'll define a message, a message handler, and then send the message and watch it get processed.

Before moving on, you might want to check your code against the completed solution (below) to see if there's anything you may have missed.

When you're ready, move on to [**Lesson 2: Sending a command**](../lesson-2/).
