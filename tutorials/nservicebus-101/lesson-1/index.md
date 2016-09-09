---
title: "Lesson 1: Hello world"
---

Every framework needs a Hello World, and for NServiceBus, this is it. There's no time to waste, so let's get started!


## Objectives

In this lesson we will set up a new development machine with NServiceBus.

By the end of this lesson, you will have learned:

* How to set up a machine for NServiceBus development
* What a messaging endpoint is and how to create one


## Prerequisites

There are a few things we need to do in order to build a solution with the latest version of NServiceBus. First we'll concentrate on getting these set up.


### .NET Framework

NServiceBus 6.0 requires .NET Framework 4.5.2 or higher, although using at least .NET Framework 4.6 will give you access to features that will make certain things easier, such as writing async code.


### Message transport

NServiceBus is a messaging framework and requires a **message transport**, normally a message queuing technology, to move messages around. Natively, NServiceBus supports Microsoft Message Queuing (MSMQ), RabbitMQ, Azure Storage Queues, Azure Service Bus, and SQL Server as message transports. (Although SQL Server is not a message queue per se, the SQL Server Transport emulates queues within database tables.)

For the sake of simplicity, this tutorial will use MSMQ, which is the default message transport for NServiceBus. MSMQ is included with every version of Windows, although it is not installed by default.

> NOTE: In some environments, you may not have sufficient privileges on your system to install MSMQ. If this is the case, the SQL Server transport is a good substitute for this tutorial. If you don't have ready access to a SQL Server instance, you can install [SQL Server Express](http://downloadsqlserverexpress.com/) locally.

> To use the SQL Server transport for this guide, see the [SQL Server transport setup instructions](using-sql-transport.md).

The easiest way to set up MSMQ is to use the **Particular Platform Installer**. The platform installer will get several things set up that will be useful later on:

* Install the MSMQ Windows Feature and configure it correctly for NServiceBus.
* Configure the Distributed Transaction Coordinator (DTC) so that queues and databases can all work together within a single atomic transaction.
* Install tools that will make developing NServiceBus systems easier. Although we won't use them until Lesson 5, the Platform Installer will get these set up right away:
    * [ServiceControl](/servicecontrol/) - a service that stores copies of processed messages to enable features found in the other tools
    * [ServiceInsight](/serviceinsight/) - a desktop application that enables debugging and visualization of your message-based system
    * [ServicePulse](/servicepulse/) - a web application, installed as a service, that provides operations monitoring of a message-driven system

You can [download the Platform Installer now](http://particular.net/start-platform-download), read the [Platform Installer documentation](/platform/installer/), or view the [release notes](/platform/release-notes.md). You can also [configure MSMQ manually](/nservicebus/msmq/#nservicebus-configuration).


## Exercise

Now that we have a system configured for NServiceBus, let's build something simple to try it out.


### Create a solution

First, let's create a basic solution and include the dependencies we need.

1. In Visual Studio, create a new project and select the **Console Application** project type.
2. Set the project name to **ClientUI**.
3. Set the Solution Name to **RetailDemo**.

Next, we need to add the NServiceBus NuGet package as a dependency. From the NuGet Package Manager Console, type the following:

    Install-Package NServiceBus -ProjectName ClientUI

This adds a reference to the NServiceBus.Core assembly to the project. With the proper dependencies in place, we're ready to start writing code.


### Configure an endpoint

Although NServiceBus is a completely async framework, the console application's `Main()` method is not. Therefore, it's necessary to create an entry point into code where we can use the `async`/`await` keywords.

In the **Program.cs** file, modify the code to look like the following:

    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {

        }
    }

Now we're ready to create a **messaging endpoint**. A messaging endpoint (or just **endpoint**) is a logical component that's capable of sending and receiving messages. An endpoint is hosted within a process, which in this case is a simple console application, but could be a web application or other .NET process.

Let's take a look at all the code required to run an endpoint within a console application as a block, and afterward we'll analyze it line-by-line.

Add this code to your AsyncMain method:

    static async Task AsyncMain()
    {
        Console.Title = "ClientUI";

        var endpointConfig = new EndpointConfiguration("ClientUI");
        endpointConfig.UseTransport<MsmqTransport>();
        endpointConfig.UseSerialization<JsonSerializer>();
        endpointConfig.UsePersistence<InMemoryPersistence>();
        endpointConfig.SendFailedMessagesTo("error");
        endpointConfig.EnableInstallers();
    }

This really isn't many lines of code at all, but let's take it line-by-line.


#### Console Title

    Console.Title = "ClientUI";

