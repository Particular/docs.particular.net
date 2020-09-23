---
title: "NServiceBus Step-by-step: Getting started"
reviewed: 2020-08-12
summary: In this 10-15 minute tutorial, you will learn how to set up a development machine for NServiceBus and create your very first messaging endpoint.
redirects:
- tutorials/intro-to-nservicebus/1-getting-started
- tutorials/nservicebus-101/lesson-1
- tutorials/intro-to-nservicebus/using-sql-transport
extensions:
- !!tutorial
  nextText: "Next Lesson: Sending a command"
  nextUrl: tutorials/nservicebus-step-by-step/2-sending-a-command
---

include: nsb101-intro-paragraph

In this first lesson, which should take 10-15 minutes, you will create your first messaging endpoint.


## Exercise

Let's build something simple to give NServiceBus a try.

Although [NServiceBus only requires .NET Framework 4.5.2](/nservicebus/operations/dotnet-framework-version-requirements.md), this tutorial assumes at least [Visual Studio 2017](https://www.visualstudio.com/downloads/) and .NET Framework 4.6.1.

NOTE: You can also build the solution in .NET Core by selecting the **Console App (.NET Core)** project type when you create a new project.


### Create a solution

First, let's create a basic solution and include the dependencies we need.

 1. In Visual Studio, create a new project and select the **Console App** project type.
 2. Be sure to select the correct .NET Framework version from the dropdown at the top or bottom of the dialog, depending on your Visual Studio version. .NET Framework 4.6.1 or higher is recommended for access to the [Task.CompletedTask](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.completedtask) API. (This option doesn't appear if you've chosen a .NET Core project type.)
 3. Set the project name to **ClientUI**.
 4. Set the solution name to **RetailDemo**.

Next, add the NServiceBus NuGet package as a dependency. From the [NuGet Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console), type the following:

```
Install-Package NServiceBus -ProjectName ClientUI
```

This adds an NServiceBus.Core assembly reference to the ClientUI project. Now we're ready to start writing code.


### Configure an endpoint

We're ready to create a [**messaging endpoint**](/nservicebus/endpoints/). A messaging endpoint (or just **endpoint**) is a logical component capable of sending and receiving messages. An endpoint is hosted within a process, which in this case is a simple console application, but could be a web application or other .NET process.

If you [enable C# 7.1 features](https://www.meziantou.net/2017/08/24/4-ways-to-enable-the-latest-c-features), you can take advantage of the [Async Main](https://docs.microsoft.com/en-us/archive/blogs/mazhou/c-7-series-part-2-async-main) feature and avoid some boilerplate code.

In the **Program.cs** file, modify the code to look like the following:

snippet: EmptyProgram

{{NOTE:
For the sake of brevity, code snippets in this tutorial do not contain the `using` statements needed to import namespaces. If you're using Visual Studio, unknown references such as `Task` or NServiceBus types will generate a "red squiggly" underline effect. If you hover or click on the red squiggly, you can click on the "light bulb" icon or press <span style="white-space: nowrap"><kbd>Ctrl</kbd> + <kbd>.</kbd></span> to see the available fixes and insert the appropriate `using` statements for the missing namespaces.

Alternatively, in the code snippet's **Copy/Edit** menu you will find a **Copy usings** item that will copy the namespaces used by the snippet to your clipboard.
}}

If you can't use C# 7.1 features, you can bootstrap a static async method using `.GetAwaiter().GetResult()`:

snippet: PreCsharp7-1AsyncBoilerplate

Now that we have a `Main` (or `AsyncMain`) method, let's take a look at the code we're going to add to it, then analyze the importance of each line. First, add the following code to the `Main` (or `AsyncMain`) method:

snippet: AsyncMain

Now, let's go line-by-line and find out exactly what each step is doing.


#### Console Title

snippet: ConsoleTitle

When running multiple console apps in the same solution, giving each a name makes them easier to identify. This console app's title is `ClientUI`. In later lessons, we'll expand this solution to host several more.


#### EndpointConfiguration

snippet: EndpointName

The `EndpointConfiguration` class is where we define all the settings that determine how our endpoint will operate. The single string parameter `ClientUI` is the [**endpoint name**](/nservicebus/endpoints/specify-endpoint-name.md), which serves as the logical identity for our endpoint, and forms a naming convention by which other things will derive their names, such as the **input queue** where the endpoint will listen for messages to process.


#### Transport

snippet: LearningTransport

This setting defines the [**transport**](/transports/) that NServiceBus will use to send and receive messages. We are using the [Learning transport](/transports/learning/), which is bundled in the NServiceBus core library as a starter transport for learning how to use NServiceBus without additional dependencies. All other transports are provided using different NuGet packages.

Capturing the `transport` settings in a variable as shown will make things easier in [a later lesson](../3-multiple-endpoints/) when we start defining message routing rules.


### Starting up

At the end of the `Main (or `AsyncMain`) method, after the configuration code, add the following code which will: start the endpoint, keep it running until we press the <kbd>Enter</kbd> key, then shut it down.

snippet: Startup

NOTE: In this tutorial we will always use `.ConfigureAwait(false)` when awaiting tasks in order to [avoid capturing and restoring the `SynchronizationContext`](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming).

The endpoint is initialized according to the settings defined by the `EndpointConfiguration` class. Once the endpoint starts, changes to the configuration information are no longer applied.

When you run the endpoint for the first time, the endpoint will:

 * Display its logging information, which is [written to a file, Trace, and the Console](/nservicebus/logging/#default-logging). NServiceBus also logs to multiple levels, so you can [change the log level](/nservicebus/logging/#default-logging-changing-the-defaults-changing-the-logging-level) from `INFO` to `DEBUG` in order to get more information.
 * Display the [status of your license](/nservicebus/licensing/).
 * Attempt to add the current user to the "Performance Monitor Users" group so that it can write [performance counters](/monitoring/metrics/performance-counters.md) to track its health and progress.
 * Create fake, file-based "queues" in a `.learningtransport` directory inside your solution directory. It is recommended to add `.learningtransport` to your source control system's ignore file.


## Summary

In this lesson, we created a simple messaging endpoint to make sure it works. In the next lesson, we'll define a message and a message handler, then send the message and watch it get processed.
