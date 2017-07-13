---
title: "Introduction to NServiceBus: Getting started"
reviewed: 2017-01-26
summary: In this 10-15 minute tutorial, you will learn how to set up a development machine for NServiceBus and create your very first messaging endpoint.
redirects:
- tutorials/nservicebus-101/lesson-1
extensions:
- !!tutorial
  nextText: "Next Lesson: Sending a command"
  nextUrl: tutorials/intro-to-nservicebus/2-sending-a-command
---

include: nsb101-intro-paragraph

In this first lesson, which should take 10-15 minutes, you will learn how to set up a new development machine for NServiceBus and create your very first messaging endpoint.


## Before we get started

NServiceBus has very few prerequisites. All it needs is the .NET Framework and message queuing infrastructure.

Although [NServiceBus only requires .NET Framework 4.5.2](/nservicebus/operations/dotnet-framework-version-requirements.md), this tutorial uses Visual Studio 2015 and .NET Framework 4.6.1, which includes some useful async-related APIs.

NServiceBus needs queuing infrastructure (a [transport](/transports/)) to move messages around. By default, this tutorial will use **Microsoft Message Queuing (MSMQ)**, which is included with every version of Windows, although it is not installed by default.

NOTE: If MSMQ is not an option for your environment, the [SQL Server Transport](/transports/sql/) can be used as a message transport instead. To get started using SQL Server instead of MSMQ, review the [SQL Server transport setup instructions](../using-sql-transport.md). Throughout the rest of the tutorial, instructions that differ between the SQL and MSMQ transports will be highlighted in an informational box like this one.

To install MSMQ on your machine, you have two options:

