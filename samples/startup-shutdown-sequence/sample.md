---
title: Startup and Shutdown Sequence
summary: The sequence of steps in an endpoint startup and shutdown process, including all available public extension points
component: Core
reviewed: 2024-11-19
related:
- nservicebus/operations/installers
---


## Code walk-through

The sample illustrates the order of startup and shutdown steps for an NServiceBus endpoint. All usages of the public extension points API are stored in the `ExtensionPoints` folder of the project.


## Logger

For each step in the process, there is a corresponding log entry written both to the console and a text file.

snippet: Logger


## Console program

The main section of the console `Program` configures and starts the endpoint while logging all these actions.

snippet: Program


### The resulting order

> [!NOTE]
> In some versions of NServiceBus, certain extension points are executed on separate threads.

snippet: StartupShutdownSequence
