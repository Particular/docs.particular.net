---
title: Self Hosting in Azure WebJobs
summary: Uses NServiceBus the self-hosting capability to host an endpoint in an Azure WebJob.
component: Core
reviewed: 2019-07-02
tags:
- Azure
- Hosting
related:
- samples/dependency-injection/aspnetcore

---

This sample is compatible with Azure WebJobs SDK 3.0.

## Running in development mode

 1. Start the [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator).
 1. Run the solution.

## Code walk-through

This sample contains one project:

- Receiver: A self-hosted endpoint running in a continuous WebJob.

### Receiver

The receiver uses the self-hosting capability to start an end endpoint inside a continuously running WebJob.

The `StartAsync` method of `IJobHost` is used to configure and start the endpoint:

snippet: WebJobHost_Start

NOTE: If dependencies need to be shared between the service collection and NServiceBus infrastructure, such as message handlers, see the [ASP.NET Core sample](samples/dependency-injection/#related-samples).

A critical error action must be defined to restart the host when a critical error is raised:

snippet: WebJobHost_CriticalError

When the WebJob host stops, the endpoint must be shutdown to properly release all acquired resources:

snippet: WebJobHost_Stop
