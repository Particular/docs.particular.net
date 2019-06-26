---
title: Self Hosting in Web Jobs
summary: Uses NServiceBus self-hosting capability to host an endpoint in a web job.
component: Core
reviewed: 2019-06-26
tags:
- Azure
- Hosting
related:
- nservicebus/dependency-injection/msdependencyinjection
- samples/dependency-injection/aspnetcore

---

## Running in development mode

 1. Start [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator)
 1. Run the solution

## Code walk-through

This sample contains one project:

- Receiver - Self-hosted endpoint running in a continuous web job

### Receiver project

The receiver project uses the self-hosting capability to start an end endpoint inside a continuously running web job.

The snippet below illustrates how the `StartAsync` method of `IJobHost` can be used to configure and start the endpoint

snippet: WebJobHost_Start

If dependencies need to be shared between the service collection and NServiceBus infrastructure like handlers the [MSDependencyInjection nuget package](nservicebus/dependency-injection/msdependencyinjection.md) needs to be configured.

A critical error action needs to be defined to restart the host when a critical error is raised.

snippet: WebJobHost_CriticalError

When the job host stops the endpoint needs to be shutdown to properly release all acquired ressources.

snippet: WebJobHost_Stop
