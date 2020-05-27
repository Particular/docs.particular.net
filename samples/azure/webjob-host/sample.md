---
title: Self-Hosting in Azure WebJobs
summary: Uses NServiceBus the self-hosting capability to host an endpoint in an Azure WebJob.
component: Core
reviewed: 2020-02-26
isLearningPath: true
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

The `UseNServiceBus` method of [`NServiceBus.Extensions.Hosting`](/nservicebus/hosting/extensions-hosting.md) is used to configure and start the endpoint:

snippet: WebJobHost_Start

NOTE: If dependencies need to be shared between the service collection and NServiceBus infrastructure, such as message handlers, see the [ASP.NET Core sample](/samples/dependency-injection/#related-samples).

A critical error action must be defined to restart the host when a critical error is raised:

snippet: WebJobHost_CriticalError

When the WebJob host stops, the endpoint endpoint is automatically stopped with the host. This is taken care off by the hosting extension.