---
title: "Introduction to NServiceBus: Getting started"
reviewed: 2017-01-26
summary: In this 10-15 minute tutorial, you will learn how to set up a development machine for NServiceBus and create your very first messaging endpoint.
redirects:
- tutorials/nservicebus-101/lesson-1
- tutorials/intro-to-nservicebus/using-sql-transport
extensions:
- !!tutorial
  nextText: "Next Lesson: Sending a command"
  nextUrl: tutorials/intro-to-nservicebus/2-sending-a-command
---

include: nsb101-intro-paragraph

In this first lesson, which should take 10-15 minutes, you will create your very first messaging endpoint.


## Exercise

Let's build something simple to give NServiceBus a try.

Although [NServiceBus only requires .NET Framework 4.5.2](/nservicebus/operations/dotnet-framework-version-requirements.md), this tutorial assumes at least [Visual Studio 2017](https://www.visualstudio.com/downloads/) and .NET Framework 4.6.1.


### Create a solution

First, let's create a basic solution and include the dependencies we need.

 1. In Visual Studio, create a new project and select the **Console Application** project type.
 1. Be sure to select the correct .NET Framework version from the dropdown at the top of the dialog. You'll want at least .NET Framework 4.6.1 for access to the convenient [Task.CompletedTask](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.completedtask.aspx) API.
 1. Set the project name to **ClientUI**.
 1. Set the solution name to **RetailDemo**.

Next, we need to add the NServiceBus NuGet package as a dependency. From the [NuGet Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console), type the following:

```
Install-Package NServiceBus -ProjectName ClientUI
```

This adds a reference to the NServiceBus.Core assembly to ClientUI. Now we're ready to start writing code.


### Configure an endpoint

Now we're ready to create a [**messaging endpoint**](/nservicebus/endpoints/). A messaging endpoint (or just **endpoint**) is a logical component that's capable of sending and receiving messages. An endpoint is hosted within a process, which in this case is a simple console application, but could be a web application or other .NET process.

If you [enable C# 7.1 features](https://www.meziantou.net/2017/08/24/3-ways-to-enable-c-7-1-features), you can take advantage of the [Async Main](https://blogs.msdn.microsoft.com/mazhou/2017/05/30/c-7-series-part-2-async-main/) feature and avoid some boilerplate code.

In the **Program.cs** file, modify the code to look like the following:

snippet: EmptyProgram

If you can't use C# 7.1 features, you can bootstrap a static async method using `.GetAwaiter().GetResult()`:

snippet: PreCsharp7-1AsyncBoilerplate

Now that we have a `Main` (or `AsyncMain`) method, let's take a look at the code we're going to add to it, and then we'll analyze the importance of each line. First, add the following code to your `Main` (or `AsyncMain`) method:

snippet: AsyncMain

Now, let's go line-by-line and find out exactly what each step is doing.


#### Console Title

snippet: ConsoleTitle

When running multiple console apps in the same solution, giving each a name makes them easier to identify. This console app's title uses `ClientUI`. In later lessons, we'll expand this solution to host several more.


#### EndpointConfiguration

snippet: EndpointName

The `EndpointConfiguration` class is where we define all the settings that determine how our endpoint will operate. The single string parameter `ClientUI` is the [**endpoint name**](/nservicebus/endpoints/specify-endpoint-name.md), which serves as the logical identity for our endpoint, and forms a naming convention by which other things will derive their names, such as the **input queue** where the endpoint will listen for messages to process.


#### Transport

snippet: LearningTransport

This setting defines the [**transport**](/transports/) that NServiceBus will use to send and receive messages. We are using the [LearningTransport](/transports/learning/), which is bundled within the NServiceBus core library as a starter transport to learn how to use NServiceBus without any additional dependencies. All other transports are provided using different NuGet packages.

Capturing the `transport` settings in a variable as shown will make things easier in [Lesson 3](../3-multiple-endpoints/) when we start defining message routing rules.


### Starting up

At the end of the `AsyncMain` method, after the configuration code, add the following code which will start up the endpoint, keep it running until we press the Enter key, and then shut it down.

snippet: Startup

NOTE: In this tutorial we will always use `.ConfigureAwait(false)` when awaiting tasks, in order to [avoid capturing and restoring the SynchronizationContext](/nservicebus/handlers/async-handlers.md#usage-of-configureawait).

The endpoint is initialized according to the settings defined by the `EndpointConfiguration` class. Once the endpoint starts, changes to the configuration information are no longer applied.

When you run the endpoint for the first time, the endpoint will:

 * Display its logging information, which is [written to a file, Trace and the Console](/nservicebus/logging/#default-logging). NServiceBus also logs to multiple levels, so you can [change the log level](/nservicebus/logging/#logging-levels) from `INFO` to log level `DEBUG` in order to get more information.
 * Display the [status of your license](/nservicebus/licensing/).
 * Attempt to add the current user to the "Performance Monitor Users" group so that it can write [performance counters](/monitoring/metrics/performance-counters.md) to track its health and progress.
 * Create fake, file-based "queues" in a `.learningtransport` directory inside your solution directory. It would be a good idea to add `.learningtransport` to your source control system's ignore file.


## Summary

In this lesson we created a simple messaging endpoint to make sure it works. In the next lesson, we'll define a message, a message handler, and then send the message and watch it get processed.