* [Download and run the Particular Platform Installer](https://particular.net/start-platform-download). This will install the MSMQ Windows Feature, configure the Distributed Transaction Coordinator (DTC) so that queues and databases can all work together within a single atomic transaction, and install tools from Particular ([ServiceControl](/servicecontrol/), [ServiceInsight](/serviceinsight/), and [ServicePulse](/servicepulse/)) that we'll use later in this tutorial.
* [Install and configure MSMQ manually](/transports/msmq/#nservicebus-configuration).

NOTE: It's enough at this point to simply install the Platform Installer tools. At the end of the install process, a button will offer the option to start ServiceControl Management to install or update a ServiceControl instance. This isn't required right now, but we'll return to this topic in [Lesson 5](../5-retrying-errors/) when we explore how to replay failed messages.

## Exercise

Let's build something simple to give NServiceBus a try.


### Create a solution

First, let's create a basic solution and include the dependencies we need.

 1. In Visual Studio, create a new project and select the **Console Application** project type.
 1. Set the project name to **ClientUI**.
 1. Set the solution name to **RetailDemo**.

Next, we need to add the NServiceBus NuGet package as a dependency. From the [NuGet Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console), type the following:

```
Install-Package NServiceBus -ProjectName ClientUI
```

This adds a reference to the NServiceBus.Core assembly to the project.

NOTE: If you are using the SQL Server transport, you also need to install the `NServiceBus.SqlServer` package before continuing. See [Using the SQL Server transport - Adding the NuGet package](/tutorials/intro-to-nservicebus/using-sql-transport.md#modifying-each-endpoint-adding-the-nuget-package) for more details.

With the proper dependencies in place, we're ready to start writing code.


### Configure an endpoint

Now we're ready to create a [**messaging endpoint**](/nservicebus/endpoints/). A messaging endpoint (or just **endpoint**) is a logical component that's capable of sending and receiving messages. An endpoint is hosted within a process, which in this case is a simple console application, but could be a web application or other .NET process.

Because of the current limitations of console applications, we need to add some boilerplate code to be able to use the `async`/`await` keywords.

In the **Program.cs** file, modify the code to look like the following:

snippet: EmptyProgram

Add the following code to your program first and then let's analyze the importance of each line.

Add this code to your AsyncMain method:

snippet: AsyncMain

Now, let's go line-by-line and find out exactly what each step is doing.


#### Console Title

snippet: ConsoleTitle

When running multiple console apps in the same solution, giving each a name makes them easier to identify. This console app's title uses `ClientUI`. In later lessons, we'll expand this solution to host several more.


#### EndpointConfiguration

snippet: EndpointName

The `EndpointConfiguration` class is where we define all the settings that determine how our endpoint will operate. The single string parameter `ClientUI` is the [**endpoint name**](/nservicebus/endpoints/specify-endpoint-name.md), which serves as the logical identity for our endpoint, and forms a naming convention by which other things will derive their names, such as the **input queue** where the endpoint will listen for messages to process.


#### Transport

snippet: MsmqTransport

This setting defines the [**transport**](/transports/) that NServiceBus will use to send and receive messages. We are using the `MsmqTransport`, which is bundled within the NServiceBus core library. All other transports require different NuGet packages.

Capturing the `transport` settings in a variable as shown will make things easier in [Lesson 3](../3-multiple-endpoints/) when we start defining message routing rules.

NOTE: If using the SQL Server transport, you must use the `SqlServerTransport` and provide a connection string to the database. See [Using the SQL Server transport - Configuring the transport](/tutorials/intro-to-nservicebus/using-sql-transport.md#modifying-each-endpoint-configuring-the-transport) for more details.


#### Serializer

snippet: Serializer

When sending messages, an endpoint needs to serialize message objects to a stream, and then deserialize the stream back to a message object on the receiving end. The choice of [**serializer**](/nservicebus/serialization/) governs what format that will take. Each endpoint in a system needs to use the same serializer in order to be able to understand each other.

Here, we are choosing the `XmlSerializer`.


#### Persistence

snippet: Persistence

A [**persistence**](/persistence/) is required to store some data in between handling messages. We will explore the reasons for this in future lessons but for now, we'll use an [implementation that stores everything in memory](/persistence/in-memory.md). This has the advantage during development of allowing us to iterate quickly by providing us with a clean slate every time we start up. Of course, as everything persisted is lost when the endpoint shuts down, it is not safe for production use, so we will want to replace it with a different persistence option before deployment.


#### Error queue

snippet: ErrorQueue

Processing a message can fail for several reasons. It could be due to a coding bug, a database deadlock, or unanticipated data inside a message. Automatic retries will make dealing with non-deterministic exceptions a non-issue, but for very serious errors, the message could get stuck at the top of the queue and be retried indefinitely. This type of message, known as a **poison message**, would block all other messages behind it. When these occur, NServiceBus needs to be able to set it aside in a different queue to allow other work to get done. This queue is referred to as the **error queue** and is commonly named `error`. We will discuss [**recoverability**](/nservicebus/recoverability/) more in [Lesson 5: Retrying errors](../5-retrying-errors/).


#### Installers

snippet: EnableInstallers

This setting instructs the endpoint to run [installers](/nservicebus/operations/installers.md) on startup. Installers are used to set up anything the endpoint requires to run. The most common example is creating necessary queues, such as the endpoint's input queue where it will receive messages.


### Starting up

At the end of the `AsyncMain` method, after the configuration code, add the following code which will start up the endpoint, keep it running until we press the Enter key, and then shut it down.

snippet: Startup

NOTE: In this tutorial we will always use `.ConfigureAwait(false)` when awaiting tasks, in order to [avoid capturing and restoring the SynchronizationContext](/nservicebus/handlers/async-handlers.md#usage-of-configureawait).

The endpoint is initialized according to the settings defined by the `EndpointConfiguration` class. Once the endpoint starts, changes to the configuration information are no longer applied.

When you run the endpoint for the first time, the endpoint will:

 * Display its logging information, which is written to a file as well as the console. NServiceBus also logs to multiple levels, so you can [change the log level](/nservicebus/logging/) from `INFO` to log level `DEBUG` in order to get more information.
 * Display the [status of your license](/nservicebus/licensing/).
 * Attempt to add the current user to the "Performance Monitor Users" group so that it can write [performance counters](/nservicebus/operations/performance-counters.md) to track its health and progress.
 * Create several queues:, if they do not already exist: 
   * `error`
   * `clientui`
   * `clientui.retries`
   * `clientui.timeouts`
   * `clientui.timeoutsdispatcher`

Now might be a good time to go look at your list of queues. There are a [variety of options for viewing MSMQ queues and messages](/transports/msmq/viewing-message-content-in-msmq.md) that you can pick from. In addition to the queues mentioned above, you may also see an `error.log` queue and queues starting with `particular.servicecontrol` if you [installed a ServiceControl instance](/servicecontrol/installation.md) while installing the Particular Service Platform.

When using the MSMQ transport, queues are created with [permissive settings](/transports/msmq/#permissions) that make things easier during development. In a production scenario these queues should be created with the minimum required privileges. The endpoint will write a log entry on startup when permissive settings are detected to remind you to do this.

NOTE: If you are using the SQL Server transport, take a look in your SQL database, where NServiceBus has created each of the queues listed above as a separate table.


## Summary

In this lesson we created a simple messaging endpoint to make sure it works. In the next lesson, we'll define a message, a message handler, and then send the message and watch it get processed.