When developing NServiceBus systems, we tend to run multiple message endpoints at the same time in separate console windows. If we're not careful, it can become difficult to tell them apart. Giving each one a short, clear name can be helpful to make it easier to find the desired window later on.

In later lessons, we'll expand our project to have several endpoints, and this one will be the test endpoint we'll use to send messages to the rest of the system. Thus we give it the name `ClientUI`.


#### EndpointConfiguration

    var endpointConfig = new EndpointConfiguration("ClientUI");

The `EndpointConfiguration` class is where we define all the settings that determine how our endpoint will operate. The single string parameter `ClientUI` is the **endpoint name**, which serves as the logical identity for our endpoint, and forms a naming convention by which other things will derive their names. Chief among these is the **input queue** where the endpoint will listen for messages to process.


#### Message transport

    endpointConfig.UseTransport<MsmqTransport>();

This setting defines the message transport that NServiceBus will use to send and receive messages. `MsmqTransport` is the only transport available within the core library. All other transports require additional NuGet packages.

The MSMQ transport is the default setting, so we technically don't need this line at all. However, if you add a period right before the semicolon (in order to see the IntelliSense) you will see that more settings related to the message transport are available as extension methods. We will explore these in later lessons. For now, it's good to make the choice of transport explicit within our code.


#### Message serializer

    endpointConfig.UseSerialization<JsonSerializer>();

When sending messages, an endpoint needs to serialize message objects to a stream, and then deserialize the stream back to a message object on the receiving end. The choice of serialization governs what format that will take.

The default serializer is the `XmlSerializer`, which is similar to the built-in .NET XML serializer. XML is the default mostly for historic reasons and to retain backward compatibility. JSON is more compact, more efficient, and easier to integrate with other systems due to its ubiquity. So we will use it instead of the default `XmlSerializer`.


#### Persistence

    endpointConfig.UsePersistence<InMemoryPersistence>();

NServiceBus needs to store some data in between handling messages. We will explore the reasons for this in future lessons but for now, we'll use an implementation that stores everything in memory. This has the advantage during development of allowing us to iterate quickly by providing us with a clean slate every time we start up.


#### Error queue

    endpointConfig.SendFailedMessagesTo("error");

When things go wrong, NServiceBus needs a place to send failed messages. Otherwise a bug in a message handler could cause a **poison message**, blocking the queue and keeping other work from getting done. This queue is referred to as the **error queue** and is commonly named `error`. We will discuss this more in [Lesson 5: Retrying errors](../lesson-5/).


#### Installers

    endpointConfig.EnableInstallers();

This setting instructs the NServiceBus endpoint to run its installers on startup. Installers are pieces of code used to set up anything the endpoint requires to run. The most common example is creating necessary queues, such as the endpoint's input queue where it will receive messages.


### Starting up

Now things start to get interesting. At the end of the `AsyncMain` method, after the configuration code, add the following code which will start up the endpoint, keep it running until we press the Enter key, and then shut it down.

    var endpointInstance = await Endpoint.Start(endpointConfig).ConfigureAwait(false);

    Console.WriteLine("Press Enter to exit...");
    Console.ReadLine();

    await endpointInstance.Stop().ConfigureAwait(false);

When the endpoint starts, the `EndpointConfiguration` information is locked down from further changes, and the settings it contains are used to initialize the endpoint.

When you run the endpoint for the first time, take note of the following:

* The endpoint displays its logging information, which is written to a file as well as the console, so if you miss something on the console, you can go back and look at the file. NServiceBus also logs to multiple levels, so you can [change the log level](/nservicebus/logging/) from `INFO` to log level `DEBUG` in order to get more information about what's going on.
* The endpoint displays the status of your license.
* The endpoint will attempt to add the current user to the "Performance Monitor Users" group so that it can write performance counters to track its health and progress. If it is not able to do so (usually because the process is not running with elevated privileges) then it will show you how to do it manually from an admin console.
* The endpoint will warn you that the queues it created have too many permissions. This makes it easier for development, but in a production scenario these queues should be created with the minimum required privileges, and these warnings will serve as reminders to do that.


## Summary

In this lesson we set up the prerequisites for NServiceBus and created a simple messaging endpoint to make sure it works. The fun doesn't really begin, however, until we start sending messages. In the next lesson, we'll define a message, a message handler, and then send the message and watch it get processed.

Before moving on, you might want to check your code against the [completed solution](solution/) to see if there's anything you may have missed.

When you're ready, move on to [**Lesson 2: Sending a command**](../lesson-2/).
