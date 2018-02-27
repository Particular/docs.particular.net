---
title: Startup and Shutdown Sequence
summary: The order of startup and shutdown operations including all extension points that plug into that process
component: Core
reviewed: 2018-02-23
related:
- nservicebus/operations/installers
---


## Code walk-through

This sample illustrates the order of startup and shutdown operations including all extension points that plug into that process. All interfaces that extend the startup and shutdown are included in an `ExtensionPoints` directory.


## Logger

At each step in the process a line is written to both the console and a text file via a logger.

snippet: Logger


## Console program

The main section of the console `Program` configures and starts the endpoint while logging all these actions.

snippet: Program


### The resulting order

Note: In some versions of NServiceBus, certain extension points are executed on separate threads.

snippet: StartupShutdownSequence
