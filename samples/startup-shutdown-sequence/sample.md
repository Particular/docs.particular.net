---
title: Startup and Shutdown Sequence
summary: The order of startup shutdown operations including all extension points that plug into that process.
component: Core
reviewed: 2016-03-30
related:
- nservicebus/operations/installers
---


## Code walk-through

Illustrates the order of startup shutdown operations including all extension points that plug into that process. So all interfaces that extend the startup and shutdown are included in an `ExtensionPoints` directory.


## Logger

At each step in the process a line is written to both the console and a text file via a logger.

snippet: Logger


## Console Program

The main of the console `Program` configures and starts the bus while logging all these actions.

snippet: Program


### The resulting order

Note: In some versions of NServiceBus certain extension points are executed on separate threads.

snippet: StartupShutdownSequence